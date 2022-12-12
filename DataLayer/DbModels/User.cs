using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }


        public string? Email { get; set; }


        public Gender Gender { get; set; }


        public string ContactNo { get; set; }

        public UserRole UserRole { get; set; }


        public string? Password { get; set; }


        public string? Salt { get; set; }


        public string? ResetCode { get; set; }


        public bool? IsActive { get; set; } = true;


        public bool? IsDeleted { get; set; } = false;


    }

}
