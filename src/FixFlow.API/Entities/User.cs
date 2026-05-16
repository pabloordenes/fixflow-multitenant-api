using FixFlow.API.Entities.WorkOrderModels;
using FixFlow.Shared.Interfaces;

namespace FixFlow.API.Entities;

public class User : IMustHaveTenant
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public string? TaxId { get; set; } // RUT
    public required string Role { get; set; }
    public string? AvatarUrl { get; set; }
    
    public Guid TenantId { get; set; }
    public Tenant? Tenant { get; set; }

}