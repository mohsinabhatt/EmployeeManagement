using AutoMapper;
using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ModelLayer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountManager accountManager;
        private readonly IConfiguration configuration;

        public AccountController(IRepository repository,IMapper mapper, IConfiguration configuration)
        {
            this.accountManager = new AccountManager(repository, mapper);
            this.configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Post([FromBody] LoginRequest loginRequest)
        {
           var loginResponse = accountManager.LogIn(loginRequest);
            if(loginResponse != null)
            {
                var tokenString = GenerateJSONWebToken(loginResponse);
                loginResponse.Token = tokenString;
                return Ok(loginResponse);

            }
            return BadRequest();
        }

        private string GenerateJSONWebToken(LoginResponse loginResponse)
        {
            var secret = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
            var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim("Id" ,loginResponse.Id.ToString()),
                new Claim("Email",loginResponse.Email)
            });

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claimsIdentity,
                Issuer = configuration["JWT:Issuer"],
                Audience = configuration["JWT:Audience"],
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddMinutes(2),
                TokenType = "JWT"
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(securityTokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
