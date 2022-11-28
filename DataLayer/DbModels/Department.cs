using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Department
    {
        public Guid Id { get; set; }

        public string DeptName { get; set; }

        public ICollection<Employee> Employees { get; set; }

    }
}
