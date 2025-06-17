using MassTransit;
using webapi.Data;
using webapi.Entities;
using webapi.Messages;

namespace webapi.Handlers;

public class CreateOrderHandler(AppDbContext dbContext) : IConsumer<CreateOrder> 
{
    private readonly AppDbContext _dbContext = dbContext;
    
    public async Task Consume(ConsumeContext<CreateOrder> context)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerName = context.Message.CustomerName,
            TotalAmount = context.Message.TotalAmount,
            CreatedAt = context.Message.CreatedAt
        };
        
        try
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            // throw new Exception();
            
            await context.Publish(new OrderCreated
            {
                OrderId = order.Id,
                CustomerName = order.CustomerName,
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt
            });
        }
        catch (Exception e)
        {
            await context.Publish(new OrderCancelled
            {
                OrderId = order.Id,
            });
        }
    }
}