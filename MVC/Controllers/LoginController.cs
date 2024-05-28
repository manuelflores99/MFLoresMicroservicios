using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MVC.DTO;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MVC.Services.Interfaces;

namespace MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenProvider _tokenProvider;

        public LoginController(IConfiguration configuration, ITokenProvider tokenProvider)
        {
            _configuration = configuration;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            DTO.LoginRequest loginRequest = new DTO.LoginRequest();

            return View(loginRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Login(DTO.LoginRequest loginRequest)
        {
            using (HttpClient cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri(_configuration["ApiAuth"]);

                var task = cliente.PostAsJsonAsync<LoginRequest>("Login", loginRequest);

                task.Wait();

                var taskResult = task.Result;

                if (taskResult.IsSuccessStatusCode)
                {
                    var taskContent = taskResult.Content.ReadAsAsync<Result>();
                    taskContent.Wait();

                    var response = JsonConvert.DeserializeObject<LoginResponsive>(taskContent.Result.Data.ToString());

                    await SignInUser(response);

                    _tokenProvider.Set(response.Token);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("auth", "Error al iniciar sesión");
                }
            }
            return View(loginRequest);
        }

        private async Task SignInUser(LoginResponsive loginResponsive)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(loginResponsive.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
              jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
              jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
