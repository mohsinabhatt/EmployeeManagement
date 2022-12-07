using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class SalaryDeduction
    {
        [ForeignKey(nameof(Salary))]
        public Guid Id { get; set; }

        public int LeaveDeductedSal { get; set; }

        public int PF { get; set; }

        public int TotalSalary { get; set; }

        public Salary Salary { get; set; }

        public ICollection<Leave> Leaves { get; set; }
    }
}
