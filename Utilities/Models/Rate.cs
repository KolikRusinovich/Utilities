using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Utilities.Models
{
    public class Rate
    {
        [Key]
        [Display(Name = "Код показания")]
        public int RateId { get; set; }
        [Display(Name = "Тип счётчика")]
        public string Type { get; set; }
        [Display(Name = "Значение")]
        public int Value { get; set; }
        [Display(Name = "Дата ввода")]
        [DataType(DataType.Date)]
        public DateTime DateOfIntroduction { get; set; }
        public virtual ICollection<Reading> Readings { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }

        public Rate()
        {
            Readings = new List<Reading>();
            Payments = new List<Payment>();
        }
    }
}
