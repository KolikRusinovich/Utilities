using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Utilities.Models;
using Utilities.ViewModels;
using Utilities.ViewModels.ReadingsViewModels;

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
        public IActionResult Index(int? tenant, int? rate, string firstDate = "01.01.0001", string secondDate = "20.01.3001",int page = 1, SortState sortOrder = SortState.ReadingIdAsc)
        {
            DateTime first = Convert.ToDateTime(firstDate);
            DateTime second = Convert.ToDateTime(secondDate);
            int pageSize = 10;
            IQueryable<Reading> source = context.Readings.Include(p => p.Tenant).Include(p => p.Rate);
            if(tenant != null && tenant != 0)
                source = source.Where(p => p.TenantId==tenant);
            if (rate != null && rate != 0)
                source = source.Where(p => p.RateId == rate);
            if (firstDate != null || secondDate !=null)
            {
                source = source.Where(p => p.DateOfReading>=first && p.DateOfReading <=second);
            }
            switch (sortOrder)
            {
                case SortState.ReadingIdDesc:
                    source = source.OrderByDescending(s => s.ReadingId);
                    break;
                case SortState.SurameOfTenantAsc:
                    source = source.OrderBy(s => s.Tenant.Surname);
                    break;
                case SortState.SurnameOfTenantDesc:
                    source = source.OrderByDescending(s => s.Tenant.Surname);
                    break;
                case SortState.ApartmentNumberAsc:
                    source = source.OrderBy(s => s.Apartmentnumber);
                    break;
                case SortState.ApartmentNumberDesc:
                    source = source.OrderByDescending(s => s.Apartmentnumber);
                    break;
                case SortState.TypeOfRateAsc:
                    source = source.OrderBy(s => s.Rate.Type);
                    break;
                case SortState.TypeOfRateDesc:
                    source = source.OrderByDescending(s => s.Rate.Type);
                    break;
                case SortState.CounterNumberAsc:
                    source = source.OrderBy(s => s.CounterNumber);
                    break;
                case SortState.CounterNumberDesc:
                    source = source.OrderByDescending(s => s.CounterNumber);
                    break;
                case SortState.IndicationsAsc:
                    source = source.OrderBy(s => s.Indications);
                    break;
                case SortState.IndicationsDesc:
                    source = source.OrderByDescending(s => s.Indications);
                    break;
                case SortState.DateOfReadingAsc:
                    source = source.OrderBy(s => s.DateOfReading);
                    break;
                case SortState.DateOfReadingDesc:
                    source = source.OrderByDescending(s => s.DateOfReading);
                    break;
                default:
                    source = source.OrderBy(s => s.ReadingId);
                    break;
            }
            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            ReadingsViewModel readings = new ReadingsViewModel
            {
                Readings = items,
                ReadingViewModel = _reading,
                PageViewModel = pageViewModel,
                SortViewModel = new ReadingSortViewModel(sortOrder),
                FilterViewModel = new ReadingsFilterViewModel(context.Tenants.ToList(), context.Rates.ToList(), tenant, rate,first, second)
            };
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