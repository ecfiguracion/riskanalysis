using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TYRISKANALYSIS.Models.LookUp;

namespace TYRISKANALYSIS.DataLayer.Model
{
    public class AssessmentCropsModel
    {
        public int id { get; set; }
        public int assessmentId { get; set; }
        public DataLookUpModel barangay { get; set; }
        public DataLookUpModel crops { get; set; }
        public int areaDamaged { get; set; }
        public int metricTons { get; set; }
        public int estimatedCost { get; set; }
        public bool isdeleted { get; set; }
    }
}
