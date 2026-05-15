namespace FixFlow.API.Entities.WorkOrderModels
{

    public enum AttachmentType
    {
        PhotoBefore = 0,
        PhotoDuring = 1,
        PhotoAfter = 2,
        ClientSignature = 3,
        Document = 4
    }
    public class WorkOrderAttachment
    {

        public Guid Id { get; set; }
        public required string FileUri { get; set; }
        public AttachmentType Type { get; set; }
        public DateTime UploadedAtUtc { get; set; } = DateTime.UtcNow;

        public Guid WorkOrderId { get; set; }
        public WorkOrder? WorkOrder { get; set; }

        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }
    }
}
