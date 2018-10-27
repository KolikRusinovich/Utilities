using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Models;

namespace Utilities.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Tenant> Tenants { get; set; }
        public IEnumerable<Rate> Rates { get; set; }
        public IEnumerable<ReadingViewModel> Readings { get; set; }
    }
}
