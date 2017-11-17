using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TYRISKANALYSIS.Controllers.API.Utilities;
using TYRISKANALYSIS.DataLayer.Model;

namespace TYRISKANALYSIS.DataLayer.Interfaces
{
    public interface IAssessmentRepository
    {
        AssessmentModel GetById(int id);
        PagedResult<AssessmentListModel> GetAll(PagedParams pagedParams);
        int GetTotalCount(string whereClause, string searchString);
        AssessmentModel Add(AssessmentModel assessment);
        AssessmentModel Update(AssessmentModel assessment);
        void Remove(int id);
    }
}
