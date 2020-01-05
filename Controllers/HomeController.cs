using FitnessManagment.Models;
using FitnessManagment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessManagment.Controllers
{
    public class HomeController : Controller
    {
        //constructor injection:
        private readonly ICustomerRepository _customerRepository; //instance of interface 
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(ICustomerRepository customerRepository, IHostingEnvironment hostingEnvironment)
        {
            _customerRepository = customerRepository; //instantiate the interface instace
            _hostingEnvironment = hostingEnvironment;
        }


        [Route("")]
        [Route("Home/index")]
        [AllowAnonymous]
        public ViewResult index()
        {
            return View();
        }


        [Route("Home/custs")]
        [AllowAnonymous]
        public ViewResult custs()
        {
            var model = _customerRepository.GetAllCustomers();
            return View(model);
        }

        
        [Route("Home/Details/{id?}")]
        [AllowAnonymous]
        public ViewResult Details(int? id) //? after int means that int is nulleble
        {
            Customer customer = _customerRepository.GetCustomer(id.Value);
            if(customer == null) //customer not found
            {
                Response.StatusCode = 404;
                return View("CustomerNotFound", id.Value);
            }
            //view model is used when we want the view to contain information from 
            //different sources like: Customer class info and pageTitle 
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel() 
            {
                Customer = customer,
                PageTitle = "Customer details"
            };
            
            
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CustomerCreateViewModel model) //values from the form that user posts are mapped to this Customer obj
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);

                Customer newCustomer = new Customer
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    // Store the file name in PhotoPath property of the employee object
                    // which gets saved to the Employees database table
                    PhotoPath = uniqueFileName
                };

                _customerRepository.AddCustomer(newCustomer);
                return RedirectToAction("details", new { id = newCustomer.Id });
            }

            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Customer customer = _customerRepository.GetCustomer(id);
            CustomerEditViewModel customerEditViewModel = new CustomerEditViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Department = customer.Department,
                existingPhotoPath = customer.PhotoPath
            };
            return View(customerEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(CustomerEditViewModel model) //updated values from the form that user posts are mapped to this Customer obj
        {
            if (ModelState.IsValid)
            {
                //if validated then store the existing (before update) customer data in a variable
                Customer customer = _customerRepository.GetCustomer(model.Id); //taken from hidden Id field in edit.cshtml
                customer.Name = model.Name;
                customer.Email = model.Email;
                customer.Department = model.Department;
                if(model.Photo != null)
                {
                    if(model.existingPhotoPath != null)
                    {
                        string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", model.existingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    customer.PhotoPath = ProcessUploadedFile(model);
                }

                _customerRepository.EditCustomer(customer);
                return RedirectToAction("custs");
            }

            return View();
        }

        private string ProcessUploadedFile(CustomerCreateViewModel model)
        {
            string uniqueFileName = null;
            // Store the file name in PhotoPath property of the customer object
            // which gets saved to the Customer database table
            // If the Photo property on the incoming model object is not null, then the user
            // has selected an image to upload.
            if (model.Photo != null)
            {
                // The image must be uploaded to the images folder in wwwroot
                // To get the path of the wwwroot folder we are using the inject
                // HostingEnvironment service provided by ASP.NET Core
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                // To make sure the file name is unique we are appending a new
                // GUID value and and an underscore to the file name
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                // Use CopyTo() method provided by IFormFile interface to
                // copy the file to wwwroot/images folder
                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
                
            }

            return uniqueFileName;
        }
    }
}
