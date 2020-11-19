
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuggestionBoard.Core.Validation;
using SuggestionBoard.Core.ViewModel;
using SuggestionBoard.Data.Service;
using SuggestionBoard.Data.ViewModel;
using SuggestionBoard.Domain;
using SuggestionBoard.Web.Models;

namespace SuggestionBoard.Web.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryController> _logger;
        private readonly UserManager<User> _userManager;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService service, IMapper mapper, UserManager<User> userManager)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<CategoryPaggingListVM>> Index(int pageNumber = 1)
        {
            ViewData["PageNumber"] = pageNumber;

            var result = _service.GetList(false, pageNumber, 10);
            result.Pagging.ActionName = "Index";
            result.Pagging.ControllerName = "Category";

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            result.CurrentUserId = user != null ? user.Id : Guid.Empty;

            return View(result);
        }

        [HttpGet]
        public async Task<ActionResult<CategoryDetailVM>> Detail(Guid? id = null)
        {
            var result = await _service.GetForEdit(id);
            result.CanEdit = true;

            if (result != null)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                result.CanEdit = user.Id == result.Rec.CreateBy;
            }

            return View(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Detail(Guid id, CategoryDetailVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            APIResultVM result = new APIResultVM();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (id.IsNull() || id == Guid.Empty)
            {
                result = await _service.AddAsync(vm.Rec, user.Id);
            }
            else
            {
                result = await _service.UpdateAsync(id, vm.Rec, user.Id);
            }

            if (!result.IsSuccessful)
            {
                if (result.Messages.Any())
                {
                    foreach (var error in result.Messages)
                    {
                        ModelState.AddModelError("GeneralError", error);
                    }
                }
                
                return View(vm);
            }
            
            return RedirectToAction("Index", "Category");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid? id = null)
        {            
            if (id.IsNull())
                return RedirectToAction("Index", "Home");

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var result = await _service.DeleteAsync(id.Value, user.Id, true);               

            return RedirectToAction("Index", "Category");
        }
    }
}
