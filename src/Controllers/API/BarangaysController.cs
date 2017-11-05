using Microsoft.AspNetCore.Mvc;
using TYRISKANALYSIS.Controllers.API.Utilities;
using Microsoft.Extensions.Configuration;
using TYRISKANALYSIS.DataLayer.Interfaces;
using TYRISKANALYSIS.DataLayer.Repository;
using TYRISKANALYSIS.DataLayer;

namespace TYRISKANALYSIS.Controllers.API
{
    [Produces("application/json")]
    [Route("api/barangays")]
    public class BarangaysController : Controller
    {
        private IBarangayRepository repository;

        public BarangaysController(IConfiguration appconfig)
        {
            repository = new BarangayRepository(appconfig);
        }

        // GET api/barangays
        [HttpGet]
        public IActionResult GetBarangays(PagedParams param)
        {
            return Ok(repository.GetAll(param));
        }

        // GET api/barangays/1
        [HttpGet("{id}")]
        public IActionResult GetBarangay(int id)
        {
            var barangay = repository.GetById(id);

            if (barangay == null)
            {
                return NotFound();
            }

            return Ok(barangay);
        }

        // POST api/barangays
        [HttpPost]
        public IActionResult SaveBarangay([FromBody] Barangay barangay)
        {
            if (barangay.Id == 0)
                repository.Add(barangay);
            else
                repository.Update(barangay);
            return Ok(barangay);
        }

        // DELETE api/barangays/1
        [HttpDelete("{id}")]
        public IActionResult DeleteBarangay(int id)
        {
            repository.Remove(id);
            return Ok();
        }
    }
}