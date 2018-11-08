using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Utilities.Models;
using Utilities.ViewModels;

namespace Utilities.Controllers
{
    public class TenantsController : Controller
    {
        private UtilitiesContext context;

        public TenantsController(UtilitiesContext context)
        {
            this.context = context;
        }
        public IActionResult Index(int page = 1)
        {
            int pageSize = 10;
            var tenantContext = context.Tenants;
            var count = tenantContext.Count();
            var items = tenantContext.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            TenantsViewModel tenants = new TenantsViewModel
            {
                Tenants = items,
                PageViewModel = pageViewModel
            };
            return View(tenants);
        }

        [HttpGet]
        public ActionResult EditTenant(int? id)
        {
            Tenant tenant = context.Tenants.Find(id);
            return View(tenant);
        }

        [HttpPost]
        public ActionResult EditTenant(Tenant tenant)
        {
            context.Tenants.Update(tenant);
            // сохраняем в бд все изменения
            context.SaveChanges();
            return RedirectToAction("index");
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
        [ActionName("DeleteTenant")]
        public ActionResult ConfirmDeleteTenant(int id)
        {

            Tenant tenant = context.Tenants.Find(id);

            if (tenant == null)
                return View("NotFound");
            else
                return View(tenant);
        }

        [HttpPost]
        public ActionResult DeleteTenant(int id)
        {
            try
            {
                var tenant = context.Tenants.FirstOrDefault(c => c.TenantId == id);
                context.Tenants.Remove(tenant);
                context.SaveChanges();
            }
            catch { }
            return RedirectToAction("index"); ;
        }

        [HttpGet]
        public IActionResult CreateTenant()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateTenant(Tenant tenant)
        {
            context.Tenants.Add(tenant);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}