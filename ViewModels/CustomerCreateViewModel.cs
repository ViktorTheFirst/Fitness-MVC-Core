using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessManagment.ViewModels
{
    public class CustomerCreateViewModel
    {
        [Required]
        [MaxLength(20, ErrorMessage = "Name too long")]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Dept")]
        public string Department { get; set; }

        public IFormFile Photo { get; set; }
        //IFormFile is an interface that can access (with model binding) the file uploaded to the server 
    }
}
