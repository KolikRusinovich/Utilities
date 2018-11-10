using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Models;

namespace Utilities.ViewModels.RatesViewModels
{
    public class RatesSortViewModel
    {
        public SortState RateIdSort { get; private set; }
        public SortState TypeSort { get; private set; }
        public SortState ValueSort { get; private set; }
        public SortState DateOfIntroductionSort { get; private set; }
        public SortState Current { get; private set; }

        public RatesSortViewModel(SortState sortOrder)
        {
            RateIdSort = sortOrder == SortState.RateIdAsc ? SortState.RateIdDesc : SortState.RateIdAsc;
            TypeSort = sortOrder == SortState.TypeOfRateAsc ? SortState.TypeOfRateDesc : SortState.TypeOfRateAsc;
            ValueSort = sortOrder == SortState.ValueAsc ? SortState.ValueDesc : SortState.ValueAsc;
            DateOfIntroductionSort = sortOrder == SortState.DateOfRateAsc ? SortState.DateOfRateDesc : SortState.DateOfRateAsc;
            Current = sortOrder;
        }
    }
}
