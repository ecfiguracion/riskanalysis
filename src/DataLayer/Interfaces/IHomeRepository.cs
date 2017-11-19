using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TYRISKANALYSIS.Controllers.API.Utilities;
using TYRISKANALYSIS.DataLayer.Model;
using TYRISKANALYSIS.Models.LookUp;

namespace TYRISKANALYSIS.DataLayer.Interfaces
{
    public interface IHomeRepository
    {
        HomeLookupModel GetLookUps();
        RiskMapsModel GetRiskMaps(int id);
        IEnumerable<ChartDataModel> GetCharts(int id);
        IEnumerable<PopulationCommonModel> GetDisplacedEvacuated(int id);
        IEnumerable<PopulationCommonModel> GetCasualties(int id);
        IEnumerable<DamagedPropertiesModel> GetDamagedProperties(int id);
        IEnumerable<TransportationModel> GetTransportation(int id);
        IEnumerable<LifelinesCommonModel> GetCommunication(int id);
        IEnumerable<LifelinesCommonModel> GetElectrical(int id);
        IEnumerable<LifelinesCommonModel> GetWaterFacilities(int id);
        IEnumerable<AgricultureCommonModel> GetCrops(int id);
        IEnumerable<AgricultureCommonModel> GetFisheries(int id);
        IEnumerable<LivestockModel> GetLivestocks(int id);
    }
}
