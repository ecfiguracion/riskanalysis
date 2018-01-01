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
        private IUserAccountRepository authenticate;

        public TyphoonsController(IConfiguration appconfig)
        {
            repository = new TyphoonRepository(appconfig);
            authenticate = new UserAccountRepository(appconfig);
        }

        // GET api/typhoons
        [HttpGet]
        public IActionResult GetTyphoons(PagedParams param)
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

        // GET api/typhoons/1
        [HttpGet("{id}")]
        public IActionResult GetTyphoon(int id)
        {
            var token = this.Request.Headers["tokenAuthorization"];
            if (authenticate.GetByToken(token))
            {
                var typhoon = repository.GetById(id);

                if (typhoon == null)
                {
                    return NotFound();
                }

                return Ok(typhoon);
            } else
            {
                return BadRequest();
            }
        }

        // POST api/typhoons
        [HttpPost]
        public IActionResult SaveTyphoon([FromBody] Typhoon typhoon)
        {
            var token = this.Request.Headers["tokenAuthorization"];
            if (authenticate.GetByToken(token))
            {
                if (typhoon.Id == 0)
                    repository.Add(typhoon);
                else
                    repository.Update(typhoon);
                return Ok(typhoon);
            } else
            {
                return BadRequest();
            }
        }

        // DELETE api/typhoons/1
        [HttpDelete("{id}")]
        public IActionResult DeleteTyphoon(int id)
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