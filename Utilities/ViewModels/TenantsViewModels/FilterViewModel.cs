using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Models;

namespace Utilities.ViewModels
{
    public class FilterViewModel
    {
        public FilterViewModel(string name,string surname, string patronymic)
        {
            SelectedName = name;
            SelectedSurname = surname;
            SelectedPatronymic = patronymic;
        }
        public string SelectedName { get; private set; }
        public string SelectedSurname { get; private set; }
        public string SelectedPatronymic { get; private set; }
    }
}
