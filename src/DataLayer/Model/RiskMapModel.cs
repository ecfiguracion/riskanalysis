using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TYRISKANALYSIS.DataLayer.Model
{
    public class RiskMapDBModel
    {
        public int BarangayId { get; set; }
        public int SectionId { get; set; }
        public string Category { get; set; }
        public string Barangay { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Total { get; set; }
    }

    public class RiskMapDataModel
    {
        public int SectionId { get; set; }
        public string Barangay { get; set; }
        public string Summary { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
