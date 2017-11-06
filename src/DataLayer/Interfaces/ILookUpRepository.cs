using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TYRISKANALYSIS.Controllers.API.Utilities;
using TYRISKANALYSIS.DataLayer.Model;

namespace TYRISKANALYSIS.DataLayer.Interfaces
{
    public interface ILookUpRepository
    {
        LookUpModel GetById(int id);
        PagedResult<LookUpModel> GetAll(PagedParams pagedParams, int category);
        int GetTotalCount(string whereClause,int categoryid, string searchString);
        LookUpModel Add(LookUpModel barangay);
        LookUpModel Update(LookUpModel barangay);
        void Remove(int id);
    }
}
