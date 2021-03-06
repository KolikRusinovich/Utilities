﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Models;
using Utilities.ViewModels.PaymentsViewModels;

namespace Utilities.ViewModels
{
    public class PaymentsViewModel
    {
        public IEnumerable<Payment> Payments { get; set; }
        public PaymentViewModel PaymentViewModel { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public SelectList TenantsList { get; set; }
        public SelectList RatesList { get; set; }
        public PaymentsFilterViewModel FilterViewModel { get; set; }
        public PaymentsSortViewModel SortViewModel { get; set; }
    }
}
