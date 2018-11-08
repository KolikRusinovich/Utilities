using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Utilities.Models;
using Utilities.ViewModels;

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
        public IActionResult Index(int page = 1)
        {
            int pageSize = 10;
            var paymentContext = context.Payments.Include(p => p.Tenant).Include(p => p.Rate);
            var count = paymentContext.Count();
            var items = paymentContext.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            PaymentsViewModel payments = new PaymentsViewModel
            {
                Payments = items,
                PaymentViewModel = _payment,
                PageViewModel = pageViewModel
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