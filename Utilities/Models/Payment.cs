using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utilties.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int TenantId { get; set; }
        public int RateId { get; set; }
        public int Sum { get; set; }
        public DateTime DateOfPayment { get; set; }
        public Tenant Tenant { get; set; }
        public Rate Rate { get; set; }
    }
}
