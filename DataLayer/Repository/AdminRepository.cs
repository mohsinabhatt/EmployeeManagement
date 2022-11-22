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

        public IEnumerable<UserResponse> GetAllUsers()
        {
            var query = @$"Select * From Users Where IsDeleted = 0 ";
            var result = FromQuery<UserResponse>(query);
            return result;
        }

        //public int DeleteUser(Guid id)
        //{
        //    var query = $@"Update Users set IsDeleted = 1 where id ='{id}'"; 
        //    return ExecuteQuery(query);
        //}
    }
}
