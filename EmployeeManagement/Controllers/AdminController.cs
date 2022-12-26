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
        [Authorize]
        public IActionResult Post([FromBody] SignUpRequest signUpRequest)
        {
           var admin = adminManager.AddAdmin(signUpRequest);
            if(admin != null) return Ok(admin);
            return BadRequest();
        }

        [HttpGet("Admin")]
        [Authorize]
        public IActionResult Get()
        {
            var users = adminManager.GetAdmins();
            if (users != null) return Ok(users);
            return BadRequest();
        }


        [HttpGet("AdminById/{id:guid}")]
        [Authorize]
        public IActionResult GetAdminById([FromRoute] Guid id)
        {
            var user = adminManager.GetAdminById(id);
            if (user != null) return Ok(user);
            return BadRequest();
        }

        [HttpPut("Admin")]
        [Authorize]
        public IActionResult UpdateAdmin([FromBody] UpdateUserRequest updateUserRequest)
        {
            var data = adminManager.UpdateAdmin(updateUserRequest);
            if (data != null) return Ok(data);
            return BadRequest();
        }

        [HttpDelete("Deleteadmin/{id:guid}")]
        [Authorize]
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
        [Authorize]
        public IActionResult ForgetPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest, [FromServices] IEmailSender email)
        {
            //string link = Request.GetEncodedUrl().Replace(Request.Path.ToUriComponent(), "/api/Admin/ResetPassword/ResetCode/");
            string link = "http://localhost:4200/auth/reset-password/";

            string message = adminManager.ForgetPassword(forgotPasswordRequest, link , email);
            if (message != null) return Ok(message);
            return BadRequest();
        }

        [HttpPost("ResetPassword/ResetCode/{resetcode:guid}")]
        [Authorize]
        public IActionResult ResetPassword([FromRoute] Guid resetcode,[FromBody] ResetPasswordRequest resetPasswordRequest)
        {
            var user = adminManager.ResetPassword(resetcode, resetPasswordRequest);
            if (user != null) return Ok(user);
            return BadRequest();
        }


        [HttpPost("EmployeeSignup")]
        [Authorize]
        public IActionResult Post([FromBody] EmployeeSignUpRequest employeeRequest)
        {
            var employee = adminManager.AddEmployee(employeeRequest);
            if (employee != null) return Ok(employee);
            return BadRequest();
        }


        [HttpGet("Employee")]
        [Authorize]
        public IActionResult GetEmployees()
        {
            var employee = adminManager.GetEmployees();
            if(employee != null) return Ok(employee);
            return BadRequest();
        }


        [HttpGet("EmployeeById/{id:guid}")]
        [Authorize]
        public IActionResult GetEmployeeById([FromRoute] Guid id)
        {
            var user = adminManager.GetEmployeeById(id);
            if (user != null) return Ok(user);
            return BadRequest();
        }


        [HttpPut("Employee")]
        [Authorize]
        public IActionResult UpdateEmployee([FromBody] UpdateEmployeeRequest updateEmployee)
        {
            var employee = adminManager.UpdateEmployee(updateEmployee);
            if(employee != null) return Ok(employee);
            return BadRequest();
        }


        [HttpDelete("deleteemployee/{id:guid}")]
        [Authorize]
        public IActionResult DeleteEmployee([FromRoute] Guid id)
        {
            var user = adminManager.DeleteEmployee(id);
            if (user != null) return Ok(user);
            return BadRequest();
        }


        [HttpPost("AddSalary/{empId:guid}")]
        [Authorize]
        public IActionResult AddSalary([FromRoute] Guid empId,[FromBody] SalaryRequest salaryRequest)
        {
            var salary = adminManager.AddSalary(empId,salaryRequest);   
            if(salary != null) return Ok(salary);
            return BadRequest();
        }


        [HttpGet("SalaryByEmpId{empId:guid}")]
        [Authorize]
        public IActionResult GetSalaryByEmpId([FromRoute] Guid empId)
        {
            var salary = adminManager.GetSalaryByEmpId(empId);
            if (salary != null) return Ok(salary);
            return BadRequest();
        }

        [HttpPut("Salary")]
        [Authorize]
        public IActionResult UpdateSalary([FromBody] UpdateSalaryRequest updateSalary)
        {
            var salary = adminManager.UpdateSalary(updateSalary);
            if (salary != 0) return Ok(salary);
            return BadRequest();
        }

        [HttpGet("getleavesbyid/{id:guid}")]
        public IActionResult GetLeavesById([FromRoute] Guid id)
        {
           var response = adminManager.GetLeaveById(id);
            if (response != null) return Ok(response);
            return BadRequest();
        }

        [HttpPost("AddLeave")]
        [Authorize]
        public IActionResult EntryLeave([FromBody] LeaveDetailRequest leaveDetailRequest)
        {
            var leave = adminManager.EntryLeave(leaveDetailRequest);    
            if(leave != null) return Ok(leave);
            return BadRequest();
        }

        [HttpPost("TotalLeaveOfEmployee")]
        [Authorize]
        public IActionResult LeaveEmp([FromBody] LeaveRequest leaveRequest)
        {
            var leave = adminManager.LeaveCount(leaveRequest);
            if(leave != null) return Ok(leave);    
            return BadRequest();    
        }

        [HttpPut("Updateleave")]
        [Authorize]
        public IActionResult UpdateLeave(UpdateLeaveRequest updateLeaveRequest)
        {
            var leave = adminManager.UpdateLeave(updateLeaveRequest);
            if (leave != null) return Ok(leave);
            return BadRequest();
        }

        [HttpGet("salarydeduction/{id:guid}")]
        public IActionResult GetSalaryDeduction([FromRoute] Guid id)
        {
            SalaryDeductionRequest salaryDeductionRequest = new SalaryDeductionRequest()
            {
                empId = id,
            };
           var response = adminManager.GetSalaryDeduction(salaryDeductionRequest);
            if (response != null) return Ok(response);
            return BadRequest();
        }

        [HttpPost("SalaryDeduction")]
        [Authorize]
        public IActionResult DeductedSalary([FromBody] SalaryDeductionRequest salaryDeductionRequest)
        {
            var salaryDeducted = adminManager.SalaryDeduction(salaryDeductionRequest);  
            if( salaryDeducted != null) return Ok(salaryDeducted);
            return BadRequest();
        }

        [HttpPost("AddExperinece")]
        [Authorize]
        public IActionResult AddExperience([FromBody] ExperienceRequest experienceRequest)
        {
            var experienceDetails = adminManager.AddExperience(experienceRequest);
            if(experienceDetails != null) return Ok(experienceDetails);
            return BadRequest();
        }

        
        [HttpGet("getexperiencebyid")]
        [Authorize]
        public IActionResult GetExperienceById([FromBody] Guid id)
        {
            var response = adminManager.GetExperienceById(id);
            if (response != null) return Ok(response);
            return BadRequest();
        }

        [HttpPut("UpdateExperience")]
        [Authorize]  
        public IActionResult UpdateExperience([FromBody] ExperienceUpdateRequest experienceUpdateRequest)
        {
            var updateExperience = adminManager.UpdateExperience(experienceUpdateRequest);  
            if(updateExperience != null) return Ok(updateExperience);   
            return BadRequest();
        }

        [HttpDelete("Deleteexperience/{id:guid}")]
        [Authorize]
        public IActionResult DeleteExperience([FromRoute] Guid id)
        {
            var experience = adminManager.DeleteExperience(id);
            if (experience != 0) return Ok(experience);
            return BadRequest();             
        }
    }

}
