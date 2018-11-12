using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Models;
using Utilities.ViewModels;
using Utilities.ViewModels.ReadingsViewModels;

namespace Utilities.Services
{
    public class ReadingService
    {
        private UtilitiesContext context;
        private static string lastKey = "";
        private IMemoryCache cache;
        private string myKey;
        public ReadingService(UtilitiesContext context, IMemoryCache memoryCache)
        {
            this.context = context;
            cache = memoryCache;
        }

        public ReadingsViewModel GetReadings(int? tenant, int? rate, string firstDate, string secondDate, int page, SortState sortOrder, string cacheKey)
        {
            ReadingsViewModel readings = null;
            myKey = myKey + tenant + rate + firstDate + secondDate + page + sortOrder;
            if (cacheKey != "Cache")
            {
                if (lastKey != myKey)
                {
                    cache.Remove("Cache");
                    cacheKey = "Cache";
                }
                else cacheKey = "NoCache";
            }
            lastKey = myKey;
            if (!cache.TryGetValue(cacheKey, out readings))
            {
                ReadingViewModel _reading = new ReadingViewModel
                {
                    Type = "",
                    Surname = ""
                };
                DateTime first = Convert.ToDateTime(firstDate);
                DateTime second = Convert.ToDateTime(secondDate);
                int pageSize = 10;
                IQueryable<Reading> source = context.Readings.Include(p => p.Tenant).Include(p => p.Rate);
                if (tenant != null && tenant != 0)
                    source = source.Where(p => p.TenantId == tenant);
                if (rate != null && rate != 0)
                    source = source.Where(p => p.RateId == rate);
                if (firstDate != null || secondDate != null)
                {
                    source = source.Where(p => p.DateOfReading >= first && p.DateOfReading <= second);
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
                readings = new ReadingsViewModel
                {
                    Readings = items,
                    ReadingViewModel = _reading,
                    PageViewModel = pageViewModel,
                    SortViewModel = new ReadingSortViewModel(sortOrder),
                    FilterViewModel = new ReadingsFilterViewModel(context.Tenants.ToList(), context.Rates.ToList(), tenant, rate, first, second)
                };
                if (readings != null)
                {
                    cache.Set("Cache", readings,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return readings;
        }
    }
}
