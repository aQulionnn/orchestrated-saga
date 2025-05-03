using Microsoft.EntityFrameworkCore;
using webapi.Entities;

namespace webapi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<InventoryReservation> InventoryReservations { get; set; }
    public DbSet<Payment> Payments { get; set; }
}