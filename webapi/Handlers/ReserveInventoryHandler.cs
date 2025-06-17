using MassTransit;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Entities;
using webapi.Messages;

namespace webapi.Handlers;

public class ReserveInventoryHandler(AppDbContext dbContext) : IConsumer<ReserveInventory>
{
    private readonly AppDbContext _dbContext = dbContext;
    
    public async Task Consume(ConsumeContext<ReserveInventory> context)
    {
        var reservation = new InventoryReservation
        {
            Id = Guid.NewGuid(),
            ProductName = context.Message.ProductName,
            Quantity = context.Message.Quantity,
        };

        try
        {
            await _dbContext.InventoryReservations.AddAsync(reservation);
            await _dbContext.SaveChangesAsync();
            
            await context.Publish(new InventoryReserved
            {
                OrderId = context.Message.OrderId,
                InventoryId = reservation.Id,
                ProductName = context.Message.ProductName,
                Quantity = context.Message.Quantity,
            });
            
            throw new Exception();
        }
        catch (Exception e)
        {
            await context.Publish(new InventoryReleased
            {
                InventoryId = reservation.Id,
            });
        }
    }
}