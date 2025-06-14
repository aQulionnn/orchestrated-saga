using MassTransit;
using webapi.Messages;

namespace webapi.Sagas;

public class OrderProcessingSaga : MassTransitStateMachine<OrderProcessingSagaData>
{
    public State Pending { get; set; }
    public State ReservingInventory { get; set; }
    public State ProcessingPayment { get; set; }
    
    public Event<OrderCreated> OrderCreatedEvent { get; set; }
    public Event<InventoryReserved> InventoryReservedEvent { get; set; }
    public Event<PaymentSucceeded> PaymentSucceededEvent { get; set; }

    public OrderProcessingSaga()
    {
        InstanceState(x => x.CurrentState);
        
        Event(() => OrderCreatedEvent, e => e.CorrelateById(m => m.Message.OrderId));
        Event(() => InventoryReservedEvent, e => e.CorrelateById(m => m.Message.OrderId));
        Event(() => PaymentSucceededEvent, e => e.CorrelateById(m => m.Message.OrderId));

        Initially(
            When(OrderCreatedEvent)
                .Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.CustomerName = context.Message.CustomerName;
                    context.Saga.TotalAmount = context.Message.TotalAmount;
                    context.Saga.CreatedAt = context.Message.CreatedAt;
                    context.Saga.OrderCreated = true;
                })
                .TransitionTo(Pending)
                .Publish(context => new ReserveInventory(context.Message.OrderId, "Laptop", 1)));

        During(Pending,
            When(InventoryReservedEvent)
                .Then(context =>
                {
                    context.Saga.ProductName = context.Message.ProductName;
                    context.Saga.Quantity = context.Message.Quantity;
                    context.Saga.InventoryReserved = true;
                })
                .TransitionTo(ReservingInventory)
                .Publish(context => new ChargePayment(context.Message.OrderId, context.Saga.TotalAmount)));


        During(ReservingInventory,
            When(PaymentSucceededEvent)
                .Then(context =>
                {
                    context.Saga.PaymentCompleted = true;
                    context.Saga.Amount = context.Message.Amount;
                })
                .TransitionTo(ProcessingPayment)
                .Finalize());
    }
}