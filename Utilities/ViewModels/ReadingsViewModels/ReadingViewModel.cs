using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Utilities.ViewModels
{
    public class ReadingViewModel
    {
        [Display(Name = "Код показания")]
        public int ReadingId { get; set; }

        [Display(Name = "Фамилия квартиросъёмщика")]
        public string Surname { get; set; }

        [Display(Name = "Номер квартиры")]
        [Required(ErrorMessage = "Не указан номер квартиры")]
        public int Apartmentnumber { get; set; }

        [Display(Name = "Тип счётчика")]

        public string Type { get; set; }

        [Display(Name = "Номер счётчика")]
        [Required(ErrorMessage = "Не указан номер  счетчика")]
        public int CounterNumber { get; set; }

		
        [Display(Name = "Показания")]
        [Required(ErrorMessage = "Не указаны показания")]
        public int Indications { get; set; }

        [Display(Name = "Дата снятия показаний")]
        [DataType(DataType.Date)]
        public DateTime DateOfReading { get; set; }

    }
}
