namespace FixFlow.API.Entities.WorkOrderModels
{
    public class WorkOrderHistory
    {
        public Guid Id { get; set; }
        public required string PreviousStatus { get; set; }
        public required string NewStatus { get; set; }  
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;

        public double? GpsLatitude { get; set; }
        public double? GpsLongitude { get; set; }

        public Guid WorkOrderId { get; set; }
        public WorkOrder? WorkOrder { get; set; }

        public Guid ChangedByUserId { get; set; }
        public User? ChangedByUser { get; set; }

        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }

    }
}
