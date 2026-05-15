using FixFlow.API.Entities.WorkOrderModels;

namespace FixFlow.API.Entities;

public class Product
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Sku { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    
    public Guid TenantId { get; set; }
    public Tenant? Tenant { get; set; }

    public ICollection<WorkOrderProduct> WorkOrderProducts { get; set; } = [];
}