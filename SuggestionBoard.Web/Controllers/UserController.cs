using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuggestionBoard.Core.Validation;
using SuggestionBoard.Data.Service;
using SuggestionBoard.Data.ViewModel;
using SuggestionBoard.Domain;
using SuggestionBoard.Web.Helper;

namespace SuggestionBoard.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _service;
        private readonly ICategoryService _categoryService;

        public UserController(ILogger<UserController> logger, IMapper mapper, UserManager<User> userManager,
            IUserService service, ICategoryService categoryService)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _service = service;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ProfileVM>> Profile(Guid? id = null, string sortOrder = "newest", int pageNumber = 1, Guid? categoryId = null)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["PageNumber"] = pageNumber;
            ViewData["Category"] = categoryId;

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (id == null || id == Guid.Empty)
                id = user.Id;

            var vm = await _service.GetProfileData(id.Value, user.Id, sortOrder, pageNumber, 5, categoryId);

            if (!vm.IsSuccessful)
                return RedirectToAction("Index", "Home");

            ToolbarVM toolbarVM = new ToolbarVM();
            toolbarVM.ControllerName = "User";
            toolbarVM.ActionName = "Profile";
            toolbarVM.ShowSearch = false;
            toolbarVM.Categories = _categoryService.GetSelectList();

            (vm.Rec as ProfileVM).ToolbarData = toolbarVM;

            return View(vm.Rec);
        }

        [HttpGet]
        public async Task<ActionResult<RegisterVM>> Edit()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ProfileUpdateVM vm = new ProfileUpdateVM();
            vm.EMail = user.Email;
            vm.PictureUrl = user.PictureUrl;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProfileUpdateVM model)
        {
            if (ModelState.IsValid)
            {
                var userData = await _userManager.FindByNameAsync(User.Identity.Name);

                if (!model.EMail.Equals(userData.Email))
                {
                    //var emailChangeToken = await _userManager.GenerateChangeEmailTokenAsync(userData, model.EMail);
                    //var emailChangeResult = await _userManager.ChangeEmailAsync(userData, model.EMail, emailChangeToken);

                    //if (!emailChangeResult.Succeeded)
                    //{
                    //    ModelState.AddModelError("GeneralError", "We couldn't change the EMail, please check your input");
                    //    return View(model);
                    //}
                }

                if (model.NewPassword != null && model.NewPassword != String.Empty)
                {
                    var hashedOldPassword = PasswordHasher.HashPassword(model.OldPassword);
                    var hashedNewPassword = PasswordHasher.HashPassword(model.NewPassword);
                    if (!hashedOldPassword.Equals(hashedNewPassword))
                    {
                        var passwordChangeResult = await _userManager.ChangePasswordAsync(userData, model.OldPassword, model.NewPassword);

                        if (!passwordChangeResult.Succeeded)
                        {
                            ModelState.AddModelError("GeneralError", "We couldn't change the Password, please check your input");
                            return View(model);
                        }
                    }
                }

                if (((model.PictureUrl.IsNullOrEmpty() && !userData.PictureUrl.IsNullOrEmpty())
                    || (!model.PictureUrl.IsNullOrEmpty() && userData.PictureUrl.IsNullOrEmpty())
                    || (!model.PictureUrl.IsNullOrEmpty() && !userData.PictureUrl.IsNullOrEmpty()
                        && !model.PictureUrl.Equals(userData.PictureUrl))) 
                    || !model.EMail.Equals(userData.Email))
                {
                    userData.UserName = !model.EMail.Equals(userData.Email) ? model.EMail : userData.Email;
                    userData.Email = userData.UserName;
                    userData.PictureUrl = !model.PictureUrl.IsNullOrEmpty() ? model.PictureUrl : "";
                    var pictureUpdateResult = await _userManager.UpdateAsync(userData);

                    if (!pictureUpdateResult.Succeeded)
                    {
                        ModelState.AddModelError("GeneralError", "We couldn't update your data, please check your the form values");
                        return View(model);
                    }
                }
            }
            else
            {
                return View(model);
            }

            return RedirectToAction("Logout", "Authentication");
        }
    }
}
