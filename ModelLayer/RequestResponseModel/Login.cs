using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class LoginRequest
    {
        public string Email { get; set; }


        public string password { get; set; }
    }

    public class LoginResponse 
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public Gender Gender { get; set; }

        public string ContactNo { get; set; }

        public UserRole UserRole { get; set; }

        public string? Token { get; set; }
    }
}
