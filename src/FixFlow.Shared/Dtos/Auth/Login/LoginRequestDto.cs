using System;
using System.Collections.Generic;
using System.Text;

namespace FixFlow.Shared.Dtos.Auth.Login;

public class LoginRequestDto
{
    public required string Email { get; init; } = string.Empty;
    public required string Password { get; init; } = string.Empty;

}
