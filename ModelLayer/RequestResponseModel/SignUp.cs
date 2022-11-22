using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class SignUpRequest
    {

        public string Name { get; set; }

        public string Email { get; set; }

        public Gender Gender { get; set; }

        public string ContactNo { get; set; }

        public UserRole UserRole { get; set; }


        public string Password { get; set; }


        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }

    public class SignUpresponse
    {

        public Guid Id { get; set; }

        public string Email { get; set; }

        public Gender Gender { get; set; }

        public UserRole UserRole { get; set; }

        public string ContactNo { get; set; }
    }
}
