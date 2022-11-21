using AutoMapper;
using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;

namespace WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly AccountManager accountManager;

        public AccountController(IRepository repository,IMapper mapper)
        {
            this.accountManager = new AccountManager(repository, mapper);
        }

        [HttpPost("login")]
        public IActionResult Post(LoginRequest loginRequest)
        {
           var user = accountManager.LogIn(loginRequest);
            if(user != null) return Ok(user);
            //user.Token = 
            return BadRequest();
        }
    }
}
