using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utilties.Models
{
    public class Tenant
    {
        public int TenantId { get; set; }
        public string NameOfTenant { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public int ApartmentNumber { get; set; }
        public int NumberOfPeople { get; set; }
        public int TotalArea { get; set; }
        public virtual ICollection<Reading> Readings { get; set; }

        public Tenant()
        {
            Readings = new List<Reading>();
        }
    }
}
