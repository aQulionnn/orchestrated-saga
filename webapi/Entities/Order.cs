namespace webapi.Entities;

public class Order
{
    public Guid  Id { get; init; }
    public required string CustomerName { get; set; }
    public double TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
}