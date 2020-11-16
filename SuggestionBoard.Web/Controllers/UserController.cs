using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SuggestionBoard.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public UserController()
        {

        }

        public IActionResult Profile(int? id = null)
        {
            //null ise mevcut kullanicin profili
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Save()
        {
            //to profile or edit
            return View();
        }
    }
}
