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

        //public int DeleteUser(Guid id)
        //{
        //    var query = $@"Update Users set IsDeleted = 1 where id ='{id}'"; 
        //    return ExecuteQuery(query);
        //}
    }
}
