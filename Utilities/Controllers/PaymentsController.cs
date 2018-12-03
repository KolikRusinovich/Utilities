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
using Utilities.ViewModels.PaymentsViewModels;

namespace Utilities.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly UtilitiesContext context;
        private PaymentService paymentService;
        private int tenantN = 0, rateN = 0;
        private string firstDateN = "01.01.0001", secondDateN = "01.01.3001";
        private int pageN = 1;
        private SortState sortOrderN = SortState.PaymentsIdAsc;
        private PaymentViewModel _payment = new PaymentViewModel
        {
            Type = "",
            Surname = ""
        };

        public PaymentsController(UtilitiesContext context, PaymentService paymentService)
        {
            this.context = context;
            this.paymentService = paymentService;
        }

        [SetToSession("Payments")]
        public IActionResult Index(int? tenant, int? rate, string firstDate = "01.01.0001", string secondDate = "01.01.3001", int page = 1, SortState sortOrder = SortState.PaymentsIdAsc, string cacheKey = "NoCache")
        {
            var sessionOrganizations = HttpContext.Session.Get("Payments");
            if (sessionOrganizations != null && tenant == null && rate == null && firstDate == "01.01.0001" && secondDate == "01.01.3001" && page == 1 && sortOrder == SortState.PaymentsIdAsc && cacheKey == "NoCache")
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
                cacheKey = "PaymentsCache";
            }
            if (cacheKey == "Edit")
                cacheKey = "NoCache";
            PaymentsViewModel payments = paymentService.GetPayments(tenant, rate, firstDate, secondDate, page, sortOrder, cacheKey);
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
            var sessionOrganizations = HttpContext.Session.Get("Payments");
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
            var sessionOrganizations = HttpContext.Session.Get("Payments");
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
            var sessionOrganizations = HttpContext.Session.Get("Payments");
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