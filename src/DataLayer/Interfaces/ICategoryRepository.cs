using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TYRISKANALYSIS.Controllers.API.Utilities;

namespace TYRISKANALYSIS.DataLayer.Interfaces
{
    public interface ICategoryRepository
    {
        Category GetById(int id);
        PagedResult<Category> GetAll(PagedParams pagedParams);
        int GetTotalCount(string whereClause, string searchString);
        Category Add(Category category);
        Category Update(Category category);
        void Remove(int id);
    }
}
