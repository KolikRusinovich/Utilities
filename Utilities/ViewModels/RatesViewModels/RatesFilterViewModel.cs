using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utilities.ViewModels.RatesViewModels
{
    public class RatesFilterViewModel
    {
        public RatesFilterViewModel(string type, DateTime firstDate, DateTime secondDate)
        {
            SelectedType = type;
            SelectedFirstDate = firstDate;
            SelectedSecondDate = secondDate;
        }
        public string SelectedType { get; private set; }
        public DateTime SelectedFirstDate { get; private set; }
        public DateTime SelectedSecondDate { get; private set; }
    }
}
