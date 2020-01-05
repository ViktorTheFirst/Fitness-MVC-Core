using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessManagment.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Name too long")]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Dept")]
        public string Department { get; set; }

        public string PhotoPath { get; set; }
    }
}
