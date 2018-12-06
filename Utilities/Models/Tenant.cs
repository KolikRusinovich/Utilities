using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Utilities.ViewModels;

namespace Utilities.Models
{
    public class Tenant
    {
        [Key]
        [Display(Name = "Код квартиросъёмщика")]
        public int TenantId { get; set; }
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Не указано имя")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        public string NameOfTenant { get; set; }
        [Required(ErrorMessage = "Не указано фамилия")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Не указано Отчество")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Длина строки должна быть от 6 до 50 символов")]
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }
        [Display(Name = "Номер квартиры")]
        [Required(ErrorMessage = "Не указано номер квартиры")]
        public int ApartmentNumber { get; set; }
        [Display(Name = "Количество людей")]
        [Required(ErrorMessage = "Не указано количество людей")]
        [Range(1, 8, ErrorMessage = "Недопустимое количество людей(Введите от 1 до 8)")]
        public int NumberOfPeople { get; set; }
        [Required(ErrorMessage = "Не указана площадь")]
        [Range(30, 200, ErrorMessage = "Недопустимая площадь(введите от 30 до 200)")]
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
