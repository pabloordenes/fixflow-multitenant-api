using FixFlow.Shared.Dtos.Auth.Login;

namespace FixFlow.API.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}
