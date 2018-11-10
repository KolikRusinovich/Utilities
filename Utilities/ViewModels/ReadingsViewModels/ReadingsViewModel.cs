using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Models;
using Utilities.ViewModels.ReadingsViewModels;

namespace Utilities.ViewModels
{
    public class ReadingsViewModel
    {
        public IEnumerable<Reading> Readings { get; set; }
        public ReadingViewModel ReadingViewModel { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public SelectList TenantsList { get; set; }
        public SelectList RatesList { get; set; }
        public ReadingSortViewModel SortViewModel { get; set; }
        public ReadingsFilterViewModel FilterViewModel { get; set; }
    }
}
