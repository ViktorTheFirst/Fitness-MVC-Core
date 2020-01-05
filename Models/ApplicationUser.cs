using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//extantion class to the IdentityUser class
namespace FitnessManagment.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string City { get; set; }
    }
}
