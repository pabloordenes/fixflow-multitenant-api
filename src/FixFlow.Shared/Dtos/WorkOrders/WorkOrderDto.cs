
namespace FixFlow.Shared.Dtos.WorkOrders;

public record WorkOrderDto
{
    public string Id { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public string? Description { get; init; }

    // public WorkOrderStatus Status { get; init; }

    public DateTime ScheduledDate { get; init; } //fehca programada de la orden, define cuando el tecnico debe ejecutarla 

    public string CustomerId { get; init; } = string.Empty; //identifica al cliente al que pertenece la orden de trabajo

    public string TechnicianId { get; init; } = string.Empty; //define que tecnico tiene asignada la orden

    public byte[] RowVersion { get; init; } = Array.Empty<byte>(); 
}