using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Utilities.Models
{
    public class Tenant
    {
        [Key]
        [Display(Name = "Код квартиросъёмщика")]
        public int TenantId { get; set; }
        [Display(Name = "Имя")]
        public string NameOfTenant { get; set; }
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }
        [Display(Name = "Номер квартиры")]
        public int ApartmentNumber { get; set; }
        [Display(Name = "Количество людей")]
        public int NumberOfPeople { get; set; }
        [Display(Name = "Общая площадь")]
        public int TotalArea { get; set; }
        public virtual ICollection<Reading> Readings { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }

        public Tenant()
        {
            Readings = new List<Reading>();
            Payments = new List<Payment>();
        }
    }
}
