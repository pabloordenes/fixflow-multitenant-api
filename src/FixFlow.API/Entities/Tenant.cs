using FixFlow.API.Entities.WorkOrderModels;

namespace  FixFlow.API.Entities;

public class Tenant
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string TaxId { get; set; } // RUT
    public string Industry { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string Plan { get; set; } = "Free";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<User> Users { get; set; } = [];
    public ICollection<Product> Products { get; set; } = [];
    public ICollection<WorkOrder> WorkOrders { get; set; } = [];
}

