using AutoMapper;
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
            user.IsActive = true;
            user.IsDeleted = false;
            user.Id = Guid.NewGuid();
            user.UserRole = UserRole.Admin;
            user.Password = AppEncryption.CreatePasswordHash(signUpRequest.Password, user.Salt);
            if (adminRepository.AddAndSave(user) != 0)
                return mapper.Map<SignUpResponse>(user);
            return null;
        }
        public EmployeeResponse AddEmployee(EmployeeSignUpRequest employeeSignUpRequest)
        {
            var employee = mapper.Map<Employee>(employeeSignUpRequest);
            employee.IsActive = true;
            employee.Id = Guid.NewGuid();
            employee.UserRole = UserRole.Employee;
            employee.IsDeleted = false;
            if (adminRepository.AddAndSave(employee) != 0)
                return mapper.Map<EmployeeResponse>(employee);
            return null;
        }

        public IEnumerable<UserResponse> GetAddmins()
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
    }
}
