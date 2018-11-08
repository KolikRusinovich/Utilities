using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Models;

namespace Utilities.ViewModels
{
    public class SortViewModel
    {
        public SortState TenantIdSort { get; private set; }
        public SortState NameSort{ get; private set; } 
        public SortState SurnameSort { get; private set; } 
        public SortState PatronymicSort { get; private set; }
        public SortState ApartmentNumberSort { get; private set; }
        public SortState NumberOfPeopleSort { get; private set; }
        public SortState TotalAreaSort { get; private set; }
        public SortState Current { get; private set; } 

        public SortViewModel(SortState sortOrder)
        {
            TenantIdSort = sortOrder == SortState.TenantIdAsc ? SortState.TenantIdDesc : SortState.TenantIdAsc;
            NameSort = sortOrder == SortState.NameOfTenantAsc ? SortState.NameOfTenantDesc : SortState.NameOfTenantAsc;
            SurnameSort = sortOrder == SortState.SurameOfTenantAsc ? SortState.SurnameOfTenantDesc : SortState.SurameOfTenantAsc;
            PatronymicSort = sortOrder == SortState.PatronymicOfTenantAsc ? SortState.PatronymicOfTenantDesc : SortState.PatronymicOfTenantAsc;
            ApartmentNumberSort = sortOrder == SortState.ApartmentNumberAsc ? SortState.ApartmentNumberDesc : SortState.ApartmentNumberAsc;
            NumberOfPeopleSort = sortOrder == SortState.NumberOfPeopleAsc ? SortState.NumberOfPeopleDesc : SortState.NumberOfPeopleAsc;
            TotalAreaSort = sortOrder == SortState.TotalAreaAsc ? SortState.TotalAreaDesc : SortState.TotalAreaAsc;
            Current = sortOrder;
        }
    }
}
