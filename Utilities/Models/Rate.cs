using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utilties.Models
{
    public class Rate
    {
        public int RateId { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }
        public DateTime DateOfIntroduction { get; set; }
        public virtual ICollection<Reading> Readings { get; set; }

        public Rate()
        {
            Readings = new List<Reading>();
        }
    }
}
