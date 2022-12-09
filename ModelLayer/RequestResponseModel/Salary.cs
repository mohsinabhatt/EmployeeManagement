﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class SalaryRequest
    {
        public int BasicSalary { get; set; }

        public int TA { get; set; }

        public int DA { get; set; }

        public int HRA { get; set; }

        //public Guid EmpId { get; set; }

    }

    public class SalaryResponse :SalaryRequest
    {
        public Guid Id { get; set; }
    }

    public class UpdateSalaryRequest
    {
        public Guid Id { get; set; }

        public int BasicSalary { get; set; }

        public int TA { get; set; }

        public int DA { get; set; }

        public int HRA { get; set; }
    }


    public class SalaryDeductionRequest
    {
        public Guid Id { get; set; }

        public int LeaveDeductedSal { get; set; }

        public int PF { get; set; }

        public int TotalSalary { get; set; }

    }
    public class SalaryDeductionResponse :SalaryDeductionRequest
    {

    }

    public class LeaveRequest
    {
        public Guid Id { get; set; }

        public LegalLeave LegalLeaves { get; set; }

        public int NoOfLeaves { get; set; }

        public int TotalLeaves { get; set; }

        public Guid? SalaryDeductionId { get; set; }
    }

    public class LeaveResponse :LeaveRequest
    {

    }
}