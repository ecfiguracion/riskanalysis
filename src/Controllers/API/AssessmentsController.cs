using Microsoft.AspNetCore.Mvc;
using TYRISKANALYSIS.Controllers.API.Utilities;
using Microsoft.Extensions.Configuration;
using TYRISKANALYSIS.DataLayer.Interfaces;
using TYRISKANALYSIS.DataLayer.Repository;
using TYRISKANALYSIS.DataLayer;
using TYRISKANALYSIS.DataLayer.Model;
using TYRISKANALYSIS.Models;
using TYRISKANALYSIS.Constants;
using System.Collections.Generic;

namespace TYRISKANALYSIS.Controllers.API
{
    [Produces("application/json")]
    [Route("api/assessments")]
    public class AssessmentsController : Controller
    {
        private IAssessmentRepository repository;
        private IConfiguration appConfig;

        public AssessmentsController(IConfiguration appconfig)
        {
            repository = new AssessmentRepository(appconfig);
            appConfig = appconfig;
        }

        // GET api/assessments
        [HttpGet]
        public IActionResult GetAssessments(PagedParams param)
        {
            var request = this.Request.QueryString;
            return Ok(repository.GetAll(param));
        }

        // GET api/assessments/1
        [HttpGet("{id}")]
        public IActionResult GetAssessment(int id)
        {
            var assessment = repository.GetById(id);

            if (assessment == null)
            {
                return NotFound();
            }

            return Ok(assessment);
        }

        // GET api/assessments/datalookup
        [HttpGet("datalookups")]
        public IActionResult GetLookUp()
        {
            var typhoons = new TyphoonRepository(appConfig);
            var barangays = new BarangayRepository(appConfig);
            var datalookup = new LookUpRepository(appConfig);

            var lookups = new AssessmentLookupsModel
            {
                Typhoons = typhoons.Lists(),
                Barangays = barangays.Lists(),
                PopulationLookup = datalookup.GetByCategory(new List<int> { CategoryConstants.DisplacedEntities, CategoryConstants.CasualtiesEntities }),
                StructuresLookup = datalookup.GetByCategory(new List<int> { CategoryConstants.PropertiesStructures }),
                TransportationLookup = datalookup.GetByCategory(new List<int> { CategoryConstants.TransportationFacilities }),
                CommunicationLookup = datalookup.GetByCategory(new List<int> { CategoryConstants.CommunicationFacilities }),
                ElectricalPowerLookup = datalookup.GetByCategory(new List<int> { CategoryConstants.ElectricalFacilities }),
                WaterFacilitiesLookup = datalookup.GetByCategory(new List<int> { CategoryConstants.WaterFacilities }),
                CropsLookup = datalookup.GetByCategory(new List<int> { CategoryConstants.Crops }),
                FisheriesLookup = datalookup.GetByCategory(new List<int> { CategoryConstants.Fisheries }),
                LivestockLookup = datalookup.GetByCategory(new List<int> { CategoryConstants.Livestock })
            };

            return Ok(lookups);
        }

        // POST api/assessments
        [HttpPost]
        public IActionResult SaveAssessment([FromBody] AssessmentModel assessment)
        {
            if (assessment.IsNew)
                repository.Add(assessment);
            else
                repository.Update(assessment);
            return Ok(assessment);
        }

        // DELETE api/assessments/1
        [HttpDelete("{id}")]
        public IActionResult DeleteAssessment(int id)
        {
            repository.Remove(id);
            return Ok();
        }
    }
}