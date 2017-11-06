using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TYRISKANALYSIS.DataLayer
{
    public class Typhoon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateHit { get; set; }
        public int SignalNo { get; set; }
        public int WindKPH { get; set; }
        public decimal HoursLasted { get; set; }
    }
}
