using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Models;
using Utilities.ViewModels;

namespace Utilities.Services
{
    public class TenantService
    {
        private UtilitiesContext context;
        private static string lastKey = "";
        private IMemoryCache cache;
        private string myKey;
        public TenantService(UtilitiesContext context, IMemoryCache memoryCache)
        {
            this.context = context;
            cache = memoryCache;
        }

        public TenantsViewModel GetTenants(string name, string surname, string patronymic, int page, SortState sortOrder, string cacheKey)
        {
            TenantsViewModel tenants = null;
            myKey = myKey + name + surname + patronymic + page + sortOrder;
            //if (cacheKey != "TenantsCache")
            //{
                if (lastKey != myKey)
                {
                    cache.Remove("TenantsCache");
                    cacheKey = "TenantsCache";
                }
                //else cacheKey = "NoCache";
            //}
            lastKey = myKey;
            if (!cache.TryGetValue(cacheKey, out tenants))
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
                tenants = new TenantsViewModel
                {
                    Tenants = items,
                    PageViewModel = pageViewModel,
                    SortViewModel = new SortViewModel(sortOrder),
                    FilterViewModel = new FilterViewModel(name, surname, patronymic),
                };
                if (tenants != null)
                {
                    cache.Set("TenantsCache", tenants,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return tenants;
        }
    }
}
