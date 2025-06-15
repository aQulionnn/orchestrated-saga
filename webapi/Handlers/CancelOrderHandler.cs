using MassTransit;
using webapi.Data;
using webapi.Messages;

namespace webapi.Handlers;

public class CancelOrderHandler(AppDbContext dbContext) : IConsumer<CancelOrder>
{
    private readonly AppDbContext _dbContext = dbContext;
    
    public async Task Consume(ConsumeContext<CancelOrder> context)
    {
        var order = await _dbContext.Orders.FindAsync(context.Message.OrderId);
        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync();

        await context.Publish(new OrderCreationFailed
        {
            OrderId = order.Id
        });
    }
}