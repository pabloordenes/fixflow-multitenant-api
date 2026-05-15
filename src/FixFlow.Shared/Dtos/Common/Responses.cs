using System;
using System.Collections.Generic;
using System.Text;

namespace FixFlow.Shared.Dtos.Common;

public record PaginatedResponse<T>
{
    public List<T> Items { get; init; } = new();
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
}