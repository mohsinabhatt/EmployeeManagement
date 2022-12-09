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

        public LegalLeave LegalLeaves { get; set; }

        public int NoOfLeaves { get; set; }

        public int TotalLeaves { get; set; }

        public Guid? SalaryDeductionId { get; set; }


        [ForeignKey(nameof(SalaryDeductionId))] 
        public SalaryDeduction SalaryDeduction { get; set; }

    }
}
