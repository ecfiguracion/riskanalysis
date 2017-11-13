using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TYRISKANALYSIS.DataLayer;

namespace TYRISKANALYSIS.Models
{
    public class AssessmentLookupsModel
    {
        public IEnumerable<LookUp.DataLookUpModel> Typhoons { get; set; }
        public IEnumerable<LookUp.DataLookUpModel> Barangays { get; set; }
        public IEnumerable<LookUp.DataLookUpModel> PopulationLookup { get; set; }
        public IEnumerable<LookUp.DataLookUpModel> StructuresLookup { get; set; }
        public IEnumerable<LookUp.DataLookUpModel> TransportationLookup { get; set; }
        public IEnumerable<LookUp.DataLookUpModel> CommunicationLookup { get; set; }
        public IEnumerable<LookUp.DataLookUpModel> ElectricalPowerLookup { get; set; }
        public IEnumerable<LookUp.DataLookUpModel> WaterFacilitiesLookup { get; set; }
    }
}
