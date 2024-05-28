using MicroserviceAuth.DTO;
using MicroserviceAuth.Models;
using MicroserviceAuth.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            Result result = new Result();
            var resultLogin = await _authService.Login(model);

            if(resultLogin.Usuario == null)
            {
                result.Success = false;
                result.MessageError = "Usuario o Contraseña incorrecta";
                return BadRequest(result);
            }
            result.Data = resultLogin;
            return Ok(result);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] DTO.RegisterRequest usuario)
        {
            Result result = new Result();

            var errorMessage = await _authService.Register(usuario);
            if (errorMessage.Length > 0)
            {
                result.Success = false;
                result.MessageError = errorMessage;
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }
    }
}
