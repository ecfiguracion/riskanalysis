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

        public CategoryController(IConfiguration appconfig)
        {
            repository = new CategoryRepository(appconfig);
        }

        // GET api/barangays
        [HttpGet]
        public IActionResult GetCategory(PagedParams param)
        {
            return Ok(repository.GetAll(param));
        }

        // GET api/barangays/1
        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            var category = repository.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // POST api/barangays
        [HttpPost]
        public IActionResult SaveCategory([FromBody] Category category)
        {
            if (category.Id == 0)
                repository.Add(category);
            else
                repository.Update(category);
            return Ok(category);
        }
    }
}