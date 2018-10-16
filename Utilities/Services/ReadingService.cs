using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Utilties.Models;
using Microsoft.Extensions.Configuration;
using Utilties.ViewModels;

namespace Utilities.Services
{
    public class ReadingService
    {
        private UtilitiesContext _context;

        public ReadingService(UtilitiesContext context)
        {
            _context = context;
        }

        public HomeViewModel GetHomeViewModel()
        {
            var tenants = _context.Tenants.Take(10).ToList();
            var rates = _context.Rates.Take(10).ToList();
            List<ReadingViewModel> readings = _context.Readings
                .Select(t => new ReadingViewModel { ReadingId = t.ReadingId, Surname = t.Tenant.Surname, Apartmentnumber = t.Apartmentnumber, Type = t.Rate.Type, CounterNumber = t.CounterNumber, Indications = t.Indications, DateOfReading = t.DateOfReading })
                .Take(10)
                .ToList();
            HomeViewModel homeViewModel = new HomeViewModel { Tenants = tenants, Rates = rates, Readings = readings };
            return homeViewModel;
        }
    }
}
