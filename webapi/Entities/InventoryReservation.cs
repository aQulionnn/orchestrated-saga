namespace webapi.Entities;

public class InventoryReservation
{
    public Guid Id { get; init; }
    public required string ProductName { get; set; }
    public int Quantity { get; set; }
}