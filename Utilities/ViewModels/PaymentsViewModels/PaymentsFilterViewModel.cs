using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Models;

namespace Utilities.ViewModels.PaymentsViewModels
{
    public class PaymentsFilterViewModel
    {
        public PaymentsFilterViewModel(List<Tenant> tenants, List<Rate> rates, int? tenant, int? rate, DateTime firstDate, DateTime secondDate)
        {
            tenants.Insert(0, new Tenant { TenantId = 0, Surname = "Все" });
            rates.Insert(0, new Rate { RateId = 0, Type = "Все" });
            Tenants = new SelectList(tenants, "TenantId", "Surname", tenant);
            Rates = new SelectList(rates, "RateId", "Type", rate);
            SelectedFirstDate = firstDate;
            SelectedSecondDate = secondDate;
        }
        public SelectList Tenants { get; private set; }
        public SelectList Rates { get; private set; }
        public int? SelectedTenant { get; private set; }
        public int? SelectedRate { get; private set; }
        public DateTime SelectedFirstDate { get; private set; }
        public DateTime SelectedSecondDate { get; private set; }
    }
}
