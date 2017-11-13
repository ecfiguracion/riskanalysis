using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TYRISKANALYSIS.Models.LookUp;

namespace TYRISKANALYSIS.DataLayer.Model
{
    public class AssessmentModel
    {
        public int Id { get; set; }
        public DataLookUpModel Typhoon { get; set; }
        public string Remarks { get; set; }

        public IEnumerable<AssessmentPopulationModel> Population { get; set; }
        public IEnumerable<AssessmentPropertiesModel> Properties { get; set; }
        public IEnumerable<AssessmentTransportationModel> Transportation { get; set; }
        public IEnumerable<AssessmentCommunicationModel> Communication { get; set; }
        public IEnumerable<AssessmentElectricalModel> ElectricalPower { get; set; }
        public IEnumerable<AssessmentWaterFacilitiesModel> WaterFacilities { get; set; }
    }
}
