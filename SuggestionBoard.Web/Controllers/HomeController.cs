using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuggestionBoard.Core.Validation;
using SuggestionBoard.Data.Service;
using SuggestionBoard.Data.ViewModel;
using SuggestionBoard.Web.Models;

namespace SuggestionBoard.Web.Controllers
{
    public class HomeController : Controller
    {
        private ISuggestionService _service;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ISuggestionService service)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IAsyncEnumerable<SuggestionVM>>> Index()
        {
            var result = await _service.GetAllAsync();
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
