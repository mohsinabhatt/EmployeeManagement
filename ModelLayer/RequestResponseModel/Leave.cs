using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class LeaveRequest
    {

        //public LegalLeave LegalLeaves { get; set; }

        //public int NoOfLeaves { get; set; }

        //public int TotalLeaves { get; set; }

        public Guid EmpId { get; set; }

        public DateTime Date { get; set; }
    }

    public class LeaveResponse : LeaveRequest
    {
        public Guid Id { get; set; }

    }

   public class  UpdateLeaveRequest
    {

        public Guid Id { get; set; }

        public LegalLeave LegalLeaves { get; set; }

        public int NoOfLeaves { get; set; }

        public int TotalLeaves { get; set; }

        public Guid EmpId { get; set; }

        public DateTime Date { get; set; }
    }
}
