using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuggestionBoard.Data.Service;
using SuggestionBoard.Data.ViewModel;
using SuggestionBoard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuggestionBoard.Web.Controllers
{
    [Authorize]
    public class SuggestionCommentController : Controller
    {
        private readonly ISuggestionCommentService _service;
        private readonly ISuggestionService _suggestionService;
        private readonly IMapper _mapper;
        private readonly ILogger<SuggestionCommentController> _logger;
        private readonly UserManager<User> _userManager;

        public SuggestionCommentController(ILogger<SuggestionCommentController> logger, ISuggestionCommentService service, IMapper mapper, UserManager<User> userManager, ISuggestionService suggestionService)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _suggestionService = suggestionService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Send(SuggestionCommentSaveVM vm)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("GeneralError", "Form values are not valid!");
                return RedirectToAction("DetailCommentCallBack", "Suggestion", vm);
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            bool isSuggestionExist = await _suggestionService.AnyAsync(vm.SuggestionId);

            if (isSuggestionExist && user != null)
            {
                var result = await _service.AddAsync(vm, user.Id);

                if (!result.IsSuccessful)
                {
                    if (result.Messages.Any())
                    {
                        foreach (var error in result.Messages)
                        {
                            ModelState.AddModelError("GeneralError", error);
                        }
                    }

                    return RedirectToAction("DetailCommentCallBack", "Suggestion", vm);
                }
            }
            else
            {
                ModelState.AddModelError("GeneralError", "Comment couldnt posted!");
                return RedirectToAction("DetailCommentCallBack", "Suggestion", vm);
            }

            return RedirectToAction("Detail", "Suggestion", new { id = vm.SuggestionId });
        }
    }
}
