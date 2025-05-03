using MassTransit;
using webapi.Data;
using webapi.Entities;
using webapi.Messages;

namespace webapi.Handlers;

public class ChargePaymentHandler(AppDbContext dbContext) : IConsumer<ChargePayment>
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task Consume(ConsumeContext<ChargePayment> context)
    {
        var payment = await _dbContext.Payments.AddAsync(new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = context.Message.OrderId,
            Amount = context.Message.Amount
        });
        
        await _dbContext.SaveChangesAsync();

        await context.Publish(new PaymentSucceeded
        {
            OrderId = context.Message.OrderId,
            Amount = context.Message.Amount
        });
    }
}