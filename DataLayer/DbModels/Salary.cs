using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Salary
    {
        public Guid Id { get; set; }

        public int BasicSalary { get; set; }

        public int TA { get; set; }

        public int DA { get; set; }

        public int HRA { get; set; }

        public ICollection<Employee> Employees { get; set; }

        public SalaryDeduction SalaryDeduction { get; set; }
    }
}
