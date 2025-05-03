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
        var order = await _dbContext.Orders.AddAsync(new Order
        {
            Id = Guid.NewGuid(),
            CustomerName = context.Message.CustomerName,
            TotalAmount = context.Message.TotalAmount,
            CreatedAt = context.Message.CreatedAt,
        });
        
        await _dbContext.SaveChangesAsync();

        await context.Publish(new OrderCreated
        {
            OrderId = order.Entity.Id,
            CustomerName = order.Entity.CustomerName,
            TotalAmount = order.Entity.TotalAmount,
            CreatedAt = order.Entity.CreatedAt
        });
    }
}