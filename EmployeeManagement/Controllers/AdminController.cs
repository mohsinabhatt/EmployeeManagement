using AutoMapper;
using BusinessLayer;
using Castle.Core.Smtp;
using DataLayer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Identity.Client;
using ModelLayer;
using SharedLibrary;
using System.Diagnostics.Contracts;
using System.Security.Claims;

namespace WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AdminController : ControllerBase
    {
        private readonly AdminManager adminManager;
        private readonly AdminRepository adminRepository;
        private readonly IEmailSender emailSender;

        public AdminController(IMapper mapper,AdminRepository adminRepository,IEmailSender emailSender)
        {
            adminManager = new AdminManager(mapper, adminRepository, emailSender);
            this.adminRepository = adminRepository;
        }


        [HttpPost("AdminSignup")]
        public IActionResult Post([FromBody] SignUpRequest signUpRequest)
        {
           var admin = adminManager.AddAdmin(signUpRequest);
            if(admin != null) return Ok(admin);
            return BadRequest();
        }

        [HttpGet("Admin")]
        public IActionResult Get()
        {
            var users = adminManager.GetAdmins();
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

        [HttpPut("ChangePassword")]
        public IActionResult ChangeAdminPassword([FromBody] ChangePasswordRequest changePasswordRequest)
        {
            var admin = adminManager.ChangePassword(changePasswordRequest); 
            if(admin != 0) return Ok(admin);    
            return BadRequest();
        }

        [HttpPut("ForgotPassword")]
        public IActionResult ForgetPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest, [FromServices] IEmailSender email)
        {
            string link = Request.GetEncodedUrl().Replace(Request.Path.ToUriComponent(), "/api/Admin/ResetPassword/ResetCode/");
            string message = adminManager.ForgetPassword(forgotPasswordRequest, link, email);
            if(message != null) return Ok(message);
            return BadRequest();
        }

        [HttpPost("ResetPassword/ResetCode/{resetcode:guid}")]
        public IActionResult ResetPassword([FromRoute] Guid resetcode,[FromBody] ResetPasswordRequest resetPasswordRequest)
        {
            var user = adminManager.ResetPassword(resetcode, resetPasswordRequest);
            if (user != null) return Ok(user);
            return BadRequest();
        }


        [HttpPost("EmployeeSignup")]
        public IActionResult Post([FromBody] EmployeeSignUpRequest employeeRequest)
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
        public IActionResult UpdateEmployee([FromBody] UpdateEmployeeRequest updateEmployee)
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


        [HttpPost("AddSalary/{empId:guid}")]
        public IActionResult AddSalary([FromRoute] Guid empId,[FromBody] SalaryRequest salaryRequest)
        {
            var salary = adminManager.AddSalary(empId,salaryRequest);   
            if(salary != null) return Ok(salary);
            return BadRequest();
        }


        [HttpGet("SalaryByEmpId{empId:guid}")]
        public IActionResult GetSalaryByEmpId([FromRoute] Guid empId)
        {
            var salary = adminManager.GetSalaryByEmpId(empId);
            if (salary != null) return Ok(salary);
            return BadRequest();
        }

        [HttpPut("Salary")]
        public IActionResult UpdateSalary([FromBody] UpdateSalaryRequest updateSalary)
        {
            var salary = adminManager.UpdateSalary(updateSalary);
            if (salary != 0) return Ok(salary);
            return BadRequest();
        }

        [HttpPost("AddLeave")]
        public IActionResult EntryLeave([FromBody] LeaveDetailRequest leaveDetailRequest)
        {
            var leave = adminManager.EntryLeave(leaveDetailRequest);    
            if(leave != null) return Ok(leave);
            return BadRequest();
        }

        [HttpPost("TotalLeaveOfEmployee")]
        public IActionResult LeaveEmp([FromBody] LeaveRequest leaveRequest)
        {
            var leave = adminManager.LeaveCount(leaveRequest);
            if(leave != null) return Ok(leave);    
            return BadRequest();    
        }

        [HttpPut("Updateleave")]
        public IActionResult UpdateLeave(UpdateLeaveRequest updateLeaveRequest)
        {
            var leave = adminManager.UpdateLeave(updateLeaveRequest);
            if (leave != null) return Ok(leave);
            return BadRequest();
        }

        [HttpPost("SalaryDeduction")]
        public IActionResult DeductedSalary([FromBody] SalaryDeductionRequest salaryDeductionRequest)
        {
            var salaryDeducted = adminManager.SalaryDeduction(salaryDeductionRequest);  
            if( salaryDeducted != null) return Ok(salaryDeducted);
            return BadRequest();
        }

        [HttpPost("AddExperinece")]
        public IActionResult AddExperience([FromBody] ExperienceRequest experienceRequest)
        {
            var experienceDetails = adminManager.AddExperience(experienceRequest);
            if(experienceDetails != null) return Ok(experienceDetails);
            return BadRequest();
        }

        [HttpPut("UpdateExperience")]   
        public IActionResult UpdateExperience([FromBody] ExperienceUpdateRequest experienceUpdateRequest)
        {
            var updateExperience = adminManager.UpdateExperience(experienceUpdateRequest);  
            if(updateExperience != null) return Ok(updateExperience);   
            return BadRequest();
        }

        [HttpDelete("Deleteexperience/{id:guid}")]
        public IActionResult DeleteExperience([FromRoute] Guid id)
        {
            var experience = adminManager.DeleteExperience(id);
            if (experience != 0) return Ok(experience);
            return BadRequest();             
        }
    }

}
