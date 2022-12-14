using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Experience
    {
        public Guid Id { get; set; }

        public string CompanyName { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string TotalExperience { get; set; }

        public int Salary { get; set; }

        public bool IsDeleted { get; set; }

        public Guid EmpId { get; set; }


        [ForeignKey(nameof(EmpId))]
        public ICollection<Employee> Employees { get; set; }

    }
}
