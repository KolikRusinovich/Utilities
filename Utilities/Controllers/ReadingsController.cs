using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Utilities.Infrastructure;
using Utilities.Infrastructure.Filtres;
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
        private int tenantN = 0, rateN = 0;
        private string firstDateN = "01.01.0001", secondDateN = "10.01.2091";
        private int pageN = 1;
        private SortState sortOrderN = SortState.ReadingIdAsc;

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
        [SetToSession("Readings")]
        public IActionResult Index(int? tenant, int? rate, string firstDate = "01.01.0001", string secondDate = "10.01.2091",int page = 0, SortState sortOrder = SortState.ReadingIdAsc, string cacheKey = "NoCache")
        {
            var sessionOrganizations = HttpContext.Session.Get("Readings");
            if (sessionOrganizations != null && tenant == null && rate == null && firstDate == "01.01.0001" && secondDate == "10.01.2091" && page == 0 && sortOrder == SortState.ReadingIdAsc && cacheKey == "NoCache")
            {
                if (sessionOrganizations.Keys.Contains("tenant"))
                    tenant = Convert.ToInt32(sessionOrganizations["tenant"]);
                if (sessionOrganizations.Keys.Contains("rate"))
                    rate = Convert.ToInt32(sessionOrganizations["rate"]);
                if (sessionOrganizations.Keys.Contains("firstDate"))
                    firstDate = sessionOrganizations["firstDate"];
                if (sessionOrganizations.Keys.Contains("secondDate"))
                    secondDate = sessionOrganizations["secondDate"];
                if (sessionOrganizations.Keys.Contains("page"))
                    page = Convert.ToInt32(sessionOrganizations["page"]);
                if (sessionOrganizations.Keys.Contains("sortOrder"))
                    sortOrder = (SortState)Enum.Parse(typeof(SortState), sessionOrganizations["sortOrder"]);
                cacheKey = "ReadingsCache";
            }
            if (cacheKey == "Edit")
                cacheKey = "NoCache";
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
            context.SaveChanges();
            var sessionOrganizations = HttpContext.Session.Get("Readings");
                if (sessionOrganizations.Keys.Contains("tenant"))
                    tenantN = Convert.ToInt32(sessionOrganizations["tenant"]);
                if (sessionOrganizations.Keys.Contains("rate"))
                    rateN = Convert.ToInt32(sessionOrganizations["rate"]);
                if (sessionOrganizations.Keys.Contains("firstDate"))
                    firstDateN = sessionOrganizations["firstDate"];
                if (sessionOrganizations.Keys.Contains("secondDate"))
                    secondDateN = sessionOrganizations["secondDate"];
                if (sessionOrganizations.Keys.Contains("page"))
                    pageN = Convert.ToInt32(sessionOrganizations["page"]);
                if (sessionOrganizations.Keys.Contains("sortOrder"))
                    sortOrderN = (SortState)Enum.Parse(typeof(SortState), sessionOrganizations["sortOrder"]);
            return RedirectToAction("index", new { tenant = tenantN, rate = rateN,firstDate = firstDateN, secondDate = secondDateN, page = pageN, cacheKey = "Edit"});
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
            var sessionOrganizations = HttpContext.Session.Get("Readings");
            if (sessionOrganizations.Keys.Contains("tenant"))
                tenantN = Convert.ToInt32(sessionOrganizations["tenant"]);
            if (sessionOrganizations.Keys.Contains("rate"))
                rateN = Convert.ToInt32(sessionOrganizations["rate"]);
            if (sessionOrganizations.Keys.Contains("firstDate"))
                firstDateN = sessionOrganizations["firstDate"];
            if (sessionOrganizations.Keys.Contains("secondDate"))
                secondDateN = sessionOrganizations["secondDate"];
            if (sessionOrganizations.Keys.Contains("page"))
                pageN = Convert.ToInt32(sessionOrganizations["page"]);
            if (sessionOrganizations.Keys.Contains("sortOrder"))
                sortOrderN = (SortState)Enum.Parse(typeof(SortState), sessionOrganizations["sortOrder"]);
            return RedirectToAction("index", new { tenant = tenantN, rate = rateN, firstDate = firstDateN, secondDate = secondDateN, page = pageN, cacheKey = "Edit" });
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
            var sessionOrganizations = HttpContext.Session.Get("Readings");
            if (sessionOrganizations.Keys.Contains("tenant"))
                tenantN = Convert.ToInt32(sessionOrganizations["tenant"]);
            if (sessionOrganizations.Keys.Contains("rate"))
                rateN = Convert.ToInt32(sessionOrganizations["rate"]);
            if (sessionOrganizations.Keys.Contains("firstDate"))
                firstDateN = sessionOrganizations["firstDate"];
            if (sessionOrganizations.Keys.Contains("secondDate"))
                secondDateN = sessionOrganizations["secondDate"];
            if (sessionOrganizations.Keys.Contains("page"))
                pageN = Convert.ToInt32(sessionOrganizations["page"]);
            if (sessionOrganizations.Keys.Contains("sortOrder"))
                sortOrderN = (SortState)Enum.Parse(typeof(SortState), sessionOrganizations["sortOrder"]);
            return RedirectToAction("index", new { tenant = tenantN, rate = rateN, firstDate = firstDateN, secondDate = secondDateN, page = pageN, cacheKey = "Edit" });
        }
    }
}