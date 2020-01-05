using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessManagment.ViewModels
{
    public class CustomerEditViewModel : CustomerCreateViewModel
    {
        public int Id { get; set; }
        public string existingPhotoPath { get; set; }
    }
}
