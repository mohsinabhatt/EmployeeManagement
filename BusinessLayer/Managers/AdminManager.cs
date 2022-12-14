﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataLayer;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.VisualBasic;
using ModelLayer;
using Org.BouncyCastle.Math.EC.Rfc7748;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class AdminManager
    {
        private readonly IMapper mapper;
        private readonly AdminRepository adminRepository;

        public AdminManager(IMapper mapper, AdminRepository adminRepository)
        {
            this.mapper = mapper;
            this.adminRepository = adminRepository;
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
            var employee = mapper.Map<Employee>(employeeSignUpRequest);
            employee.Id = Guid.NewGuid();
            employee.UserRole = UserRole.Employee;
            if (adminRepository.AddAndSave(employee) != 0)
                return mapper.Map<EmployeeResponse>(employee);
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
            var user = mapper.Map<User>(updateUserRequest);
            if (adminRepository.UpdateAndSave(user) != 0)
                return updateUserRequest;
            return null;
        }

        public UpdateEmployeeRequest UpdateEmployee(UpdateEmployeeRequest updateemployee)
        {
            var employee = mapper.Map<Employee>(updateemployee);
            if (adminRepository.UpdateAndSave(employee) != 0)
                return updateemployee;
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
            if (adminRepository.FindBy<Experience>(x => x.EmpId == experienceRequest.EmpId && x.CompanyName == experienceRequest.CompanyName && x.From == experienceRequest.From && x.To == experienceRequest.To ).FirstOrDefault() != null) return null;
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
