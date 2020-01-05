using FitnessManagment.Models;
using FitnessManagment.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessManagment.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Remote(action:"IsEmailInUse",controller:"Account")] //remotly activates the IsEmailInUse method 
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",
            ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string City { get; set; }
    }
}
