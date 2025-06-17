using MassTransit;
using webapi.Data;
using webapi.Messages;

namespace webapi.Handlers;

public class ReleaseInventoryHandler(AppDbContext dbContext) : IConsumer<ReleaseInventory>
{
    private readonly AppDbContext _dbContext = dbContext;
    
    public async Task Consume(ConsumeContext<ReleaseInventory> context)
    {
        var inventory = await _dbContext.InventoryReservations.FindAsync(context.Message.InventoryId);
        _dbContext.InventoryReservations.Remove(inventory);
        await _dbContext.SaveChangesAsync();

        await context.Publish(new InventoryReleased
        {
            InventoryId = context.Message.InventoryId,
        });
    }
}