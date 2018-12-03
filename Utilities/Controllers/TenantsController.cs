using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Utilities.Infrastructure;
using Utilities.Infrastructure.Filtres;
using Utilities.Models;
using Utilities.Services;
using Utilities.ViewModels;

namespace Utilities.Controllers
{
    public class TenantsController : Controller
    {
        private UtilitiesContext context;
        TenantService tenantService;
        private string nameN = "", surnameN = "", patronymicN = "";
        private SortState sortOrderN = SortState.TenantIdAsc;
        private int pageN = 1;

        public TenantsController(UtilitiesContext context,TenantService service)
        {
            this.context = context;
            tenantService = service;
        }
        [SetToSession("Tenants")]
        public IActionResult Index(string name, string surname, string patronymic, int page = 1,SortState sortOrder = SortState.TenantIdAsc, string cacheKey = "NoCache")
        {
            var sessionOrganizations = HttpContext.Session.Get("Tenants");
            if (sessionOrganizations != null && name == null && surname == null && page == 1 && sortOrder == SortState.TenantIdAsc && cacheKey == "NoCache")
            {
                if (sessionOrganizations.Keys.Contains("name"))
                    name = sessionOrganizations["name"];
                if (sessionOrganizations.Keys.Contains("surname"))
                    surname = sessionOrganizations["surname"];
                if (sessionOrganizations.Keys.Contains("patronymic"))
                    patronymic = sessionOrganizations["patronymic"];
                if (sessionOrganizations.Keys.Contains("page"))
                    page = Convert.ToInt32(sessionOrganizations["page"]);
                if (sessionOrganizations.Keys.Contains("sortOrder"))
                    sortOrder = (SortState)Enum.Parse(typeof(SortState), sessionOrganizations["sortOrder"]);
                cacheKey = "TenantsCache";
            }
            if (cacheKey == "Edit")
                cacheKey = "NoCache";
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
            context.SaveChanges();
            var sessionOrganizations = HttpContext.Session.Get("Tenants");
            if (sessionOrganizations.Keys.Contains("name"))
                nameN = sessionOrganizations["name"];
            if (sessionOrganizations.Keys.Contains("surname"))
                surnameN = sessionOrganizations["surname"];
            if (sessionOrganizations.Keys.Contains("patronymic"))
                patronymicN = sessionOrganizations["patronymic"];
            if (sessionOrganizations.Keys.Contains("page"))
                pageN = Convert.ToInt32(sessionOrganizations["page"]);
            if (sessionOrganizations.Keys.Contains("sortOrder"))
                sortOrderN = (SortState)Enum.Parse(typeof(SortState), sessionOrganizations["sortOrder"]);
            return RedirectToAction("index", new { name = nameN, surname = surnameN, patronymic = patronymicN, page = pageN, sortOrder = sortOrderN, cacheKey = "Edit"});
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
            var sessionOrganizations = HttpContext.Session.Get("Tenants");
            if (sessionOrganizations.Keys.Contains("name"))
                nameN = sessionOrganizations["name"];
            if (sessionOrganizations.Keys.Contains("surname"))
                surnameN = sessionOrganizations["surname"];
            if (sessionOrganizations.Keys.Contains("patronymic"))
                patronymicN = sessionOrganizations["patronymic"];
            if (sessionOrganizations.Keys.Contains("page"))
                pageN = Convert.ToInt32(sessionOrganizations["page"]);
            if (sessionOrganizations.Keys.Contains("sortOrder"))
                sortOrderN = (SortState)Enum.Parse(typeof(SortState), sessionOrganizations["sortOrder"]);
            return RedirectToAction("index", new { name = nameN, surname = surnameN, patronymic = patronymicN, page = pageN, sortOrder = sortOrderN, cacheKey = "Edit" });
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
            var sessionOrganizations = HttpContext.Session.Get("Tenants");
            if (sessionOrganizations.Keys.Contains("name"))
                nameN = sessionOrganizations["name"];
            if (sessionOrganizations.Keys.Contains("surname"))
                surnameN = sessionOrganizations["surname"];
            if (sessionOrganizations.Keys.Contains("patronymic"))
                patronymicN = sessionOrganizations["patronymic"];
            if (sessionOrganizations.Keys.Contains("page"))
                pageN = Convert.ToInt32(sessionOrganizations["page"]);
            if (sessionOrganizations.Keys.Contains("sortOrder"))
                sortOrderN = (SortState)Enum.Parse(typeof(SortState), sessionOrganizations["sortOrder"]);
            return RedirectToAction("index", new { name = nameN, surname = surnameN, patronymic = patronymicN, page = pageN, sortOrder = sortOrderN, cacheKey = "Edit" });
        }
    }
}