using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WishList.Models;
using WishList.Models.AccountViewModels;

namespace WishList.Controllers
{

    public class Connection
    {
        public SqlConnection sqlConnection { get; set; }

        public Connection()
        {
            //"Server=127.0.0.1;Port=3306;Database=testdb;Uid=sa;Pwd = admin;" //keyword not supported post
            //sqlConnection = new SqlConnection("Server=127.0.0.1;port=3306;Database=testdb;Uid=sa;Pwd=admin;");
        }
    }
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            /*
            Console.WriteLine("Register HttpGet");
            using (SqlConnection test = new Connection().sqlConnection)
            {
                try
                {
                    test.Open();
                    SqlCommand sqlCommand = new SqlCommand("select * from user", test);
                    sqlCommand.ExecuteNonQuery();
                }
                catch
                {
                    Console.WriteLine("unable to open db");
                }
            }
            */
            
                
               
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = _userManager.CreateAsync(new ApplicationUser() { Email = model.Email, UserName = model.Email }, model.Password).Result;
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("Password", errorMessage:error.Description);
                }
                return View(model);
            
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false).Result;

            if (!result.Succeeded)
            {
               
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                
                return View(model);

            }

            return RedirectToAction("Index", "Item");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
