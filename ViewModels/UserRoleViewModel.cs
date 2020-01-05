using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessManagment.ViewModels
{
    //this view model is needed in order to bridge between the "AspNetRoles" & "AspNetUsers" tables 
    //
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsSelected { get; set; }
    }
}
