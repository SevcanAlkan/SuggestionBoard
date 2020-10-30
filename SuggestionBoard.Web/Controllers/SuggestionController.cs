using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuggestionBoard.Core.Validation;
using SuggestionBoard.Core.ViewModel;
using SuggestionBoard.Data.Service;
using SuggestionBoard.Data.ViewModel;
using SuggestionBoard.Web.Models;

namespace SuggestionBoard.Web.Controllers
{
    public class SuggestionController : Controller
    {
        private ISuggestionService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;

        public SuggestionController(ILogger<HomeController> logger, ISuggestionService service, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
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
        public async Task<ActionResult> Detail(Guid id, SuggestionSaveVM record) //[Bind("Id, Title, Description")]
        {
            if (id != record.Id)
            {
                return NotFound();
                //view bag errors
            }

            if (!ModelState.IsValid)
            {
                return View(record);
                //view bag error messages
            }

            APIResultVM result = new APIResultVM();

            if (record.Id.IsNull() || record.Id == Guid.Empty
                || id.IsNull() || id == Guid.Empty)
            {
                result = await _service.AddAsync(record);
            }
            else
            {
                result = await _service.UpdateAsync(record.Id, record);
            }

            if (!result.IsSuccessful)
            {
                return View(record);
                //wiew bag error messages
            }

            // view bag success info
            return RedirectToAction("Index", "Home");
        }
    }
}
