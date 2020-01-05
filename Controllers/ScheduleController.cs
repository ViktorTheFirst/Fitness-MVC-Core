using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FitnessManagment.Controllers
{
    public class ScheduleController : Controller
    {
        [HttpGet]
        public IActionResult Trainings()
        {
            return View();
        }
    }
}