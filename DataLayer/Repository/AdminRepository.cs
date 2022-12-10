using ModelLayer;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    [ScopedService]
    public class AdminRepository : Repository
    {
        private readonly AppDbContext dbContext;

        public AdminRepository(AppDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<UserResponse> GetAllAdmins()
        {
            var query = @$"Select * From Users Where IsDeleted = 0 ";
            return FromQuery<UserResponse>(query);
        }


        public IEnumerable<EmployeeResponse> GetAllEmployees()
        {
            var query = $@"select * from Employees Where IsDeleted = 0";
            return FromQuery<EmployeeResponse>(query);
        }

       public SalaryResponse GetSalaryByEmpId(Guid empId)
        {
            var query = $@" select * from Salaries where EmpId= '{empId}' ";
            return GetObject<SalaryResponse>(query);
        }

        public int UpdateSalary(UpdateSalaryRequest updateSalary)
        {
            string query = $@"Update Salaries Set BasicSalary='{updateSalary.BasicSalary}',
                           TA ='{updateSalary.TA}',
                           HRA='{updateSalary.HRA}' 
                           where id ='{updateSalary.Id}'";
            return ExecuteQuery(query);
        }

        public IEnumerable<LeaveDetailResponse> GetDetailLeaveByEmpId(Guid empId)
        {
            var query = $@" Select * from LeaveDetails where EmpId='{empId}'";
            return FromQuery<LeaveDetailResponse>(query);
        }
    }
}
