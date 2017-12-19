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

        public UserAccountController(IConfiguration appconfig)
        {
            repository = new UserAccountRepository(appconfig);
        }

        // GET api/useraccounts
        [HttpGet]
        public IActionResult GetUserAccount(PagedParams param)
        {
            var request = this.Request.QueryString;
            return Ok(repository.GetAll(param));
        }

        // GET api/useraccounts/1
        [HttpGet("{id}")]
        public IActionResult GetUserAccount(int id)
        {
            var userAccount = repository.GetById(id);

            if (userAccount == null)
            {
                return NotFound();
            }

            return Ok(userAccount);
        }

        // POST api/useraccounts
        [HttpPost]
        public IActionResult SaveUserAccount([FromBody] UserAccount userAccount)
        {
            if (userAccount.Id == 0)
                repository.Add(userAccount);
            else
                repository.Update(userAccount);
            return Ok(userAccount);
        }

        // DELETE api/useraccounts/1
        [HttpDelete("{id}")]
        public IActionResult DeleteUserAccount(int id)
        {
            repository.Remove(id);
            return Ok();
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