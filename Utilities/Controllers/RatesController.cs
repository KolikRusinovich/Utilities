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
    public class RatesController : Controller
    {
        private UtilitiesContext context;

        public RatesController(UtilitiesContext context)
        {
            this.context = context;
        }

        public IActionResult Index(int page = 1)
        {
            int pageSize = 10;
            var ratesContext = context.Rates;
            var count = ratesContext.Count();
            var items = ratesContext.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            RatesViewModel rates = new RatesViewModel
            {
                Rates = items,
                PageViewModel = pageViewModel
            };
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