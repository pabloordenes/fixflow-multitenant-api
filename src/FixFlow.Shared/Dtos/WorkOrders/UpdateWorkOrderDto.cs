using System;
using System.Collections.Generic;
using System.Text;

namespace FixFlow.Shared.Dtos.WorkOrders;

public record UpdateWorkOrderDto
{
    public string Id { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public string? Description { get; init; }

    // public WorkOrderStatus Status { get; init; }

    public byte[] RowVersion { get; init; } = Array.Empty<byte>();
}