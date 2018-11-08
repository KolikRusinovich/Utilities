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
        public IActionResult Index(string name, string surname, string patronymic, int page = 1,SortState sortOrder = SortState.TenantIdAsc)
        {
            int pageSize = 10;
            var tenantContext = context.Tenants;
            IQueryable<Tenant> source = context.Tenants;
            if (!String.IsNullOrEmpty(name))
            {
                source = source.Where(p => p.NameOfTenant.Contains(name));
            }
            if (!String.IsNullOrEmpty(surname))
            {
                source = source.Where(p => p.Surname.Contains(surname));
            }
            if (!String.IsNullOrEmpty(patronymic))
            {
                source = source.Where(p => p.Patronymic.Contains(patronymic));
            }
            switch (sortOrder)
            {
                case SortState.TenantIdDesc:
                    source = source.OrderByDescending(s => s.TenantId);
                    break;
                case SortState.NameOfTenantAsc:
                    source = source.OrderBy(s => s.NameOfTenant);
                    break;
                case SortState.NameOfTenantDesc:
                    source = source.OrderByDescending(s => s.NameOfTenant);
                    break;
                case SortState.SurameOfTenantAsc:
                    source = source.OrderBy(s => s.Surname);
                    break;
                case SortState.SurnameOfTenantDesc:
                    source = source.OrderByDescending(s => s.Surname);
                    break;
                case SortState.PatronymicOfTenantAsc:
                    source = source.OrderBy(s => s.Patronymic);
                    break;
                case SortState.PatronymicOfTenantDesc:
                    source = source.OrderByDescending(s => s.Patronymic);
                    break;
                case SortState.ApartmentNumberAsc:
                    source = source.OrderBy(s => s.ApartmentNumber);
                    break;
                case SortState.ApartmentNumberDesc:
                    source = source.OrderByDescending(s => s.ApartmentNumber);
                    break;
                case SortState.NumberOfPeopleAsc:
                    source = source.OrderBy(s => s.NumberOfPeople);
                    break;
                case SortState.NumberOfPeopleDesc:
                    source = source.OrderByDescending(s => s.NumberOfPeople);
                    break;
                case SortState.TotalAreaAsc:
                    source = source.OrderBy(s => s.TotalArea);
                    break;
                case SortState.TotalAreaDesc:
                    source = source.OrderByDescending(s => s.TotalArea);
                    break;
                default:
                    source = source.OrderBy(s => s.TenantId);
                    break;
            }
            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
           
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            TenantsViewModel tenants = new TenantsViewModel
            {
                Tenants = items,
                PageViewModel = pageViewModel,
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(context.Tenants.ToList(), name, surname, patronymic),
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