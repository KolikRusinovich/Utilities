﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Models;

namespace Utilities.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Reading> Readings { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public ReadingViewModel ReadingViewModel { get; set; }
    }
}
