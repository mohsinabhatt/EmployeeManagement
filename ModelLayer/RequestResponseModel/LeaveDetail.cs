using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class LeaveDetailRequest
    {
        public DateTime Date { get; set; }

        public Guid EmpId { get; set; }


    }
    public class LeaveDetailResponse :LeaveDetailRequest
    {
        public Guid Id { get; set; }
    }

}

