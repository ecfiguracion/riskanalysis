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
        private IUserAccountRepository authenticate;

        public AssessmentsController(IConfiguration appconfig)
        {
            repository = new AssessmentRepository(appconfig);
            authenticate = new UserAccountRepository(appconfig);
            appConfig = appconfig;
        }

        // GET api/assessments
        [HttpGet]
        public IActionResult GetAssessments(PagedParams param)
        {
            var token = this.Request.Headers["tokenAuthorization"];
            if (authenticate.GetByToken(token))
            {
                var request = this.Request.QueryString;
                return Ok(repository.GetAll(param));
            } else
            {
                return BadRequest();
            }
        }

        // GET api/assessments/1
        [HttpGet("{id}")]
        public IActionResult GetAssessment(int id)
        {
            var token = this.Request.Headers["tokenAuthorization"];
            if (authenticate.GetByToken(token))
            {
                var assessment = repository.GetById(id);

                if (assessment == null)
                {
                    return NotFound();
                }

                return Ok(assessment);
            } else
            {
                return BadRequest();
            }
        }

        // GET api/assessments/datalookup
        [HttpGet("datalookups")]
        public IActionResult GetLookUp()
        {
            var token = this.Request.Headers["tokenAuthorization"];
            if (authenticate.GetByToken(token))
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
            } else
            {
                return BadRequest();
            }
        }

        // POST api/assessments
        [HttpPost]
        public IActionResult SaveAssessment([FromBody] AssessmentModel assessment)
        {
            var token = this.Request.Headers["tokenAuthorization"];
            if (authenticate.GetByToken(token))
            {
                if (assessment.IsNew)
                    repository.Add(assessment);
                else
                    repository.Update(assessment);
                return Ok(assessment);
            } else
            {
                return BadRequest();
            }
        }

        // DELETE api/assessments/1
        [HttpDelete("{id}")]
        public IActionResult DeleteAssessment(int id)
        {
            var token = this.Request.Headers["tokenAuthorization"];
            if (authenticate.GetByToken(token))
            {
                repository.Remove(id);
                return Ok();
            } else
            {
                return BadRequest();
            }
        }
    }
}