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
        private IUserAccountRepository authenticate;

        public BarangaysController(IConfiguration appconfig)
        {
            repository = new BarangayRepository(appconfig);
            authenticate = new UserAccountRepository(appconfig);
        }

        // GET api/barangays
        [HttpGet]
        public IActionResult GetBarangays(PagedParams param)
        {
            var request = this.Request.QueryString;
            var token = this.Request.Headers["tokenAuthorization"];
            if (authenticate.GetByToken(token))
                return Ok(repository.GetAll(param));
            else
                return BadRequest();
        }

        // GET api/barangays/1
        [HttpGet("{id}")]
        public IActionResult GetBarangay(int id)
        {
            var token = this.Request.Headers["tokenAuthorization"];
            if (authenticate.GetByToken(token))
            {
                var barangay = repository.GetById(id);

                if (barangay == null)
                {
                    return NotFound();
                }

                return Ok(barangay);
            } else
            {
                return BadRequest();
            }
        }

        // POST api/barangays
        [HttpPost]
        public IActionResult SaveBarangay([FromBody] Barangay barangay)
        {
            var token = this.Request.Headers["tokenAuthorization"];
            if (authenticate.GetByToken(token))
            {
                if (barangay.Id == 0)
                    repository.Add(barangay);
                else
                    repository.Update(barangay);
                return Ok(barangay);
            } else
            {
                return BadRequest();
            }
        }

        // DELETE api/barangays/1
        [HttpDelete("{id}")]
        public IActionResult DeleteBarangay(int id)
        {
            var token = this.Request.Headers["tokenAuthorization"];
            if (authenticate.GetByToken(token))
            {
                repository.Remove(id);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}