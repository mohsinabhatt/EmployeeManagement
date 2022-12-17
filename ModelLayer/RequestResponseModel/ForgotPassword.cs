using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class ForgotPasswordRequest
    {
        [Required(ErrorMessage ="Please Enter Your Email To Reset Your Password")]
        public string Email { get; set; }
    }

    public class ForgotPasswordResponse : ForgotPasswordRequest
    {

    }
}
