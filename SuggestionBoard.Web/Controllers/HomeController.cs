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
        private readonly ICategoryService _categoryService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ISuggestionService suggestionService, ICategoryService categoryService)
        {
            _suggestionService = suggestionService;
            _logger = logger;
            _categoryService = categoryService;
        }

        [HttpGet]
        public ActionResult<IAsyncEnumerable<SuggestionPaggingListVM>> Index(string sortOrder = "newest", string searchString = "", int pageNumber = 1, Guid? categoryId = null)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = searchString;
            ViewData["PageNumber"] = pageNumber;
            ViewData["Category"] = categoryId;

            var suggestions = _suggestionService.GetList(false, searchString, sortOrder, pageNumber, 5, categoryId);
            suggestions.Pagging.ActionName = "Index";
            suggestions.Pagging.ControllerName = "Home";

            ToolbarVM toolbarVM = new ToolbarVM();
            toolbarVM.ControllerName = "Home";
            toolbarVM.ActionName = "Index";
            toolbarVM.ShowSearch = true;
            toolbarVM.Categories = _categoryService.GetSelectList();

            suggestions.ToolbarData = toolbarVM;

            return View(suggestions);
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
