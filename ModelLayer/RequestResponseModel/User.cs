using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class UserRequest
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public Gender Gender { get; set; }

        public string ContactNo { get; set; }

        public UserRole UserRole { get; set; }

        public string Salt { get; set; }


        public string ResetCode { get; set; }


        public bool IsActive { get; set; }


        public bool IsDeleted { get; set; }

    }

    public class UserResponse 
    {

        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public Gender Gender { get; set; }

        public string ContactNo { get; set; }

        public UserRole UserRole { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

    }

    public class UpdateUserRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Gender Gender { get; set; }

        public string ContactNo { get; set; }

        public bool IsActive { get; set; }

    }
}
