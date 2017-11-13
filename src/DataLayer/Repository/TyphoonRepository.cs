using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TYRISKANALYSIS.Constants;
using TYRISKANALYSIS.Controllers.API.Utilities;
using TYRISKANALYSIS.DataLayer.Interfaces;
using TYRISKANALYSIS.Models.LookUp;

namespace TYRISKANALYSIS.DataLayer.Repository
{
    public class TyphoonRepository : ITyphoonRepository
    {
        private IDbConnection db;

        public TyphoonRepository(IConfiguration appconfig)
        {            
            db = new SqlConnection(appconfig.GetConnectionString(AppConfigConstants.DefaultConnection));
        }

        public PagedResult<Typhoon> GetAll(PagedParams pagedParams)
        {
            var sql = "SELECT id,name,datehit,signalno,windkph,hourslasted FROM typhoon";
            var sqlWhereClause = string.Empty;

            // add filtering
            if (!string.IsNullOrEmpty(pagedParams.searchString))
            {
                sqlWhereClause += " WHERE name LIKE @searchString ";
                sql += sqlWhereClause;
            }

            // implement paging
            sql += @" ORDER BY datehit 
                      OFFSET @pageNo ROWS
                      FETCH NEXT @pageSize ROWS ONLY";

            pagedParams.SetPageNo();
            var pageNo = (pagedParams.pageNo - 1) * pagedParams.pageSize;
            var searchString = "%" + pagedParams.searchString + "%";
            var typhoons = db.Query<Typhoon>(sql, new { searchString, pageNo, pagedParams.pageSize });

            return new PagedResult<Typhoon>(typhoons, pagedParams, this.GetTotalCount(sqlWhereClause,searchString));
        }

        public int GetTotalCount(string whereClause,string searchString)
        {
            var sql = "SELECT COUNT(id) FROM typhoon " + whereClause;
            return db.Query<int>(sql,new { searchString } ).Single();
        }

        public Typhoon GetById(int id)
        {
            var sql = @"SELECT id,name,datehit,signalno,windkph,hourslasted FROM typhoon
                        WHERE id = @id";
            return db.Query<Typhoon>(sql, new { id }).SingleOrDefault();
        }

        public Typhoon Add(Typhoon typhoon)
        {
            var sql = @"INSERT INTO typhoon(name,datehit,signalno,windkph,hourslasted)
                    VALUES (@Name,@DateHit,@SignalNo,@WindKPH,@HoursLasted)
                    SELECT CAST(SCOPE_IDENTITY() AS int)";
            var id = db.Query<int>(sql, typhoon).Single();
            typhoon.Id = id;
            return typhoon;
        }

        public Typhoon Update(Typhoon typhoon)
        {            
            var sql = @"UPDATE typhoon SET 
                        name = @Name,
                        datehit = @DateHit,
                        signalno = @SignalNo,
                        windkph = @WindKPH,
                        hourslasted = @HoursLasted
                    WHERE id = @id";
            db.Execute(sql, typhoon);
            return typhoon;
        }

        public void Remove(int id)
        {
            var sql = @"DELETE FROM typhoon
                        WHERE id = @id";
            db.Query<Typhoon>(sql, new { id });
        }

        #region Miscellaneous

        public IEnumerable<DataLookUpModel> Lists()
        {
            var lists = db.Query<DataLookUpModel>("SELECT id,name FROM typhoon ORDER by name");
            return lists;
        }

        #endregion
    }
}
