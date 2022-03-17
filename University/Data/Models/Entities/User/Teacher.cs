﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace University.Data.Models.Entities.User
{
    public class Teacher
    {
        //Cascade On Delete...
        public Teacher()
        {
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Enter Teacher Id.")]
        public string TeacherId { get; set; }

        [Required(ErrorMessage = "Enter Your Name.")]
        [Column(TypeName = "nvarchar(20)")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter Your Phone.")]
        [Column(TypeName = "nvarchar(13)")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Enter Your Address.")]
        [Column(TypeName = "nvarchar(200)")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Enter Your Photo.")]
        [NotMapped]
        public IFormFile Images { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Enter Your Role.")]
        [Display(Name = "Role")]
        public string Role { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter Your Password.")]
        public string Password { get; set; }

        [Display(Name = "ConfirmPassword")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter Your Password Confirm.")]
        [Compare("Password", ErrorMessage = "Passwords does not match.")]
        public string ConfirmPassword { get; set; }
    }
}
