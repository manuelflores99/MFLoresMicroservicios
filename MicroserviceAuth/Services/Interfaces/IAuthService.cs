using MicroserviceAuth.DTO;

namespace MicroserviceAuth.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<LoginResponsive> Login(LoginRequest model);
        public Task<string> Register(RegisterRequest usuario);
    }
}
