﻿using ModelLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Employee
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
          
        public Gender Gender { get; set; }

        public UserRole UserRole { get; set; }

        public bool? IsActive { get; set; } = true;

        public bool? IsDeleted { get; set; } = false;

        public string Address { get; set; }

        public string ContactNo { get; set; }

        public string? EmpCode { get; set; }


        public Guid? DeptId { get; set; }


        [ForeignKey(nameof(DeptId))]
        public Department Department { get; set; }


        public Salary Salary { get; set; }


    }
}
