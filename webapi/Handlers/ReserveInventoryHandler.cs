using MassTransit;
using webapi.Data;
using webapi.Entities;
using webapi.Messages;

namespace webapi.Handlers;

public class ReserveInventoryHandler(AppDbContext dbContext) : IConsumer<ReserveInventory>
{
    private readonly AppDbContext _dbContext = dbContext;
    
    public async Task Consume(ConsumeContext<ReserveInventory> context)
    {
        var reservation = await _dbContext.InventoryReservations.AddAsync(new InventoryReservation
        {
            Id = Guid.NewGuid(),
            ProductName = context.Message.ProductName,
            Quantity = context.Message.Quantity,
        });
        
        await _dbContext.SaveChangesAsync();

        await context.Publish(new InventoryReserved
        {
            OrderId = context.Message.OrderId,
            ProductName = context.Message.ProductName,
            Quantity = context.Message.Quantity,
        });
    }
}