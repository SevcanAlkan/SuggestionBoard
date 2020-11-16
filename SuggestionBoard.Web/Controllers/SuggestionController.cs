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
    public class SuggestionController : Controller
    {
        private readonly ISuggestionService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<SuggestionController> _logger;
        private readonly UserManager<User> _userManager;

        public SuggestionController(ILogger<SuggestionController> logger, ISuggestionService service, IMapper mapper, UserManager<User> userManager)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<SuggestionDetailVM>> Detail(Guid? id = null)
        {
            return View(await GetSuggestion(id));
        }

        [HttpGet]
        public async Task<ActionResult<SuggestionDetailVM>> DetailCommentCallBack(SuggestionCommentSaveVM vm)
        {
            ViewData["CommentFormData"] = vm;

            return View("Detail", await GetSuggestion(vm.SuggestionId));
        }

        [HttpGet]
        public async Task<ActionResult<SuggestionDetailVM>> DetailReactionCallBack(SuggestionReactionSaveVM vm)
        {
            return View("Detail", await GetSuggestion(vm.SuggestionId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Detail(Guid id, SuggestionDetailVM vm)
        {
            //if (id != vm.Rec.Id)
            //{
            //    ModelState.AddModelError("GeneralError", "Invalid attempt!");
            //    return View(vm);
            //}

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            APIResultVM result = new APIResultVM();

            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

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
            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid? id = null)
        {            
            if (id.IsNull())
                return RedirectToAction("Index", "Home");

            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var result = await _service.DeleteAsync(id.Value, user.Id, true);               

            return RedirectToAction("Index", "Home");
        }

        private async Task<SuggestionDetailVM> GetSuggestion(Guid? id = null)
        {
            var result = await _service.GetWithAdditionalData(id);
            result.CanEdit = true;

            if (result != null)
            {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                result.CanEdit = user.Id == result.Rec.CreateBy;
            }

            return result;
        }
    }
}
