using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Utilities.Models;
using Utilities.Services;
using Utilities.ViewModels;
using Utilities.ViewModels.ReadingsViewModels;

namespace Utilities.Controllers
{
    public class ReadingsController : Controller
    {
        private readonly UtilitiesContext context;
        private ReadingService readingService;

        private ReadingViewModel _reading = new ReadingViewModel
        {
            Type = "",
            Surname = ""
        };

        public ReadingsController(UtilitiesContext context, ReadingService readingService)
        {
            this.context = context;
            this.readingService = readingService;
        }
        public IActionResult Index(int? tenant, int? rate, string firstDate = "01.01.0001", string secondDate = "10.01.2091",int page = 1, SortState sortOrder = SortState.ReadingIdAsc, string cacheKey = "NoCache")
        {
            ReadingsViewModel readings = readingService.GetReadings(tenant, rate, firstDate, secondDate, page, sortOrder, cacheKey);
            return View(readings);
        }

        [HttpGet]
        public ActionResult EditReading(int? id)
        {
            var readingContext = context.Readings.Include(p => p.Tenant).Include(p => p.Rate);
            var items = readingContext.Where(p => p.ReadingId == id).ToList();
            var tenants  = new SelectList(context.Tenants, "TenantId", "Surname", items.First().TenantId);
            var rates = new SelectList(context.Rates, "RateId", "Type", items.First().RateId);
            ReadingsViewModel reading = new ReadingsViewModel
            {
                Readings = items,
                ReadingViewModel = _reading,
                TenantsList = tenants,
                RatesList = rates
            };
            return View(reading);
        }

        [HttpPost]
        public ActionResult EditReading(Reading reading)
        {
            context.Readings.Update(reading);
            // сохраняем в бд все изменения
            context.SaveChanges();
            return RedirectToAction("index");
        }

        [HttpGet]
        [ActionName("DeleteReading")]
        public ActionResult ConfirmDeleteReading(int id)
        {
            var readingContext = context.Readings.Include(p => p.Tenant).Include(p => p.Rate);
            var items = readingContext.Where(p => p.ReadingId == id).ToList();
            var tenants = new SelectList(items, "TenantId", "Surname"); ;
            var rates = new SelectList(context.Rates, "RateId", "Type");
            ReadingViewModel readingView = new ReadingViewModel
            {
                ReadingId = items.First().ReadingId,
                Apartmentnumber = items.First().Apartmentnumber,
                CounterNumber = items.First().CounterNumber,
                DateOfReading = items.First().DateOfReading,
                Indications = items.First().Indications,
                Type = items.First().Rate.Type,
                Surname = items.First().Tenant.Surname
            };
            ReadingsViewModel reading = new ReadingsViewModel
            {
                Readings = items,
                ReadingViewModel = readingView,
                TenantsList = tenants,
                RatesList = rates
            };
            if (items == null)
                return View("NotFound");
            else
                return View(reading);
        }

        [HttpPost]
        public ActionResult DeleteReading(int id)
        {
            try
            {
                var reading = context.Readings.FirstOrDefault(c => c.ReadingId == id);
                context.Readings.Remove(reading);
                context.SaveChanges();
            }
            catch { }
            return RedirectToAction("index"); ;
        }

        [HttpGet]
        public IActionResult CreateReading()
        {
            var tenants = new SelectList(context.Tenants, "TenantId", "Surname"); ;
            var rates = new SelectList(context.Rates, "RateId", "Type");
            ViewBag.TenantID = tenants;
            ViewBag.RateId = rates;
            return View();
        }
        [HttpPost]
        public ActionResult CreateReading(Reading reading)
        {
            context.Readings.Add(reading);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}