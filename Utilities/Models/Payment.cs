using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Utilities.Models
{
    public class Payment
    {
        [Key]
        [Display(Name = "Код платежа")]
        public int PaymentId { get; set; }
        [Display(Name = "Код квартиросъёмщика")]
        public int TenantId { get; set; }
        [Display(Name = "Код счётчика")]
        public int RateId { get; set; }
        [Required(ErrorMessage = "Не указана сумма")]
        [Display(Name = "Сумма")]
        public int Sum { get; set; }
        [Display(Name = "Дата платежа")]
        [DataType(DataType.Date)]
        public DateTime DateOfPayment { get; set; }
        public Tenant Tenant { get; set; }
        public Rate Rate { get; set; }
    }
}
