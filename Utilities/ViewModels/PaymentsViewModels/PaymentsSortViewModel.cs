using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Models;

namespace Utilities.ViewModels.PaymentsViewModels
{
    public class PaymentsSortViewModel
    {
        public SortState PaymentsIdSort { get; private set; }
        public SortState SurnameSort { get; private set; }
        public SortState TypeSort { get; private set; }
        public SortState SumSort { get; private set; }
        public SortState DateSort { get; private set; }
        public SortState Current { get; private set; }

        public PaymentsSortViewModel(SortState sortOrder)
        {
            PaymentsIdSort = sortOrder == SortState.PaymentsIdAsc ? SortState.PaymentsIdDesc : SortState.PaymentsIdAsc;
            SurnameSort = sortOrder == SortState.SurameOfTenantAsc ? SortState.SurnameOfTenantDesc : SortState.SurameOfTenantAsc;
            TypeSort = sortOrder == SortState.TypeOfRateAsc ? SortState.TypeOfRateDesc : SortState.TypeOfRateAsc;
            SumSort = sortOrder == SortState.PaymentsSumAsc ? SortState.PaymentsSumDesc : SortState.PaymentsSumAsc;
            DateSort = sortOrder == SortState.DateOfPaymentsAsc ? SortState.DateOfPaymentsDesc : SortState.DateOfPaymentsAsc;
            Current = sortOrder;
        }
    }
}
