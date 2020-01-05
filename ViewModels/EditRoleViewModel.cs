using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessManagment.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }


        public string Id { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        public string RoleName { get; set; } 

        //this is a collection so it must be created before we call methods on that collection
        public List<string> Users { get; set; } //all users under certain role
    }
}
