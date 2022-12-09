using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class EmployeeSignUpRequest
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public Gender Gender { get; set; }

        public string Address { get; set; }

        public string ContactNo { get; set; }

        //public Guid? SalaryId { get; set; }

        public string EmpCode { get; set; }


    }

    public class EmployeeSingUpResponse :EmployeeSignUpRequest
    {
        public Guid Id { get; set; }
    }

    public class UpdateEmployeeRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public Gender Gender { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public string Address { get; set; }

        public string ContactNo { get; set; }

        public string? EmpCode { get; set; }

        public Guid? DeptId { get; set; }

    }
}
