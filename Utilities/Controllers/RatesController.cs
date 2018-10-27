using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Utilities.Models;

namespace Utilities.Controllers
{
    public class RatesController : Controller
    {
        private UtilitiesContext context;

        public RatesController(UtilitiesContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var rates = context.Rates.ToList();
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
            context.Rates.Add(rate);
            // сохраняем в бд все изменения
            context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}