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
    public class AdminController : ControllerBase
    {
        private readonly AdminManager adminManager;
        private readonly AdminRepository adminRepository;

        public AdminController(IMapper mapper,AdminRepository adminRepository)
        {
            adminManager = new AdminManager(mapper, adminRepository);
            this.adminRepository = adminRepository;
        }


        [HttpPost("signup")]
        public IActionResult Post(SignUpRequest signUpRequest)
        {
           var user = adminManager.AddUser(signUpRequest);
            if(user != null) return Ok(user);
            return BadRequest();
        }

        [HttpGet]
        public IActionResult Get()
        {
           var users = adminManager.GetAll();
            if (users != null) return Ok(users);
            return BadRequest();
        }

        [HttpGet("getbyid/{id:guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
           var user = adminManager.GetById(id);
            if (user != null) return Ok(user);
            return BadRequest();
        }

        [HttpPut]
        public IActionResult Update([FromBody]UpdateUserRequest updateUserRequest)
        {
            var data = adminManager.Update(updateUserRequest);
            if(data != null) return Ok(data);
            return BadRequest();
        }

        [HttpDelete("delete/{id:guid}")]
        public IActionResult Delete([FromRoute]Guid id)
        {
          var user = adminManager.Delete(id);
            if(user != null) return Ok(user);
            return BadRequest();
        }
    }
}
