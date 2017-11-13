using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TYRISKANALYSIS.Models.LookUp;

namespace TYRISKANALYSIS.DataLayer.Model
{
    public class AssessmentCommunicationModel
    {
        public int id { get; set; }
        public int assessmentId { get; set; }
        public DataLookUpModel facility { get; set; }
        public DataLookUpModel barangay { get; set; }
        public bool isOperational { get; set; }
        public int total { get; set; }
        public int estimatedCost { get; set; }
        public bool isdeleted { get; set; }
    }
}
