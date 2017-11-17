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
        private IConfiguration appConfig;

        public LookUpsController(IConfiguration appconfig)
        {
            appConfig = appconfig;
            repository = new LookUpRepository(appconfig);
        }

        // GET api/lookups/datalookup
        [HttpGet("datalookups")]
        public IActionResult GetLookUp()
        {
            var category = new CategoryRepository(appConfig);

            return Ok(category.Lists());
        }

        // GET api/lookups
        [HttpGet]
        public IActionResult GetLookUp(PagedParams param)
        {
            var categoryId = param.Parameter1;
            return Ok(repository.GetAll(param,categoryId));
        }

        // GET api/lookups/1
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

        // POST api/lookups
        [HttpPost]
        public IActionResult SaveCategory([FromBody] LookUpModel lookup)
        {
            if (lookup.Id == 0)
                repository.Add(lookup);
            else
                repository.Update(lookup);
            return Ok(lookup);
        }

        // DELETE api/typhoons/1
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            repository.Remove(id);
            return Ok();
        }
    }
}