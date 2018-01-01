using Microsoft.AspNetCore.Mvc;
using TYRISKANALYSIS.Controllers.API.Utilities;
using Microsoft.Extensions.Configuration;
using TYRISKANALYSIS.DataLayer.Interfaces;
using TYRISKANALYSIS.DataLayer.Repository;
using TYRISKANALYSIS.DataLayer;

namespace TYRISKANALYSIS.Controllers.API
{
    [Produces("application/json")]
    [Route("api/useraccounts")]
    public class UserAccountController : Controller
    {
        private IUserAccountRepository repository;
        private IUserAccountRepository authenticate;

        public UserAccountController(IConfiguration appconfig)
        {
            repository = new UserAccountRepository(appconfig);
            authenticate = new UserAccountRepository(appconfig);
        }

        // GET api/useraccounts
        [HttpGet]
        public IActionResult GetUserAccount(PagedParams param)
        {
            var token = this.Request.Headers["tokenAuthorization"];
            if (authenticate.GetByToken(token))
            {
                var request = this.Request.QueryString;
                return Ok(repository.GetAll(param));
            }
            else
            {
                return BadRequest();
            }
        }

        // GET api/useraccounts/1
        [HttpGet("{id}")]
        public IActionResult GetUserAccount(int id)
        {
            var token = this.Request.Headers["tokenAuthorization"];
            if (authenticate.GetByToken(token))
            {
                var userAccount = repository.GetById(id);

                if (userAccount == null)
                {
                    return NotFound();
                }

                return Ok(userAccount);
            } else
            {
                return BadRequest();
            }
        }

        // POST api/useraccounts
        [HttpPost]
        public IActionResult SaveUserAccount([FromBody] UserAccount userAccount)
        {
            var token = this.Request.Headers["tokenAuthorization"];
            if (authenticate.GetByToken(token))
            {
                if (userAccount.Id == 0)
                    repository.Add(userAccount);
                else
                    repository.Update(userAccount);
                return Ok(userAccount);
            } else
            {
                return BadRequest();
            }
        }

        // DELETE api/useraccounts/1
        [HttpDelete("{id}")]
        public IActionResult DeleteUserAccount(int id)
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

        // POST api/useraccounts
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] Authenticate authenticate)
        {
            var token = repository.Authenticate(authenticate);
            return Ok(token);
        }
    }
}