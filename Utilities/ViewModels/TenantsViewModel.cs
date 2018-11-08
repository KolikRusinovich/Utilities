using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Models;

namespace Utilities.ViewModels
{
    public class TenantsViewModel
    {
        public IEnumerable<Tenant> Tenants { get; set; }
        public Tenant Tenant { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
