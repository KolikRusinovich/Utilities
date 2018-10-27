using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Models;

namespace Utilities.ViewModels
{
    public class PaymentsViewModel
    {
        public IEnumerable<Payment> Payments { get; set; }
        public PaymentViewModel PaymentViewModel { get; set; }
    }
}
