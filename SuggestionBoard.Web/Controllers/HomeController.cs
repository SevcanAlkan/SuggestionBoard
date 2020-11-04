using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuggestionBoard.Core.Validation;
using SuggestionBoard.Data.Service;
using SuggestionBoard.Data.ViewModel;
using SuggestionBoard.Web.Models;

namespace SuggestionBoard.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private ISuggestionService _suggestionService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ISuggestionService suggestionService)
        {
            _suggestionService = suggestionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IAsyncEnumerable<SuggestionVM>>> Index()
        {
            _logger.LogInformation("Home page openned");

            var result = await _suggestionService.GetAllAsync();
            //sadece gecerli olanlari cek ve sirala

            //order by ekle : date old/newest ve reaction

            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
