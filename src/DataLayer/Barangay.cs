using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TYRISKANALYSIS.DataLayer
{
    public class Barangay
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Latitude { get; set; }
        public int? Longitude { get; set; }
    }
}
