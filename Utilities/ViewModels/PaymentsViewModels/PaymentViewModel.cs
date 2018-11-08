using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Utilities.ViewModels
{
    public class PaymentViewModel
    {
        [Display(Name = "Код платежа")]
        public int PaymentId { get; set; }

        [Display(Name = "Фамилия квартиросъёмщика")]
        public string Surname { get; set; }

        [Display(Name = "Тип счётчика")]

        public string Type { get; set; }

        [Display(Name = "Сумма")]
        public int Sum { get; set; }

        [Display(Name = "Дата платежа")]
        [DataType(DataType.Date)]
        public DateTime DateOfPayment { get; set; }
    }
}
