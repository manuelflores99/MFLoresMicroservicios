using Azure;
using MicroserviceAuth.DTO;
using MicroserviceAuth.Models;
using MicroserviceAuth.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MicroserviceAuth.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJWTokenGenerator _jwTokenGenerator;
        public AuthService(AppDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IJWTokenGenerator jwTokenGenerator)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwTokenGenerator = jwTokenGenerator;
        }

        public async Task<LoginResponsive> Login(LoginRequest model)
        {
            LoginResponsive responsive = new LoginResponsive();
            var getUser = _context.Users.SingleOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());

            if(getUser != null)
            {
                bool isValid = await _userManager.CheckPasswordAsync(getUser, model.Password);

                if (isValid)
                {
                    var token = _jwTokenGenerator.GenerateToken(getUser);
                    responsive.Token = token;

                    UsuarioDTO usuario = new UsuarioDTO();
                    usuario.IdUsuario = getUser.Id;
                    usuario.UserName = getUser.UserName;
                    usuario.Numero = getUser.PhoneNumber;
                    usuario.Email = getUser.Email;

                    responsive.Usuario = usuario;
                }
                else
                {
                    responsive.Token = "";
                    responsive.Usuario = null;
                }
            }
            else
            {
                responsive.Token = "";
                responsive.Usuario = null;
            }
            return responsive;
        }

        public async Task<string> Register(RegisterRequest usuario)
        {
            IdentityUser identityUser = new IdentityUser
            {
                UserName = usuario.UserName,
                Email = usuario.Email,
                NormalizedEmail= usuario.Email.ToUpper(),
                PhoneNumber = usuario.Numero
            };

            try
            {
                var result = await _userManager.CreateAsync(identityUser, usuario.Password);

                if(result.Succeeded)
                {
                    var userResult = _context.Users.SingleOrDefault(u => u.UserName == usuario.UserName);
                    UsuarioDTO us = new UsuarioDTO
                    {
                        UserName = userResult.UserName,
                        Email = userResult.Email,
                        IdUsuario = userResult.Id,
                        Numero = userResult.PhoneNumber
                    };

                    return "";
                }
                else
                {
                    return result.Errors.First().Description;
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
    }
}
