using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Models;

namespace Utilities.ViewModels.ReadingsViewModels
{
    public class ReadingSortViewModel
    {
        public SortState ReadingIdSort { get; private set; }
        public SortState SurnameSort { get; private set; }
        public SortState ApartmentNumberSort { get; private set; }
        public SortState TypeSort { get; private set; }
        public SortState CounterNumberSort{ get; private set; }
        public SortState IndicationsSort { get; private set; }
        public SortState DateSort { get; private set; }
        public SortState Current { get; private set; }

        public ReadingSortViewModel(SortState sortOrder)
        {
            ReadingIdSort = sortOrder == SortState.ReadingIdAsc ? SortState.ReadingIdDesc : SortState.ReadingIdAsc;
            SurnameSort = sortOrder == SortState.SurameOfTenantAsc ? SortState.SurnameOfTenantDesc : SortState.SurameOfTenantAsc;
            ApartmentNumberSort = sortOrder == SortState.ApartmentNumberAsc ? SortState.ApartmentNumberDesc : SortState.ApartmentNumberAsc;
            TypeSort = sortOrder == SortState.TypeOfRateAsc ? SortState.TypeOfRateDesc : SortState.TypeOfRateAsc;
            CounterNumberSort = sortOrder == SortState.CounterNumberAsc ? SortState.CounterNumberDesc : SortState.CounterNumberAsc;           
            IndicationsSort = sortOrder == SortState.IndicationsAsc ? SortState.IndicationsDesc : SortState.IndicationsAsc;
            DateSort = sortOrder == SortState.DateOfReadingAsc ? SortState.DateOfReadingDesc : SortState.DateOfReadingAsc;
            Current = sortOrder;
        }
    }
}
