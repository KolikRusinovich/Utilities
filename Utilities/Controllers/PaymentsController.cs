using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Utilities.Infrastructure;
using Utilities.Infrastructure.Filtres;
using Utilities.Models;
using Utilities.Services;
using Utilities.ViewModels;
using Utilities.ViewModels.PaymentsViewModels;
using Utilities.ViewModels.ProcedureViewModels;

namespace Utilities.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly UtilitiesContext context;
        private PaymentService paymentService;
        private int tenantN = 0, rateN = 0;
        private string firstDateN = "01.01.1981", secondDateN = "10.01.2091";
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
        public IActionResult Index(int? tenant, int? rate, string firstDate = "01.01.1981", string secondDate = "10.01.2091", int page = 0, SortState sortOrder = SortState.PaymentsIdAsc, string cacheKey = "NoCache")
        {
            var sessionOrganizations = HttpContext.Session.Get("Payments");
            if (sessionOrganizations != null && tenant == null && rate == null && firstDate == "01.01.1981" && secondDate == "10.01.2091" && page == 0 && sortOrder == SortState.PaymentsIdAsc && cacheKey == "NoCache")
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

        public async Task<IActionResult> Information(string d0 = "01.01.1970", string d = "01.01.2030", int page = 1)
        {
            DateTime firstDate, secondDate;
            int pageSize = 10;  // количество элементов на странице
            List<object[]> source = new List<object[]>();
            firstDate = Convert.ToDateTime(d0);
            secondDate = Convert.ToDateTime(d);
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                DbParameter parameter = command.CreateParameter();
                parameter.DbType = System.Data.DbType.DateTime;
                parameter.ParameterName = "@FirstDate";
                parameter.Value = firstDate;
                command.Parameters.Add(parameter);
                DbParameter parameter2 = command.CreateParameter();
                parameter2.DbType = System.Data.DbType.DateTime;
                parameter2.ParameterName = "@SecondDate";
                parameter2.Value = secondDate;
                command.Parameters.Add(parameter2);
                command.CommandText = "Information";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Connection.Open();
                DbDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        object[] item = new object[reader.FieldCount];
                        reader.GetValues(item);
                        source.Add(item);
                    }
                }
                command.Connection.Close();
            }
            var count = source.Count;
            var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            ProcedureViewModel viewModel = new ProcedureViewModel
            {
                FilterViewModel = new FilterViewModel(source, d0, d),
                PageViewModel = pageViewModel,
                Objects = items,
            };
            return View(viewModel);
        }


        [HttpGet]
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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