using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TYRISKANALYSIS.Controllers.API.Utilities;

namespace TYRISKANALYSIS.DataLayer.Interfaces
{
    public interface IUserAccountRepository
    {
        UserAccount GetById(int id);
        bool GetByToken(string token);
        PagedResult<UserAccount> GetAll(PagedParams pagedParams);
        int GetTotalCount(string whereClause, string searchString);
        UserAccount Add(UserAccount barangay);
        UserAccount Update(UserAccount barangay);
        string Authenticate(Authenticate authenticate);
        void Remove(int id);
    }
}
