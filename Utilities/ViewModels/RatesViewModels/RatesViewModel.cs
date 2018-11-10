using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Models;
using Utilities.ViewModels.RatesViewModels;

namespace Utilities.ViewModels
{
    public class RatesViewModel
    {
        public IEnumerable<Rate> Rates { get; set; }
        public Rate Rate { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public RatesSortViewModel SortViewModel { get; set; }
        public RatesFilterViewModel FilterViewModel { get; set; }
    }
}
