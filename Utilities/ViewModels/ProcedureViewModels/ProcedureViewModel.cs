using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.ViewModels.ProcedureViewModels;

namespace Utilities.ViewModels
{
    public class ProcedureViewModel
    {
        public IEnumerable<object[]> Objects { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
    }
}
