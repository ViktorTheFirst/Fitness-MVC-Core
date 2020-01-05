using FitnessManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessManagment.ViewModels
{
    //this class include info from the Employee model and 
    //some other info like PageTitle
    //then teh view only have to include this class to use 
    //both the model and the other info
    public class HomeDetailsViewModel
    {
        public Customer Customer { get; set; }
        public string PageTitle { get; set; }

    }
}
