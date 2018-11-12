using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Utilities.Models;
using Utilities.Services;
using Utilities.ViewModels;

namespace Utilities.Controllers
{
    public class TenantsController : Controller
    {
        private UtilitiesContext context;
        TenantService tenantService;

        public TenantsController(UtilitiesContext context,TenantService service)
        {
            this.context = context;
            tenantService = service;
        }
        public IActionResult Index(string name, string surname, string patronymic, int page = 1,SortState sortOrder = SortState.TenantIdAsc, string cacheKey = "NoCache")
        {
            TenantsViewModel tenants = tenantService.GetTenants(name, surname, patronymic, page, sortOrder,cacheKey);
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