using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utilities.Models;
using Utilities.ViewModels;

namespace Utilities.Controllers
{
    public class ReadingsController : Controller
    {
        private readonly UtilitiesContext context;

        private ReadingViewModel _reading = new ReadingViewModel
        {
            Type = "",
            Surname = ""
        };

        public ReadingsController(UtilitiesContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var readingContext = context.Readings.Include(p => p.Tenant).Include(p => p.Rate);
            ReadingsViewModel readings = new ReadingsViewModel
            {
                Readings = readingContext.Take(50).ToList(),
                ReadingViewModel = _reading
            };
            return View(readings);
        }
    }
}