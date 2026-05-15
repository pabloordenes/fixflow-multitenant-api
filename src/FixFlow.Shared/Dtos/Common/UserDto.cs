using System;
using System.Collections.Generic;
using System.Text;

namespace FixFlow.Shared.Dtos.Common;

public class UserDto
{
    public string Id { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string FullName { get; init; } = string.Empty;

    public string TenantId { get; init; } = string.Empty;

    public string Role { get; init; } = string.Empty;
}