using FitnessManagment.Models;
using FitnessManagment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessManagment.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        //this field is needed to populate the users list of the editRole method
        private readonly UserManager<ApplicationUser> _userManager; 

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users; //pass to the view all the application users
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            //if validation is ok
            if (ModelState.IsValid)
            {
                //we want to create a new Identity role object
                //in ASP.NET a role is represented by this built in IdentityRole class
                IdentityRole identityRole = new IdentityRole { Name = model.RoleName };
                //use injected _roleManager to create the role in the database (dbo.AspNetRoles table)
                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                //if the role creation is ok
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "administration");
                }
                //if the creation of user is not successful add errors to model state
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(model);
        }

        //method to get all the roles in the application
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles; //takes all roles from database
            return View(roles); //passes to view
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            //use _roleManager to find the id of the role from the database
            var role = await _roleManager.FindByIdAsync(id);
            
            //if we cant find the coresponding role in the database
            if(role == null)
            {
                //add a message for the error view
                ViewBag.ErrorMessage = $"Role with Id= {id} cannot be found";
                return View("NotFound");
            }

            //if we found the role lets create a new instance of EditRoleViewModel class
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
            };
            //_userManager.Users returns all the users in our application
            foreach (var user in _userManager.Users)
            {
                //we need to check that the current user we iterating over has a role
                if(await _userManager.IsInRoleAsync(user, role.Name))
                {
                    //if the current user is in role we need to add his name to the
                    //users field of our EditRoleViewModel instance
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            //use _roleManager to check if the role is in DB
            var role = await _roleManager.FindByIdAsync(model.Id);

            //if we cant find the coresponding role in the database
            if (role == null)
            {
                //add a message for the error view
                ViewBag.ErrorMessage = $"Role with Id= {model.Id} cannot be found";
                return View("NotFound");
            }
            //if we found the role we want to edit in the DB
            else
            {
                //update the RoleName property with the incoming name from the model 
                role.Name = model.RoleName;

                //now we want to update the AspNetRoles in the DB for that we can use _roleManager
                //and update method that needs role to be passed at
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    //if true - we know data is updated
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        //method activated when we want to "Add or Remove users" at "EditRole" view
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            //we store roleId in viewbag and not at the "UserRoleViewModel" model 
            //because we dont want to duplicate roleId for each user
            ViewBag.roleId = roleId;

            //in order to know wich role we want add or remove users to 
            //we need to fetch the role from DB using _roleManager
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null) //role not found at the DB
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            //role is found, lets create instance of  UserRoleViewModel List
            var model = new List<UserRoleViewModel>();

            //loop through all registered users in the application
            foreach (var user in _userManager.Users)
            {
                //and for each user create an instance of UserRoleViewModel
                //so we can determine which role each user has
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                //check if the cuurent user we iterating through is already assigned 
                //to the given role 
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    // set the property to true
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                //add user to the list of UserRoleViewModel
                model.Add(userRoleViewModel);
            }

            return View(model);
        }


        [HttpPost]
        //method activated when we post the update button in the "EditUsersInRole" view
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            //fetch role from DB
            var role = await _roleManager.FindByIdAsync(roleId);

            //role not found
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            //role found, we want to do some proccessing
            //loop through each "UserRoleViewModel" from the incoming model obj
            for (int i = 0; i < model.Count; i++)
            {
                //for each registered user we need to determine if that user is 
                //in this particular role or not
                var user = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                //if we selected that specific user i to be assigned to that role 
                //and he isnt already been assigned to that role 
                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    //then add the user 
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }

                //if the box of that user is unchecked but he was assigned to that role 
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    //then remove him from that role
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    //does nothing & continius the looping
                    continue;
                }

                //if DB update was successful
                if (result.Succeeded)
                {
                    //if i < count then we have more users to proccess and we need to continue looping
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }

    }
}
