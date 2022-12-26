using AutoMapper;
using AutoMapper.QueryableExtensions;
using Castle.Core.Smtp;
using DataLayer;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.VisualBasic;
using ModelLayer;
using Org.BouncyCastle.Math.EC.Rfc7748;
using SendGrid.Helpers.Mail;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class AdminManager
    {
        private readonly IMapper mapper;
        private readonly AdminRepository adminRepository;
        private readonly IEmailSender emailSender;

        public AdminManager(IMapper mapper, AdminRepository adminRepository, IEmailSender emailSender)
        {
            this.mapper = mapper;
            this.adminRepository = adminRepository;
            this.emailSender = emailSender;
        }

        public SignUpResponse AddAdmin(SignUpRequest signUpRequest)
        {
            var user = mapper.Map<User>(signUpRequest);
            user.Salt = AppEncryption.CreateSalt();
            user.Id = Guid.NewGuid();
            user.UserRole = UserRole.Admin;
            user.Password = AppEncryption.CreatePasswordHash(signUpRequest.Password, user.Salt);
            if (adminRepository.AddAndSave(user) != 0)
                return mapper.Map<SignUpResponse>(user);
            return null;
        }
        public EmployeeResponse AddEmployee(EmployeeSignUpRequest employeeSignUpRequest)
        {
            if (adminRepository.FindBy<Employee>(x => x.EmpCode == employeeSignUpRequest.EmpCode).FirstOrDefault() != null) return null;
            if (adminRepository.FindBy<Employee>(x => x.Email == employeeSignUpRequest.Email).FirstOrDefault() != null) return null;
            var employee = mapper.Map<Employee>(employeeSignUpRequest);
            employee.Id = Guid.NewGuid();
            employee.UserRole = UserRole.Employee;
            if (adminRepository.AddAndSave(employee) != 0)
                return mapper.Map<EmployeeResponse>(employee);
            return null;
        }


        public int ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            var user = adminRepository.GetById<User>(changePasswordRequest.Id);
            if (user != null)
            {
                string salt = user.Salt;
                string password = user.Password;
                string oldPassword = AppEncryption.CreatePasswordHash(changePasswordRequest.OldPassword, salt);

                if (oldPassword.Equals(password))
                {
                    user.Salt = AppEncryption.CreateSalt();
                    user.Password = AppEncryption.CreatePasswordHash(changePasswordRequest.NewPassword, user.Salt);
                    if (adminRepository.UpdateAndSave(user) != 0) return 1;
                }
            }
            return 0;
        }

        public string ForgetPassword(ForgotPasswordRequest forgotPasswordRequest, string link, IEmailSender emailSender)
        {
            User user = adminRepository.FindBy<User>(x => x.Email == forgotPasswordRequest.Email).FirstOrDefault();
            if (user != null)
            {
                Guid guid = Guid.NewGuid();
                user.ResetCode = guid.ToString();
                link += guid;
                var value = adminRepository.UpdateAndSave(user);
                if (value != 0)
                {
                    var x = SendResetEmail(user, link);
                    return "success";
                }
            }
            return "NotExist";
        }


        public async Task SendResetEmail(User request, string link)
        {

            var email = new List<string>();
            email.Add(request.Email);
            string subject = "Reset Password";
            StringBuilder stringBuilder = new();
            stringBuilder.AppendFormat("Please Click on the link to reset Password\t");
            stringBuilder.AppendFormat(link);
            string body = stringBuilder.ToString();
            emailSender?.Send("Employee Management", email[0], subject, body);
        }

        public string ResetPassword(Guid resetCode, ResetPasswordRequest resetPasswordRequest)
        {
            User user = adminRepository.FindBy<User>(x => x.ResetCode == resetCode.ToString()).FirstOrDefault();
            if (user != null)
            {
                user.Salt = AppEncryption.CreateSalt();
                user.Password = AppEncryption.CreatePasswordHash(resetPasswordRequest.NewPassword, user.Salt);
                user.ResetCode = null;
                adminRepository.UpdateAndSave(user);
                return "Password Reset Successfully";
            }
            return null;
        }

        public IEnumerable<UserResponse> GetAdmins()
        {
            return adminRepository.GetAllAdmins();
        }

        public IEnumerable<EmployeeResponse> GetEmployees()
        {
            return adminRepository.GetAllEmployees();
        }

        public UserResponse GetAdminById(Guid id)
        {
            return mapper.Map<UserResponse>(adminRepository.GetById<User>(id));
        }

        public EmployeeResponse GetEmployeeById(Guid id)
        {
            return mapper.Map<EmployeeResponse>(adminRepository.GetById<Employee>(id));
        }

        public UpdateUserRequest UpdateAdmin(UpdateUserRequest updateUserRequest)
        {
            var admin = adminRepository.GetById<User>(updateUserRequest.Id);
            if (admin != null)
            {
                admin.Id = updateUserRequest.Id;
                admin.Name = updateUserRequest.Name;
                admin.ContactNo = updateUserRequest.ContactNo;
                admin.Email = updateUserRequest.Email;
                admin.UserRole = UserRole.Admin;
                admin.Gender = updateUserRequest.Gender;
                admin.IsActive = updateUserRequest.IsActive;

                if (adminRepository.UpdateAndSave(admin) != 0) return updateUserRequest;
            }
            return null;

            //var user = mapper.Map<User>(updateUserRequest);
            //if (adminRepository.UpdateAndSave(user) != 0)
            //    return updateUserRequest;
            //return null;
        }

        public UpdateEmployeeRequest UpdateEmployee(UpdateEmployeeRequest updateemployee)
        {
            var employee = adminRepository.GetById<Employee>(updateemployee.Id);
            if (employee != null)
            {
                employee.Id = updateemployee.Id;
                employee.Name = updateemployee.Name;
                employee.ContactNo = updateemployee.ContactNo;
                employee.Email = updateemployee.Email;
                employee.Gender = updateemployee.Gender;
                employee.Address = updateemployee.Address;
                employee.DeptId = updateemployee.DeptId;
                employee.IsActive = updateemployee.IsActive;
                if (adminRepository.UpdateAndSave(employee) != 0)
                    return updateemployee;
            }
            return null;
        }

        public User Delete(Guid id)
        {
            //return adminRepository.DeleteUser(id);
            var user = adminRepository.GetById<User>(id);
            user.IsDeleted = true;
            if (adminRepository.UpdateAndSave(user) != 0)
                return user;
            return null;
        }

        public Employee DeleteEmployee(Guid id)
        {
            var employee = adminRepository.GetById<Employee>(id);
            employee.IsDeleted = true;
            if (adminRepository.UpdateAndSave(employee) != 0)
                return employee;
            return null;
        }

        public SalaryRequest AddSalary(Guid empId, SalaryRequest salaryRequest)
        {
            var employee = adminRepository.GetById<Employee>(empId);
            var sal = adminRepository.FindBy<Salary>(x => x.EmpId == empId).FirstOrDefault();
            if (sal == null)
            {
                if (employee != null)
                {
                    var salary = mapper.Map<Salary>(salaryRequest);
                    salary.Id = Guid.NewGuid();
                    salary.EmpId = employee.Id;
                    if (adminRepository.AddAndSave(salary) != 0)
                        return salaryRequest;
                }
            }
            else
                return null;
            return null;
        }

        public SalaryResponse GetSalaryByEmpId(Guid empId)
        {
            return adminRepository.GetSalaryByEmpId(empId);
        }

        public int UpdateSalary(UpdateSalaryRequest updateSalary)
        {
            return adminRepository.UpdateSalary(updateSalary);
            //var salary = mapper.Map<Salary>(updateSalary);
            //  if (adminRepository.UpdateAndSave(salary) != 0)
            //      return updateSalary;
            //  return null;
        }

        public IEnumerable<LeaveDetailResponse> GetLeaveById(Guid id)
        {
            var response = adminRepository.FindBy<LeaveDetails>(x => x.EmpId == id).Select(z=>new LeaveDetailResponse
            {
                EmpId = id,
                Date= z.Date,
                Id = z.Id
            });
            return response;
        }

        public LeaveDetailRequest EntryLeave(LeaveDetailRequest leaveDetailRequest)
        {
            var employee = adminRepository.FindBy<Employee>(x => x.Id == leaveDetailRequest.EmpId).FirstOrDefault();
            if (leaveDetailRequest.Date <= DateTime.Now)
            {
                LeaveDetails leaveDetails = new LeaveDetails()
                {
                    Id = Guid.NewGuid(),
                    Date = leaveDetailRequest.Date.Date,
                    EmpId = employee.Id,
                };
                if (adminRepository.AddAndSave(leaveDetails) != 0)
                    return leaveDetailRequest;
            }
            return null;
        }

        public LeaveRequest LeaveCount(LeaveRequest leaveRequest)
        {
            var leaveDetail = adminRepository.GetDetailLeaveByEmpId(leaveRequest.EmpId).Count();
            var emp = adminRepository.FindBy<Leave>(x => x.EmpId == leaveRequest.EmpId && x.Date == leaveRequest.Date).FirstOrDefault();
            if (emp != null)
                if (emp.Date.Year == leaveRequest.Date.Year && emp.Date.Month == leaveRequest.Date.Month) return null;
            if (leaveDetail != 0)
            {
                Leave leave = new Leave()
                {
                    Id = Guid.NewGuid(),
                    LegalLeaves = (int)LegalLeave.five,
                    NoOfLeaves = leaveDetail,
                    EmpId = leaveRequest.EmpId,
                    Date = leaveRequest.Date,

                };
                if (leave.NoOfLeaves > leave.LegalLeaves)
                    leave.TotalLeaves = leave.NoOfLeaves - leave.LegalLeaves;

                if (adminRepository.AddAndSave(leave) != 0)
                    return leaveRequest;
            }
            return null;
        }

        public UpdateLeaveRequest UpdateLeave(UpdateLeaveRequest updateLeaveRequest)
        {
            var leave = mapper.Map<Leave>(updateLeaveRequest);
            if (adminRepository.UpdateAndSave(leave) != 0) return updateLeaveRequest;
            return null;
        }

        public SalaryDeductionResponse GetSalaryDeduction(SalaryDeductionRequest salaryDeductionRequest)
        {
            var x = SalaryDeduction(salaryDeductionRequest);
            var response = adminRepository.FindBy<SalaryDeduction>(x => x.leave.EmpId == salaryDeductionRequest.empId).FirstOrDefault();
            if(response!=null) return mapper.Map<SalaryDeductionResponse>(response);
            return null;

        }

        public SalaryDeductionResponse SalaryDeduction(SalaryDeductionRequest salaryDeductionRequest)
        {
            var employeeLeave = adminRepository.FindBy<Leave>(x => x.EmpId == salaryDeductionRequest.empId && x.Date.Date == DateTime.Now.Date).FirstOrDefault();
            if (employeeLeave != null)
            {
                var employeeSalary = adminRepository.FindBy<Salary>(x => x.EmpId == salaryDeductionRequest.empId).FirstOrDefault();
                if (employeeSalary != null)
                {
                    SalaryDeduction salaryDeduction = new()
                    {
                        Id = employeeSalary.Id,
                        LeaveId = employeeLeave.Id,
                        LeaveDeductedSal = employeeSalary.BasicSalary / 30 * employeeLeave.TotalLeaves,
                        PF = employeeSalary.BasicSalary * 12 / 100,
                    };
                    salaryDeduction.TotalSalary = employeeSalary.BasicSalary + employeeSalary.TA + employeeSalary.DA + employeeSalary.HRA - salaryDeduction.LeaveDeductedSal - salaryDeduction.PF;
                    if (adminRepository.AddAndSave(salaryDeduction) != 0)
                        return mapper.Map<SalaryDeductionResponse>(salaryDeduction);
                }
            }
            return null;
        }

        public ExperienceRequest AddExperience(ExperienceRequest experienceRequest)
        {
            if (adminRepository.FindBy<Employee>(x => x.Id == experienceRequest.EmpId).FirstOrDefault() == null) return null;
            if (adminRepository.FindBy<Experience>(x => x.EmpId == experienceRequest.EmpId && x.CompanyName == experienceRequest.CompanyName && x.From == experienceRequest.From && x.To == experienceRequest.To).FirstOrDefault() != null) return null;
            Experience experience = new Experience()
            {
                Id = Guid.NewGuid(),
                EmpId = experienceRequest.EmpId,
                CompanyName = experienceRequest.CompanyName,
                Salary = experienceRequest.Salary,
                From = experienceRequest.From.Date,
                To = experienceRequest.To.Date,
            };

            if (experienceRequest.To.Month < experienceRequest.From.Month)
            {
                int yearr = experience.To.Year - experience.From.Year;
                int monthh = experience.To.Month - experience.From.Month;
                experience.TotalExperience = $"{yearr} year {-monthh} months";
            }
            else if (experienceRequest.To.Month > experienceRequest.From.Month)
            {
                int year = experience.To.Year - experience.From.Year;
                int month = experience.To.Month - experience.From.Month;
                experience.TotalExperience = $"{year} year {month} months";
            }

            if (adminRepository.AddAndSave(experience) != 0)
                return experienceRequest;
            return null;
        }

        public ExperienceResponse GetExperienceById(Guid id)
        {
            var exp = adminRepository.GetById<Experience>(id);
            if (exp != null)
                return mapper.Map<ExperienceResponse>(exp);
            return null;

        }

        public ExperienceUpdateRequest UpdateExperience(ExperienceUpdateRequest experienveUpdateRequest)
        {
            var exp = adminRepository.FindBy<Experience>(x => x.Id == experienveUpdateRequest.Id && x.EmpId == experienveUpdateRequest.EmpId).FirstOrDefault();
            if (exp != null)
            {
                exp.Id = experienveUpdateRequest.Id;
                exp.EmpId = experienveUpdateRequest.EmpId;
                exp.CompanyName = experienveUpdateRequest.CompanyName;
                exp.Salary = experienveUpdateRequest.Salary;
                exp.From = experienveUpdateRequest.From.Date;
                exp.To = experienveUpdateRequest.To.Date;

                if (experienveUpdateRequest.To.Month < experienveUpdateRequest.From.Month)
                {
                    int yearr = exp.To.Year - exp.From.Year;
                    int monthh = exp.To.Month - exp.From.Month;
                    exp.TotalExperience = $"{yearr} year {-monthh} months";
                }
                else if (experienveUpdateRequest.To.Month > experienveUpdateRequest.From.Month)
                {
                    int year = exp.To.Year - exp.From.Year;
                    int month = exp.To.Month - exp.From.Month;
                    exp.TotalExperience = $"{year} year {month} months";
                }

                if (adminRepository.UpdateAndSave(exp) != 0)
                    return experienveUpdateRequest;
            }
            return null;
        }

        public int DeleteExperience(Guid id)
        {
            var exp = adminRepository.GetById<Experience>(id);
            exp.IsDeleted = true;
            if (adminRepository.UpdateAndSave(exp) != 0) return 1;
            return 0;
        }
    }
}
