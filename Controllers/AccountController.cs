using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessManagment.Models;
using FitnessManagment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace FitnessManagment.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //inject userManagement and signInManagment to use those services 
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AcceptVerbs("Get","Post")] //responds to get and post requests
        [AllowAnonymous]
        //serverside method that checks if the provided email is already taken 
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            //email was not found in the server so the user may continue the registration
            if(user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email { email } is already in use");
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            //check if the incoming model object is valid
            if (ModelState.IsValid)
            {
                //if model is valid create new user
                var user = new ApplicationUser { 
                    FullName = model.FullName,
                    UserName = model.Email, 
                    Email = model.Email,
                    City = model.City
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                //check if the user is created successfuly
                if (result.Succeeded)
                {
                    //if we signed in as Admin redirect to "ListUsers"
                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }
                    //if registration is complete Login that user
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }
                //if the creation of user is not successful add errors to model state
                foreach(var error in result.Errors)
                {
                    //this errors will be displayed in the Register view in the Validation Summary section
                    ModelState.AddModelError("", error.Description);
                }
            }
            //if model is NOT valid rerender the view with the model info 
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            //check if the incoming model object is valid
            if (ModelState.IsValid)
            {
                //if model is valid sign in the user
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                
                //check if the user is signed in successfuly
                if (result.Succeeded)
                {
                    //if the returnURL is not empty redirect to it
                    //if the redirect url is not local just go to index
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) 
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                }
                //if the signing in of user is not successful display th eneeded error
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }
            //if model is NOT valid rerender the view with the model info 
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
