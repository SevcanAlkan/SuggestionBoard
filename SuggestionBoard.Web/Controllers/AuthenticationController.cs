using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SuggestionBoard.Core.Validation;
using SuggestionBoard.Data.ViewModel;
using SuggestionBoard.Domain;

namespace SuggestionBoard.Web.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register(string ReturnUrl = null)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var userCheck = await _userManager.FindByEmailAsync(model.EMail);
                if (userCheck == null)
                {
                    var user = new User
                    {
                        UserName = model.EMail,
                        NormalizedUserName = model.EMail,
                        Email = model.EMail,
                        PhoneNumber = "",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login", new { ReturnUrl = model.ReturnUrl });
                    }
                    else
                    {
                        if (result.Errors.Count() > 0)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("GeneralError", error.Description);
                            }
                        }
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("GeneralError", "Email already exists.");
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl = null)
        {
            LoginVM vm = new LoginVM();

            if (ReturnUrl != null)
                vm.ReturnUrl = ReturnUrl;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.EMail);
            if(user == null)
            {
                ModelState.AddModelError("EMail", "EMail address is not matching with any user!");
                return View(model);
            }

            if (await _userManager.CheckPasswordAsync(user, model.Password) == false)
            {
                ModelState.AddModelError("GeneralError", "Invalid credentials!");
                return View(model);

            }

            var canLogIn = await _signInManager.CanSignInAsync(user);

            if (!canLogIn)
            {
                ModelState.AddModelError("GeneralError", "You are unable to login right now!");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, true);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("GeneralError", "Invalid login attempt!");
                return View(model);
            }

            if (model.ReturnUrl.IsNullOrEmpty())
                return RedirectToAction("Index", "Home");
            else
                return Redirect(model.ReturnUrl);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //public IActionResult ForgotPassword()
        //{
        //    return View();
        //}

        //public IActionResult ResetPassword()
        //{
        //    return View();
        //}
    }
}
