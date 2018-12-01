using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public RatesController(UtilitiesContext context,RateService rateService)
        {
            this.context = context;
            this.rateService = rateService;
        }

        public IActionResult Index(string type, string firstDate = "01.01.1962" , string secondDate = "01.01.2040",int page = 1, SortState sortOrder = SortState.RateIdAsc, string cacheKey = "NoCache")
        {
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
            return RedirectToAction("index");
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
            return RedirectToAction("index"); ;
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
            return RedirectToAction("Index");
        }
    }
}