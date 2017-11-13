using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TYRISKANALYSIS.Models.LookUp;

namespace TYRISKANALYSIS.DataLayer.Model
{
    public class AssessmentTransportationModel
    {
        public int id { get; set; }
        public int assessmentId { get; set; }
        public string description { get; set; }
        public DataLookUpModel facility { get; set; }
        public DataLookUpModel barangay { get; set; }
        public bool isPassable { get; set; }
        public int lengthKM { get; set; }
        public int estimatedCost { get; set; }
        public bool isdeleted { get; set; }
    }
}
