using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utilities.Models;
using Utilities.ViewModels;
using Utilities.ViewModels.RatesViewModels;

namespace Utilities.Controllers
{
    public class RatesController : Controller
    {
        private UtilitiesContext context;

        public RatesController(UtilitiesContext context)
        {
            this.context = context;
        }

        public IActionResult Index(string type, string firstDate = "01.01.0001", string secondDate = "01.01.3001",int page = 1, SortState sortOrder = SortState.RateIdAsc)
        {
            DateTime first = Convert.ToDateTime(firstDate);
            DateTime second = Convert.ToDateTime(secondDate);
            int pageSize = 10;
            IQueryable<Rate> source = context.Rates;
            if (!String.IsNullOrEmpty(type))
            {
                source = source.Where(p => p.Type.Contains(type));
            }
            if (firstDate != null || secondDate != null)
            {
                source = source.Where(p => p.DateOfIntroduction >= first && p.DateOfIntroduction <= second);
            }
            switch (sortOrder)
            {
                case SortState.RateIdDesc:
                    source = source.OrderByDescending(s => s.RateId);
                    break;
                case SortState.TypeOfRateAsc:
                    source = source.OrderBy(s => s.Type);
                    break;
                case SortState.TypeOfRateDesc:
                    source = source.OrderByDescending(s => s.Type);
                    break;
                case SortState.ValueAsc:
                    source = source.OrderBy(s => s.Value);
                    break;
                case SortState.ValueDesc:
                    source = source.OrderByDescending(s => s.Value);
                    break;
                case SortState.DateOfRateAsc:
                    source = source.OrderBy(s => s.DateOfIntroduction);
                    break;
                case SortState.DateOfRateDesc:
                    source = source.OrderByDescending(s => s.DateOfIntroduction);
                    break;
                default:
                    source = source.OrderBy(s => s.RateId);
                    break;
            }
            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            RatesViewModel rates = new RatesViewModel
            {
                Rates = items,
                PageViewModel = pageViewModel,
                SortViewModel = new RatesSortViewModel(sortOrder),
                FilterViewModel = new RatesFilterViewModel(type, first, second)
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