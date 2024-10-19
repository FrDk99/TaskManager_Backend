using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Interfaces;
using TaskManager.Entities.Users;
using TaskManager.UnitOfWork.Interfaces;

namespace TaskManager.Domain.Models
{
    public class LoginDomain : ILoginDomain
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public LoginDomain(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }


        public async Task<UserResponse?> Login(UserRequest request)
        {

            var context = _unitOfWork.Create();

            var usuario = await context.Repositories.LoginRepository.Login(request) ?? throw new Exception("Usuario o contraseña incorrectos.");
            
            usuario.Token = GenerateToken(usuario);


            return usuario;

        }

        public string GenerateToken(UserResponse usuario)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credencials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Crear los claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, usuario.FirstName),
                new Claim(ClaimTypes.Email, usuario.Email),
                //new Claim(ClaimTypes.Role, usuario.Rol),
            };

            //Crear el token
            var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.Now.AddMinutes(2),
                            signingCredentials: credencials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
