using AutoMapper;
using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;

namespace WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly AdminManager adminManager;

        public AdminController(IRepository repository,IMapper mapper)
        {
            this.adminManager = new AdminManager(repository,mapper);
        }


        [HttpPost("signup")]
        public IActionResult Post(SignUpRequest signUpRequest)
        {
           var user = adminManager.AddUser(signUpRequest);
            if(user == null) return Ok(user);
            return BadRequest();
        }
    }
}
