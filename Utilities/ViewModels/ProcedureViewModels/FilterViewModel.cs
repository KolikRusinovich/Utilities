using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Models;

namespace Utilities.ViewModels.ProcedureViewModels
{
    public class FilterViewModel
    {
        public SelectList Rates { get; private set; }
        public SelectList Tenants { get; private set; }
        public SelectList Objects { get; private set; }
        public SelectList Readings { get; private set; }
        public int? SelectedRate { get; private set; }
        public int? SelectedTenant { get; private set; }

        public string FirstDate { get; private set; }
        public string SecondDate { get; private set; }

        public FilterViewModel(List<Rate> rates, List<Tenant> tenants, int? rate, int? tenant)
        {
            tenants.Insert(0, new Tenant { TenantId = 0, Surname = "Все" });
            rates.Insert(0, new Rate { Type = "Все", RateId = 0 });
            Rates = new SelectList(rates, "RateId", "Type", rate);
            Tenants = new SelectList(tenants, "TenantId", "Surname", tenant);
            SelectedRate = rate;
            SelectedTenant = tenant;
        }

        public FilterViewModel(List<object[]> objects, string d0, string d)
        {
            objects.Insert(0, new object[] { "", "" });
            Objects = new SelectList(objects);
            FirstDate = d0;
            SecondDate = d;
        }
    }
}
