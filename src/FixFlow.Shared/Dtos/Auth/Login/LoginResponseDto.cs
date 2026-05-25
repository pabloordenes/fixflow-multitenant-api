using FixFlow.Shared.Dtos.Common;


namespace FixFlow.Shared.Dtos.Auth.Login;

public class LoginResponseDto
{
    public string Token { get; init; } = string.Empty;

    public DateTime ExpiresAt { get; init; }

    public string TenantId { get; init; } = string.Empty;

    public UserDto User { get; init; } = default!;

}
