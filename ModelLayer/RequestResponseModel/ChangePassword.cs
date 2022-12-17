using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
        public class ChangePasswordRequest
        {
            public Guid Id { get; set; }

            public string Email { get; set; }


            [Required, DataType(DataType.Password)]
            public string OldPassword { get; set; }


            [Required, DataType(DataType.Password)]
            public string NewPassword { get; set; }


            [Required, DataType(DataType.Password), Compare("NewPassword")]
            public string ConfirmPassword { get; set; }
        }
}
