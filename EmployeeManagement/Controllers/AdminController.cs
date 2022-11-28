using AutoMapper;
using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
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


        [HttpPost("AdminSignup")]
        public IActionResult Post(SignUpRequest signUpRequest)
        {
           var admin = adminManager.AddAdmin(signUpRequest);
            if(admin != null) return Ok(admin);
            return BadRequest();
        }

        [HttpGet("Admin")]
        public IActionResult Get()
        {
            var users = adminManager.GetAddmins();
            if (users != null) return Ok(users);
            return BadRequest();
        }


        [HttpGet("AdminById/{id:guid}")]
        public IActionResult GetAdminById([FromRoute] Guid id)
        {
            var user = adminManager.GetAdminById(id);
            if (user != null) return Ok(user);
            return BadRequest();
        }

        [HttpPut("Admin")]
        public IActionResult UpdateAdmin([FromBody] UpdateUserRequest updateUserRequest)
        {
            var data = adminManager.UpdateAdmin(updateUserRequest);
            if (data != null) return Ok(data);
            return BadRequest();
        }

        [HttpDelete("Deleteadmin/{id:guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var user = adminManager.Delete(id);
            if (user != null) return Ok(user);
            return BadRequest();
        }



        [HttpPost("EmployeeSignup")]
        public IActionResult Post(EmployeeSignUpRequest employeeRequest)
        {
            var employee = adminManager.AddEmployee(employeeRequest);
            if (employee != null) return Ok(employee);
            return BadRequest();
        }


        [HttpGet("Employee")]
        public IActionResult GetEmployees()
        {
            var employee = adminManager.GetEmployees();
            if(employee != null) return Ok(employee);
            return BadRequest();
        }


        [HttpGet("EmployeeById/{id:guid}")]
        public IActionResult GetEmployeeById([FromRoute] Guid id)
        {
            var user = adminManager.GetEmployeeById(id);
            if (user != null) return Ok(user);
            return BadRequest();
        }


        [HttpPut("Employee")]
        public IActionResult UpdateEmployee(UpdateEmployeeRequest updateEmployee)
        {
            var employee = adminManager.UpdateEmployee(updateEmployee);
            if(employee != null) return Ok(employee);
            return BadRequest();
        }


        [HttpDelete("deleteemployee/{id:guid}")]
        public IActionResult DeleteEmployee([FromRoute] Guid id)
        {
            var user = adminManager.DeleteEmployee(id);
            if (user != null) return Ok(user);
            return BadRequest();
        }
    }
}
