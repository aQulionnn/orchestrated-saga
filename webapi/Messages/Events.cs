namespace webapi.Messages;

public class OrderCreated
{
    public Guid OrderId { get; init; }
    public required string CustomerName { get; set; }
    public double TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class OrderCancelled
{
    public Guid OrderId { get; set; }
}

public class InventoryReserved
{
    public Guid OrderId { get; init; }
    public Guid InventoryId { get; init; }
    public required string ProductName { get; set; }
    public int Quantity { get; set; }
}

public class InventoryReleased
{
    public Guid InventoryId { get; set; }
}

public class PaymentSucceeded
{
    public Guid OrderId { get; init; }
    public double Amount { get; set; }
}
