using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Entities;
using webapi.Messages;

namespace webapi.Controllers;

[Route("api/orders/")]
[ApiController]
public class OrderController(IBus bus, AppDbContext context) : ControllerBase
{
    private readonly IBus _bus = bus;
    private readonly AppDbContext _context = context;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        await _bus.Publish(new CreateOrder(request.CustomerName, request.TotalAmount, DateTime.Now));
        
        return Accepted();
    }

    [HttpGet]
    public IActionResult Get()
    {
        var orders = _context.Orders.Take(2);
        var inventories = _context.InventoryReservations.Take(2);
        var payments = _context.Payments.Take(2);
        
        return Ok(new
        {
            orders,
            inventories,
            payments
        });
    }
}

public class CreateOrderRequest
{
    public required string CustomerName { get; set; }
    public double TotalAmount { get; set; }
}