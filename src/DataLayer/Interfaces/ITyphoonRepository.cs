using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TYRISKANALYSIS.Controllers.API.Utilities;

namespace TYRISKANALYSIS.DataLayer.Interfaces
{
    public interface ITyphoonRepository
    {
        Typhoon GetById(int id);
        PagedResult<Typhoon> GetAll(PagedParams pagedParams);
        int GetTotalCount(string whereClause, string searchString);
        Typhoon Add(Typhoon barangay);
        Typhoon Update(Typhoon barangay);
        void Remove(int id);
    }
}
