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
    [Route("api/lookups")]
    public class LookUpsController : Controller
    {
        private ILookUpRepository repository;

        public LookUpsController(IConfiguration appconfig)
        {
            repository = new LookUpRepository(appconfig);
        }

        // GET api/barangays
        [HttpGet]
        public IActionResult GetLookUp(PagedParams param, int categoryid)
        {
            return Ok(repository.GetAll(param,categoryid));
        }

        // GET api/barangays/1
        [HttpGet("{id}")]
        public IActionResult GetLookUp(int id)
        {
            var lookup = repository.GetById(id);

            if (lookup == null)
            {
                return NotFound();
            }

            return Ok(lookup);
        }

        // POST api/barangays
        [HttpPost]
        public IActionResult SaveCategory([FromBody] LookUpModel lookup)
        {
            if (lookup.Id == 0)
                repository.Add(lookup);
            else
                repository.Update(lookup);
            return Ok(lookup);
        }
    }
}