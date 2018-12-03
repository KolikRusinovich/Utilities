using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Models;
using Utilities.ViewModels;
using Utilities.ViewModels.PaymentsViewModels;

namespace Utilities.Services
{
    public class PaymentService
    {
        private UtilitiesContext context;
        private static string lastKey = "";
        private IMemoryCache cache;
        private string myKey;
        public PaymentService(UtilitiesContext context, IMemoryCache memoryCache)
        {
            this.context = context;
            cache = memoryCache;
        }

        public PaymentsViewModel GetPayments(int? tenant, int? rate, string firstDate, string secondDate, int page, SortState sortOrder, string cacheKey)
        {
            PaymentsViewModel payments = null;
            myKey = myKey + tenant + rate + firstDate + secondDate + page + sortOrder;
            /* if (cacheKey != "ReadingsCache")
             {*/
            if (lastKey != myKey)
            {
                cache.Remove("PaymentsCache");
                cacheKey = "PaymentsCache";
            }
            // else cacheKey = "NoCache";
            //}
            lastKey = myKey;
            if (!cache.TryGetValue(cacheKey, out payments))
            {
                PaymentViewModel _payment = new PaymentViewModel
                {
                    Type = "",
                    Surname = ""
                };
                DateTime first = Convert.ToDateTime(firstDate);
                DateTime second = Convert.ToDateTime(secondDate);
                int pageSize = 10;
                IQueryable<Payment> source = context.Payments.Include(p => p.Tenant).Include(p => p.Rate);
                if (tenant != null && tenant != 0)
                    source = source.Where(p => p.TenantId == tenant);
                if (rate != null && rate != 0)
                    source = source.Where(p => p.RateId == rate);
                if (firstDate != null || secondDate != null)
                {
                    source = source.Where(p => p.DateOfPayment >= first && p.DateOfPayment <= second);
                }
                switch (sortOrder)
                {
                    case SortState.PaymentsIdDesc:
                        source = source.OrderByDescending(s => s.PaymentId);
                        break;
                    case SortState.SurameOfTenantAsc:
                        source = source.OrderBy(s => s.Tenant.Surname);
                        break;
                    case SortState.SurnameOfTenantDesc:
                        source = source.OrderByDescending(s => s.Tenant.Surname);
                        break;
                    case SortState.TypeOfRateAsc:
                        source = source.OrderBy(s => s.Rate.Type);
                        break;
                    case SortState.TypeOfRateDesc:
                        source = source.OrderByDescending(s => s.Rate.Type);
                        break;
                    case SortState.PaymentsSumAsc:
                        source = source.OrderBy(s => s.Sum);
                        break;
                    case SortState.PaymentsSumDesc:
                        source = source.OrderByDescending(s => s.Sum);
                        break;
                    case SortState.DateOfPaymentsAsc:
                        source = source.OrderBy(s => s.DateOfPayment);
                        break;
                    case SortState.DateOfPaymentsDesc:
                        source = source.OrderByDescending(s => s.DateOfPayment);
                        break;
                    default:
                        source = source.OrderBy(s => s.PaymentId);
                        break;
                }
                var count = source.Count();
                var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
                payments = new PaymentsViewModel
                {
                    Payments = items,
                    PaymentViewModel = _payment,
                    PageViewModel = pageViewModel,
                    SortViewModel = new PaymentsSortViewModel(sortOrder),
                    FilterViewModel = new PaymentsFilterViewModel(context.Tenants.ToList(), context.Rates.ToList(), tenant, rate, first, second)
                };
                if (payments != null)
                {
                    cache.Set("PaymentsCache", payments,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return payments;
        }
    }
}
