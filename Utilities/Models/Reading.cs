using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Utilities.Models
{
    public class Reading
    {
        [Key]
        [Display(Name = "Код показания")]
        public int ReadingId { get; set; }
        [Display(Name = "Фамилия квартиросъёмщика")]
        public int TenantId { get; set; }
        [Display(Name = "Номер квартиры")]
        [Required(ErrorMessage = "Не указан номер квартиры")]
        public int Apartmentnumber { get; set; }
        [Display(Name = "Название счётчика")]
        public int RateId { get; set; }
        [Display(Name = "Номер счётчика")]
        [Required(ErrorMessage = "Не указано номер счетчика")]
        public int CounterNumber { get; set; }
        [Display(Name = "Показания")]
        [Required(ErrorMessage = "Не указано показания")]
        public int Indications { get; set; }
        [Display(Name = "Дата")]
        [DataType(DataType.Date)]
        public DateTime DateOfReading { get; set; }
        public virtual Rate Rate { get; set; }
        public virtual Tenant Tenant { get; set; }
    }
}
