namespace webapi.Messages;

public record CreateOrder(string CustomerName, double TotalAmount, DateTime CreatedAt);
public record CancelOrder(Guid OrderId);

public record ReserveInventory(Guid OrderId, string ProductName, int Quantity);
public record ReleaseInventory(Guid InventoryId); 
    
public record ChargePayment(Guid OrderId, double Amount);
