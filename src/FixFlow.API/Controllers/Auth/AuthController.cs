using Azure;
using FixFlow.API.Interfaces.Auth;
using FixFlow.Shared.Dtos.Auth.Login;
using FixFlow.Shared.Dtos.Auth.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixFlow.API.Controllers.Auth
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) 
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);

                if (response == null)
                {
                    return Unauthorized(new { message = "Correo o contraseña incorrectos." });
                }

                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Ocurrió un error al procesar la solicitud." });
            }
        }


        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterTenantRequestDto request)
        {
            try
            {
                var result = await _authService.RegisterTenantAsync(request);

                if (result)
                {
                    return Ok(new { message = "Organización y cuenta administradora creadas exitosamente." });
                }

                return BadRequest(new { message = "No se pudo procesar la solicitud de registro." });
            }
            catch (InvalidOperationException ex)
            {
                // El porqué del 409 Conflict: Es el estándar REST cuando un recurso (Email) ya existe.
                return Conflict(new { message = ex.Message });
            }
            catch (Exception)
            {
                // El porqué del 500: Protege los detalles de la infraestructura (fugas de trazas SQL).
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Ocurrió un error inesperado en el servidor." });
            }
        }


    }
}
