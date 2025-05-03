using MassTransit;

namespace webapi.Sagas;

public class OrderProcessingSagaData : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    
    public required string CurrentState { get; set; }
    public Guid OrderId { get; set; }
    public required string CustomerName { get; set; }
    
    public required string ProductName { get; set; }
    public int Quantity { get; set; }
    public double TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public double Amount { get; set; }
    
    public bool OrderCreated { get; set; }
    public bool InventoryReserved { get; set; }
    public bool PaymentCompleted { get; set; }
}