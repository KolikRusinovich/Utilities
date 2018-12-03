using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utilities.Models;
using Utilities.ViewModels;

namespace Utilities.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private UtilitiesContext _db;

        public HomeController(UtilitiesContext db)
        {
            _db = db;
        }

        private ReadingViewModel _reading = new ReadingViewModel
        {
            Type = "",
            Surname = ""
        };

        public IActionResult Index(int page = 1)
        {
            int pageSize = 10;
            var readingContext = _db.Readings.Include(p => p.Tenant).Include(p => p.Rate);
            var count = readingContext.Count();
            var items = readingContext.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            HomeViewModel homeViewModel = new HomeViewModel
            {
                Readings = items,
                ReadingViewModel = _reading,
                PageViewModel = pageViewModel
            };
            return View(homeViewModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}