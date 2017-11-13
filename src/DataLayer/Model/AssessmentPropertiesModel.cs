using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TYRISKANALYSIS.Models.LookUp;

namespace TYRISKANALYSIS.DataLayer.Model
{
    public class AssessmentPropertiesModel
    {
        public int id { get; set; }
        public int assessmentId { get; set; }
        public DataLookUpModel structure { get; set; }
        public DataLookUpModel barangay { get; set; }
        public bool isTotallyDamaged { get; set; }
        public bool isCriticallyDamaged { get; set; }
        public int estimatedCost { get; set; }
        public bool isdeleted { get; set; }
    }
}
