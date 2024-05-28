using Microsoft.AspNetCore.Identity;

namespace MicroserviceAuth.Services.Interfaces
{
    public interface IJWTokenGenerator
    {
        string GenerateToken(IdentityUser user);
    }
}
