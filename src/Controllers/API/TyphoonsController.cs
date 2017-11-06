using Microsoft.AspNetCore.Mvc;
using TYRISKANALYSIS.Controllers.API.Utilities;
using Microsoft.Extensions.Configuration;
using TYRISKANALYSIS.DataLayer.Interfaces;
using TYRISKANALYSIS.DataLayer.Repository;
using TYRISKANALYSIS.DataLayer;

namespace TYRISKANALYSIS.Controllers.API
{
    [Produces("application/json")]
    [Route("api/typhoons")]
    public class TyphoonsController : Controller
    {
        private ITyphoonRepository repository;

        public TyphoonsController(IConfiguration appconfig)
        {
            repository = new TyphoonRepository(appconfig);
        }

        // GET api/typhoons
        [HttpGet]
        public IActionResult GetTyphoons(PagedParams param)
        {
            var request = this.Request.QueryString;
            return Ok(repository.GetAll(param));
        }

        // GET api/typhoons/1
        [HttpGet("{id}")]
        public IActionResult GetTyphoon(int id)
        {
            var typhoon = repository.GetById(id);

            if (typhoon == null)
            {
                return NotFound();
            }

            return Ok(typhoon);
        }

        // POST api/typhoons
        [HttpPost]
        public IActionResult SaveTyphoon([FromBody] Typhoon typhoon)
        {
            if (typhoon.Id == 0)
                repository.Add(typhoon);
            else
                repository.Update(typhoon);
            return Ok(typhoon);
        }

        // DELETE api/typhoons/1
        [HttpDelete("{id}")]
        public IActionResult DeleteTyphoon(int id)
        {
            repository.Remove(id);
            return Ok();
        }
    }
}