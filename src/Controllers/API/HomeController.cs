using Microsoft.AspNetCore.Mvc;
using TYRISKANALYSIS.Controllers.API.Utilities;
using Microsoft.Extensions.Configuration;
using TYRISKANALYSIS.DataLayer.Interfaces;
using TYRISKANALYSIS.DataLayer.Repository;
using TYRISKANALYSIS.DataLayer;
using TYRISKANALYSIS.DataLayer.Model;

namespace TYRISKANALYSIS.Controllers.API
{
    [Produces("application/json")]
    [Route("api/home")]
    public class HomeController : Controller
    {
        private IHomeRepository repository;

        public HomeController(IConfiguration appconfig)
        {
            repository = new HomeRepository(appconfig);
        }

        // GET api/home/datalookups
        [HttpGet("datalookups")]
        public IActionResult GetDataLookups()
        {
            return Ok(repository.GetLookUps());
        }

        [HttpGet("risktrends")]
        public IActionResult GetDataLookups(int section, int category, int support)
        {
            return Ok(repository.GetTrends(section,category,support));
        }

        // GET api/home/{id}
        [HttpGet("{id}")]
        public IActionResult GetTyphoons(int id)
        {
            var homeModel = new HomeModel();

            var riskdata = repository.GetRiskMaps(id);

            // Risk Maps
            homeModel.RiskMapSummary = riskdata.Summary;
            homeModel.RiskMaps = riskdata.Maps;

            // Charts
            homeModel.Charts = repository.GetCharts(id);

            // Data - Population
            homeModel.DisplacedEvacuated = repository.GetDisplacedEvacuated(id);
            homeModel.Casualties = repository.GetCasualties(id);

            // Data - Properties
            homeModel.DamagedProperties = repository.GetDamagedProperties(id);

            // Data - Lifelines
            homeModel.Transportation = repository.GetTransportation(id);
            homeModel.Communication = repository.GetCommunication(id);
            homeModel.Electrical = repository.GetElectrical(id);
            homeModel.WaterFacilities = repository.GetWaterFacilities(id);

            // Data - Agriculture
            homeModel.Crops = repository.GetCrops(id);
            homeModel.Fisheries = repository.GetFisheries(id);
            homeModel.Livestocks = repository.GetLivestocks(id);

            return Ok(homeModel);
        }
    }
}