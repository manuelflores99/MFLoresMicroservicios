using MVC.Services.Interfaces;

namespace MVC.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public string Get()
        {
            throw new NotImplementedException();
        }

        public void Set(string token)
        {
            _contextAccessor.HttpContext.Response.Cookies.Append("TokenCookie", token);
        }
    }
}
