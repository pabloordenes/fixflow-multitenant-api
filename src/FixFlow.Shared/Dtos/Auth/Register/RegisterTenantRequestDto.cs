using System;
using System.Collections.Generic;
using System.Text;

namespace FixFlow.Shared.Dtos.Auth.Register
{
    public class RegisterTenantRequestDto
    {
        public required string CompanyName { get; init; }
        public required string CompanyRut { get; init; }
        public string? Services { get; init; }

        public required string FullName { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }

        public string? UserRut { get; init; }
    }
}
