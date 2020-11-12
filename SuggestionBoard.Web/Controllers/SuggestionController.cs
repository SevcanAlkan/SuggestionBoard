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
        public async Task<ActionResult<SuggestionSaveVM>> Detail(Guid? id = null)
        {
            SuggestionSaveVM rec = new SuggestionSaveVM();

            if (id.IsNull())
                return View(rec);

            var result = await _service.GetByIdAsync(id.Value);

            if (result != null)
                rec = _mapper.Map<SuggestionSaveVM>(result);

            return View(rec);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Detail(Guid id, SuggestionSaveVM record)
        {
            if (id != record.Id)
            {
                ModelState.AddModelError("GeneralError", "Invalid attempt!");
                return View(record);
            }

            if (!ModelState.IsValid)
            {
                return View(record);
            }

            APIResultVM result = new APIResultVM();

            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            if (record.Id.IsNull() || record.Id == Guid.Empty
                || id.IsNull() || id == Guid.Empty)
            {
                result = await _service.AddAsync(record, user.Id);
            }
            else
            {
                result = await _service.UpdateAsync(record.Id, record, user.Id);
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
                
                return View(record);
            }
            
            return RedirectToAction("Index", "Home");
        }
    }
}
