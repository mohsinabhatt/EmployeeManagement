using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class LeaveDetails
    {

        public Guid Id { get; set; }


        public DateTime Date { get; set; }  


        public Guid EmpId { get; set; }


        [ForeignKey(nameof(EmpId))]
        public ICollection<Employee> Employees { get; set; }



    }
}
