using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TYRISKANALYSIS.Models.LookUp;

namespace TYRISKANALYSIS.DataLayer.Model
{
    public class HomeLookupModel
    {
        public IEnumerable<DataLookUpModel> Typhoons { get; set; }
        public IEnumerable<DataLookUpModel> Sections { get; set; }
        public IEnumerable<DataLookUpLinkModel> Categories { get; set; }
    }

    public class RiskMapsModel
    {
        public SectionSummaryModel Summary { get; set; }
        public IEnumerable<RiskMapDataModel> Maps { get; set; }
    }

    public class ChartDataModel
    {
        public int SectionId { get; set; }
        public string Name { get; set; }
        public string Total { get; set; }
    }

    public class PopulationCommonModel
    {
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string Barangay { get; set; }
        public int Total { get; set; }
    }

    public class DamagedPropertiesModel
    {
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string Barangay { get; set; }
        public int TotallyDamaged { get; set; }
        public string TotallyDamagedUnit { get; set; }
        public int CriticallyDamaged { get; set; }
        public string CriticallyDamagedUnit { get; set; }
        public int EstimatedCost { get; set; }
    }

    public class TransportationModel
    {
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string Description { get; set; }
        public string Barangay { get; set; }
        public bool IsPassable { get; set; }
        public int LengthKM { get; set; }
        public int EstimatedCost { get; set; }
    }

    public class LifelinesCommonModel
    {
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string Barangay { get; set; }
        public bool IsOperational { get; set; }
        public int Total { get; set; }
        public int EstimatedCost { get; set; }
    }

    public class AgricultureCommonModel
    {
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string Barangay { get; set; }
        public int AreaDamaged { get; set; }
        public int MetricTons { get; set; }
        public int EstimatedCost { get; set; }
    }

    public class LivestockModel
    {
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string Barangay { get; set; }
        public int Total { get; set; }
        public int EstimatedCost { get; set; }
    }

    public class HomeModel
    {
        public SectionSummaryModel RiskMapSummary { get; set; }
        public IEnumerable<RiskMapDataModel> RiskMaps { get; set; }
        public IEnumerable<ChartDataModel> Charts { get; set; }
        public IEnumerable<PopulationCommonModel> DisplacedEvacuated { get; set; }
        public IEnumerable<PopulationCommonModel> Casualties { get; set; }
        public IEnumerable<DamagedPropertiesModel> DamagedProperties { get; set; }
        public IEnumerable<TransportationModel> Transportation { get; set; }
        public IEnumerable<LifelinesCommonModel> Communication { get; set; }
        public IEnumerable<LifelinesCommonModel> Electrical { get; set; }
        public IEnumerable<LifelinesCommonModel> WaterFacilities { get; set; }
        public IEnumerable<AgricultureCommonModel> Crops { get; set; }
        public IEnumerable<AgricultureCommonModel> Fisheries { get; set; }
        public IEnumerable<LivestockModel> Livestocks { get; set; }


    }
}
