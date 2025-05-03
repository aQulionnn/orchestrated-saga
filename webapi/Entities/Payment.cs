namespace webapi.Entities;

public class Payment
{
    public Guid Id { get; init; }
    public Guid OrderId { get; init; }
    public double Amount { get; set; }
}