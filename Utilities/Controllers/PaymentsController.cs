using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Utilities.Models;
using Utilities.ViewModels;
using Utilities.ViewModels.PaymentsViewModels;

namespace Utilities.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly UtilitiesContext context;
        private PaymentViewModel _payment = new PaymentViewModel
        {
            Type = "",
            Surname = ""
        };

        public PaymentsController(UtilitiesContext context)
        {
            this.context = context;
        }
        public IActionResult Index(int? tenant, int? rate, string firstDate = "01.01.0001", string secondDate = "20.01.3001", int page = 1, SortState sortOrder = SortState.PaymentsIdAsc)
        {
            DateTime first = Convert.ToDateTime(firstDate);
            DateTime second = Convert.ToDateTime(secondDate);
            int pageSize = 10;
            IQueryable<Payment> source = context.Payments.Include(p => p.Tenant).Include(p => p.Rate);
            if (tenant != null && tenant != 0)
                source = source.Where(p => p.TenantId == tenant);
            if (rate != null && rate != 0)
                source = source.Where(p => p.RateId == rate);
            if (firstDate != null || secondDate != null)
            {
                source = source.Where(p => p.DateOfPayment >= first && p.DateOfPayment <= second);
            }
            switch (sortOrder)
            {
                case SortState.PaymentsIdDesc:
                    source = source.OrderByDescending(s => s.PaymentId);
                    break;
                case SortState.SurameOfTenantAsc:
                    source = source.OrderBy(s => s.Tenant.Surname);
                    break;
                case SortState.SurnameOfTenantDesc:
                    source = source.OrderByDescending(s => s.Tenant.Surname);
                    break;
                case SortState.TypeOfRateAsc:
                    source = source.OrderBy(s => s.Rate.Type);
                    break;
                case SortState.TypeOfRateDesc:
                    source = source.OrderByDescending(s => s.Rate.Type);
                    break;
                case SortState.PaymentsSumAsc:
                    source = source.OrderBy(s => s.Sum);
                    break;
                case SortState.PaymentsSumDesc:
                    source = source.OrderByDescending(s => s.Sum);
                    break;
                case SortState.DateOfPaymentsAsc:
                    source = source.OrderBy(s => s.DateOfPayment);
                    break;
                case SortState.DateOfPaymentsDesc:
                    source = source.OrderByDescending(s => s.DateOfPayment);
                    break;
                default:
                    source = source.OrderBy(s => s.PaymentId);
                    break;
            }
            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            PaymentsViewModel payments = new PaymentsViewModel
            {
                Payments = items,
                PaymentViewModel = _payment,
                PageViewModel = pageViewModel,
                SortViewModel = new PaymentsSortViewModel(sortOrder),
                FilterViewModel = new PaymentsFilterViewModel(context.Tenants.ToList(), context.Rates.ToList(), tenant, rate, first, second)
            };
            return View(payments);
        }

        [HttpGet]
        public ActionResult EditPayment(int? id)
        {
            var paymentContext = context.Payments.Include(p => p.Tenant).Include(p => p.Rate);
            var items = paymentContext.Where(p => p.PaymentId == id).ToList();
            var tenants = new SelectList(context.Tenants, "TenantId", "Surname", items.First().TenantId); ;
            var rates = new SelectList(context.Rates, "RateId", "Type", items.First().RateId);
            PaymentsViewModel payments = new PaymentsViewModel
            {
                Payments = items,
                PaymentViewModel = _payment,
                TenantsList = tenants,
                RatesList = rates
            };
            return View(payments);
        }

        [HttpPost]
        public ActionResult EditPayment(Payment payment)
        {
            context.Payments.Update(payment);
            // сохраняем в бд все изменения
            context.SaveChanges();
            return RedirectToAction("index");
        }

        [HttpGet]
        [ActionName("DeletePayment")]
        public ActionResult ConfirmDeletePayment(int id)
        {
            var paymentContext = context.Payments.Include(p => p.Tenant).Include(p => p.Rate);
            var items = paymentContext.Where(p => p.PaymentId == id).ToList();
            var tenants = new SelectList(context.Tenants, "TenantId", "Surname"); ;
            var rates = new SelectList(context.Rates, "RateId", "Type");
            PaymentViewModel paymentView = new PaymentViewModel
            {
                PaymentId = items.First().PaymentId,
                Sum = items.First().Sum,
                DateOfPayment = items.First().DateOfPayment,
                Type = items.First().Rate.Type,
                Surname = items.First().Tenant.Surname
            };
            PaymentsViewModel payment = new PaymentsViewModel
            {
                Payments = items,
                PaymentViewModel = paymentView,
                TenantsList = tenants,
                RatesList = rates
            };
            if (items == null)
                return View("NotFound");
            else
                return View(payment);
        }

        [HttpPost]
        public ActionResult DeletePayment(int id)
        {
            try
            {
                var payment = context.Payments.FirstOrDefault(c => c.PaymentId == id);
                context.Payments.Remove(payment);
                context.SaveChanges();
            }
            catch { }
            return RedirectToAction("index"); ;
        }

        [HttpGet]
        public IActionResult CreatePayment()
        {
            var tenants = new SelectList(context.Tenants, "TenantId", "Surname"); ;
            var rates = new SelectList(context.Rates, "RateId", "Type");
            ViewBag.TenantID = tenants;
            ViewBag.RateId = rates;
            return View();
        }
        [HttpPost]
        public ActionResult CreatePayment(Payment payment)
        {
            context.Payments.Add(payment);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}