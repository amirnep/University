using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace University.Data.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Enter Your Role.")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Enter Your Student Id.")]
        public string Id { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter Your Password.")]
        public string Password { get; set; }
    }
}
