using Microsoft.AspNetCore.Mvc;
using TYRISKANALYSIS.Controllers.API.Utilities;
using Microsoft.Extensions.Configuration;
using TYRISKANALYSIS.DataLayer.Interfaces;
using TYRISKANALYSIS.DataLayer.Repository;
using TYRISKANALYSIS.DataLayer;

namespace TYRISKANALYSIS.Controllers.API
{
    [Produces("application/json")]
    [Route("api/category")]
    public class CategoryController : Controller
    {
        private ICategoryRepository repository;
        private IUserAccountRepository authenticate;

        public CategoryController(IConfiguration appconfig)
        {
            repository = new CategoryRepository(appconfig);
            authenticate = new UserAccountRepository(appconfig);
        }

        // GET api/barangays
        [HttpGet]
        public IActionResult GetCategory(PagedParams param)
        {
            var token = this.Request.Headers["tokenAuthorization"];
            if (authenticate.GetByToken(token))
            {
                return Ok(repository.GetAll(param));
            } 
            else
            {
                return BadRequest();
            }
        }

        // GET api/barangays/1
        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            var token = this.Request.Headers["tokenAuthorization"];
            if (authenticate.GetByToken(token))
            {
                var category = repository.GetById(id);

                if (category == null)
                {
                    return NotFound();
                }

                return Ok(category);
            } else
            {
                return BadRequest();
            }
        }

        // POST api/barangays
        [HttpPost]
        public IActionResult SaveCategory([FromBody] Category category)
        {
            var token = this.Request.Headers["tokenAuthorization"];
            if (authenticate.GetByToken(token))
            {
                if (category.Id == 0)
                    repository.Add(category);
                else
                    repository.Update(category);
                return Ok(category);
            }
             else
            {
                return BadRequest();
            }
        }
    }
}