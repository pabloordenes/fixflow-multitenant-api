using FixFlow.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FixFlow.API.Entities.WorkOrderModels;

public class WorkOrder : IMustHaveTenant
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Title { get; set; }

    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    public WorkOrderStatus Status { get; set; } = WorkOrderStatus.Pending;

    public DateTime ScheduledDate { get; set; }

    [MaxLength(200)]
    public required string Address { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    // Relación Multi-Tenant
    public Guid TenantId { get; set; }
    public Tenant? Tenant { get; set; }

    // relacion con el customer
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }

    // Relación con Técnico (Usuario)
    public Guid? AssignedTechnicianId { get; set; }

    [ForeignKey("AssignedTechnicianId")]
    public User? AssignedTechnician { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public ICollection<WorkOrderProduct> Products { get; set; } = [];
    public ICollection<WorkOrderAttachment> Attachments { get; set; } = [];
    public ICollection<WorkOrderHistory> HistoryLogs { get; set; } = [];
}