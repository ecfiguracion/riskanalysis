using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TYRISKANALYSIS.Controllers.API.Utilities;

namespace TYRISKANALYSIS.DataLayer.Interfaces
{
    public interface IBarangayRepository
    {
        Barangay GetById(int id);
        PagedResult<Barangay> GetAll(PagedParams pagedParams);
        int GetTotalCount(string whereClause, string searchString);
        Barangay Add(Barangay barangay);
        Barangay Update(Barangay barangay);
        void Remove(int id);
    }
}
