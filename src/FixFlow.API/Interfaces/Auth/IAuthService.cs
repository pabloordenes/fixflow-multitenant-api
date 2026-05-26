using FixFlow.Shared.Dtos.Auth.Login;
using FixFlow.Shared.Dtos.Auth.Register;

namespace FixFlow.API.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task<bool> RegisterTenantAsync(RegisterTenantRequestDto request);
    }
}
