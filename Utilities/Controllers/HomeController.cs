using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Utilties.Models;
using Microsoft.Extensions.Configuration;
using Utilties.ViewModels;

namespace Utilties.Controllers
{
    public class HomeController : Controller
    {
        private UtilitiesContext _db;

        public HomeController(UtilitiesContext db)
        {
            _db = db;
        }

      public IActionResult Index()
        {
            //var readings = _db.Readings.Take(10).ToList();
            var tenants = _db.Tenants.Take(10).ToList();
            var rates = _db.Rates.Take(10).ToList();
            List<ReadingViewModel> readings = _db.Readings
                .Select(t => new ReadingViewModel { ReadingId = t.ReadingId, Surname = t.Tenant.Surname, Apartmentnumber = t.Apartmentnumber, Type = t.Rate.Type, CounterNumber = t.CounterNumber, Indications = t.Indications, DateOfReading = t.DateOfReading })
                .Take(10)
                .ToList();
            HomeViewModel homeViewModel = new HomeViewModel { Tenants = tenants, Rates = rates, Readings = readings };
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
