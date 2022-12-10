using ModelLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Leave
    {
        public Guid Id { get; set; }

        public int LegalLeaves { get; set; }   

        public int NoOfLeaves { get; set; }

        public int TotalLeaves { get; set; }

        public DateTime Date { get; set; }


        public Guid EmpId { get; set; }


        [ForeignKey(nameof(EmpId))]
        public Employee Employee { get; set; }

        public SalaryDeduction SalaryDeduction { get; set; }

    }
}
