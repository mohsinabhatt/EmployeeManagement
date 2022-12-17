using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage ="Required"),DataType(DataType.Password)]
        public string NewPassword { get; set; }


        [Required(ErrorMessage ="Requirwed"),DataType(DataType.Password),Compare(nameof(NewPassword),ErrorMessage ="NewPassword doent match")]  
        public string ConfirmPassword { get; set; }

    }

}
