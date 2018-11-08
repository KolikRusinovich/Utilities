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
        public FilterViewModel(List<Tenant> tenants, string name,string surname, string patronymic)
        {
            // устанавливаем начальный элемент, который позволит выбрать всех
            tenants.Insert(0, new Tenant { NameOfTenant = "Все", Surname = "Все", Patronymic = "Все", TenantId = 0 });
            SelectedName = name;
            SelectedSurname = surname;
            SelectedPatronymic = patronymic;
        }
        public string SelectedName { get; private set; }
        public string SelectedSurname { get; private set; }
        public string SelectedPatronymic { get; private set; }
    }
}
