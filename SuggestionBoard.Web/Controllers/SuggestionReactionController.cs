using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuggestionBoard.Core.Enum;
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
    public class SuggestionReactionController : Controller
    {
        private readonly ISuggestionReactionService _service;
        private readonly ISuggestionService _suggestionService;
        private readonly IMapper _mapper;
        private readonly ILogger<SuggestionReactionController> _logger;
        private readonly UserManager<User> _userManager;

        public SuggestionReactionController(ILogger<SuggestionReactionController> logger, ISuggestionReactionService service, IMapper mapper, UserManager<User> userManager, ISuggestionService suggestionService)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _suggestionService = suggestionService;
        }

        [HttpGet]
        public async Task<ActionResult> Send(Guid suggestionId, UserReaction reaction)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user == null)
                return RedirectToAction("Register", "Authentication");

            bool isSuggestionExist = await _suggestionService.AnyAsync(suggestionId);
            if (!isSuggestionExist)
                return RedirectToAction("Index", "Home");

            bool isAlreadyReacted = await _service.AnyAsync(a => a.CreateBy == user.Id && a.SuggestionId == suggestionId);
            if (isAlreadyReacted)
                return RedirectToAction("Detail", "Suggestion", new { id = suggestionId });

            SuggestionReactionSaveVM model = new SuggestionReactionSaveVM()
            {
                SuggestionId = suggestionId,
                Reaction = reaction
            };

            var result = await _service.AddAsync(model, user.Id);
            if (result.IsSuccessful)
            {
                await _suggestionService.UpdateReactionCount(suggestionId, reaction);
            }

            return RedirectToAction("Detail", "Suggestion", new { id = suggestionId });
        }
    }
}
