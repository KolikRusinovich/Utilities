using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utilties.Models
{
    public class Reading
    {
        public int ReadingId { get; set; }
        public int TenantId { get; set; }
        public int Apartmentnumber { get; set; }
        public int RateId { get; set; }
        public int CounterNumber { get; set; }
        public int Indications { get; set; }
        public DateTime DateOfReading { get; set; }
        public Rate Rate { get; set; }
        public Tenant Tenant { get; set; }
    }
}
