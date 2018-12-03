using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utilities.Infrastructure;
using Utilities.Infrastructure.Filtres;
using Utilities.Models;
using Utilities.Services;
using Utilities.ViewModels;
using Utilities.ViewModels.RatesViewModels;

namespace Utilities.Controllers
{
    public class RatesController : Controller
    {
        private UtilitiesContext context;
        RateService rateService;
        private string typeN = "", firstDateN = "01.01.1962", secondDateN = "01.01.2040";
        private int pageN = 1;
        private SortState sortOrderN = SortState.RateIdAsc;

        public RatesController(UtilitiesContext context,RateService rateService)
        {
            this.context = context;
            this.rateService = rateService;
        }
        [SetToSession("Rates")]
        public IActionResult Index(string type, string firstDate = "01.01.1962" , string secondDate = "01.01.2040",int page = 1, SortState sortOrder = SortState.RateIdAsc, string cacheKey = "NoCache")
        {
            var sessionOrganizations = HttpContext.Session.Get("Rates");
            if (sessionOrganizations != null && type == null && firstDate == "01.01.1962" && secondDate == "01.01.2040" && page == 1 && sortOrder == SortState.RateIdAsc && cacheKey == "NoCache")
            {
                if (sessionOrganizations.Keys.Contains("type"))
                    type = sessionOrganizations["type"];
                if (sessionOrganizations.Keys.Contains("firstDate"))
                    firstDate = sessionOrganizations["firstDate"];
                if (sessionOrganizations.Keys.Contains("secondDate"))
                    secondDate = sessionOrganizations["secondDate"];
                if (sessionOrganizations.Keys.Contains("page"))
                    page = Convert.ToInt32(sessionOrganizations["page"]);
                if (sessionOrganizations.Keys.Contains("sortOrder"))
                    sortOrder = (SortState)Enum.Parse(typeof(SortState), sessionOrganizations["sortOrder"]);
                cacheKey = "RateCache";
            }
            if (cacheKey == "Edit")
                cacheKey = "NoCache";
            RatesViewModel rates = rateService.GetRates(type, firstDate, secondDate, page, sortOrder, cacheKey);
            return View(rates);
        }

        [HttpGet]
        public ActionResult EditRate(int? id)
        {
            Rate rate = context.Rates.Find(id);
            return View(rate);
        }

        [HttpPost]
        public ActionResult EditRate(Rate rate)
        {
            context.Rates.Update(rate);
            // сохраняем в бд все изменения
            context.SaveChanges();
            var sessionOrganizations = HttpContext.Session.Get("Rates");
            if (sessionOrganizations.Keys.Contains("type"))
                typeN = sessionOrganizations["type"];
            if (sessionOrganizations.Keys.Contains("firstDate"))
                firstDateN = sessionOrganizations["firstDate"];
            if (sessionOrganizations.Keys.Contains("secondDate"))
                secondDateN = sessionOrganizations["secondDate"];
            if (sessionOrganizations.Keys.Contains("page"))
                pageN = Convert.ToInt32(sessionOrganizations["page"]);
            if (sessionOrganizations.Keys.Contains("sortOrder"))
                sortOrderN = (SortState)Enum.Parse(typeof(SortState), sessionOrganizations["sortOrder"]);
            return RedirectToAction("Index", new { type = typeN, firstDate = firstDateN, secondDate = secondDateN, page = pageN, sortOrder = sortOrderN, cacheKey = "Edit" });
        }

        [HttpGet]
        [ActionName("DeleteRate")]
        public ActionResult ConfirmDeleteRate(int id)
        {

            Rate rate = context.Rates.Find(id);

            if (rate == null)
                return View("NotFound");
            else
                return View(rate);
        }

        [HttpPost]
        public ActionResult DeleteRate(int id)
        {
            try
            {
                var rate = context.Rates.FirstOrDefault(c => c.RateId == id);
                context.Rates.Remove(rate);
                context.SaveChanges();
            }
            catch { }
            var sessionOrganizations = HttpContext.Session.Get("Rates");
            if (sessionOrganizations.Keys.Contains("type"))
                typeN = sessionOrganizations["type"];
            if (sessionOrganizations.Keys.Contains("firstDate"))
                firstDateN = sessionOrganizations["firstDate"];
            if (sessionOrganizations.Keys.Contains("secondDate"))
                secondDateN = sessionOrganizations["secondDate"];
            if (sessionOrganizations.Keys.Contains("page"))
                pageN = Convert.ToInt32(sessionOrganizations["page"]);
            if (sessionOrganizations.Keys.Contains("sortOrder"))
                sortOrderN = (SortState)Enum.Parse(typeof(SortState), sessionOrganizations["sortOrder"]);
            return RedirectToAction("Index", new { type = typeN, firstDate = firstDateN, secondDate = secondDateN, page = pageN, sortOrder = sortOrderN, cacheKey = "Edit" });
        }

        [HttpGet]
        public IActionResult CreateRate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateRate(Rate rate)
        {
            context.Rates.Add(rate);
            context.SaveChanges();
            var sessionOrganizations = HttpContext.Session.Get("Rates");
            if (sessionOrganizations.Keys.Contains("type"))
                typeN = sessionOrganizations["type"];
            if (sessionOrganizations.Keys.Contains("firstDate"))
                firstDateN = sessionOrganizations["firstDate"];
            if (sessionOrganizations.Keys.Contains("secondDate"))
                secondDateN = sessionOrganizations["secondDate"];
            if (sessionOrganizations.Keys.Contains("page"))
                pageN = Convert.ToInt32(sessionOrganizations["page"]);
            if (sessionOrganizations.Keys.Contains("sortOrder"))
                sortOrderN = (SortState)Enum.Parse(typeof(SortState), sessionOrganizations["sortOrder"]);
            return RedirectToAction("Index", new { type = typeN, firstDate = firstDateN, secondDate = secondDateN,page = pageN, sortOrder = sortOrderN, cacheKey = "Edit"});
        }
    }
}