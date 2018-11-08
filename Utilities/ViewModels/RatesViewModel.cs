using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Models;

namespace Utilities.ViewModels
{
    public class RatesViewModel
    {
        public IEnumerable<Rate> Rates { get; set; }
        public Rate Rate { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
