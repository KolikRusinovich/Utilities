using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Models;
using Utilities.ViewModels;
using Utilities.ViewModels.RatesViewModels;

namespace Utilities.Services
{
    public class RateService
    {
        private UtilitiesContext context;
        private static string lastKey = "";
        private IMemoryCache cache;
        private string myKey;
        public RateService(UtilitiesContext context, IMemoryCache memoryCache)
        {
            this.context = context;
            cache = memoryCache;
        }

        public RatesViewModel GetRates(string type, string firstDate, string secondDate , int page, SortState sortOrder, string cacheKey)
        {
            RatesViewModel rates = null;
            myKey = type + firstDate + secondDate + page + sortOrder;
            /*if (cacheKey != "RateCache")
            {*/
                if (lastKey != myKey)
                {
                    cache.Remove("RateCache");
                    cacheKey = "RateCache";
                }
                //else cacheKey = "NoCache";
            //}
            lastKey = myKey;
            if (!cache.TryGetValue(cacheKey, out rates))
            {
                DateTime first, second;
                first = DateTime.Parse(firstDate);
                second = DateTime.Parse(secondDate);
                int pageSize = 10;
                IQueryable<Rate> source = context.Rates;
                if (!String.IsNullOrEmpty(type))
                {
                    source = source.Where(p => p.Type.Contains(type));
                }
                if (firstDate != null || secondDate != null)
                {
                    source = source.Where(p => p.DateOfIntroduction >= first && p.DateOfIntroduction <= second);
                }
                switch (sortOrder)
                {
                    case SortState.RateIdDesc:
                        source = source.OrderByDescending(s => s.RateId);
                        break;
                    case SortState.TypeOfRateAsc:
                        source = source.OrderBy(s => s.Type);
                        break;
                    case SortState.TypeOfRateDesc:
                        source = source.OrderByDescending(s => s.Type);
                        break;
                    case SortState.ValueAsc:
                        source = source.OrderBy(s => s.Value);
                        break;
                    case SortState.ValueDesc:
                        source = source.OrderByDescending(s => s.Value);
                        break;
                    case SortState.DateOfRateAsc:
                        source = source.OrderBy(s => s.DateOfIntroduction);
                        break;
                    case SortState.DateOfRateDesc:
                        source = source.OrderByDescending(s => s.DateOfIntroduction);
                        break;
                    default:
                        source = source.OrderBy(s => s.RateId);
                        break;
                }
                var count = source.Count();
                var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
                rates = new RatesViewModel
                {
                    Rates = items,
                    PageViewModel = pageViewModel,
                    SortViewModel = new RatesSortViewModel(sortOrder),
                    FilterViewModel = new RatesFilterViewModel(type, first, second)
                };
                if (rates != null)
                {
                    cache.Set("RateCache", rates,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return rates;
        }
    }
}
