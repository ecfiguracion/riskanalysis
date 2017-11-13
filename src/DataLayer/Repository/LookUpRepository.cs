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
using TYRISKANALYSIS.DataLayer.Model;
using TYRISKANALYSIS.Models.LookUp;

namespace TYRISKANALYSIS.DataLayer.Repository
{
    public class LookUpRepository : ILookUpRepository
    {
        private IDbConnection db;

        public LookUpRepository(IConfiguration appconfig)
        {            
            db = new SqlConnection(appconfig.GetConnectionString(AppConfigConstants.DefaultConnection));
        }

        public PagedResult<LookUpModel> GetAll(PagedParams pagedParams, int categoryid)
        {
            var sql = @"SELECT l.id,l.name,c.name as categoryname 
                        FROM lookup l
                        INNER JOIN Category c ON l.categoryid = c.id";

            var sqlWhereClause = " WHERE l.categoryid = @categoryid";

            // add filtering
            if (!string.IsNullOrEmpty(pagedParams.searchString))
            {
                sqlWhereClause += " AND l.name LIKE @searchString ";
            }
            sql += sqlWhereClause;


            // implement paging
            sql += @" ORDER BY l.name
                      OFFSET @pageNo ROWS
                      FETCH NEXT @pageSize ROWS ONLY";

            pagedParams.SetPageNo();
            var pageNo = (pagedParams.pageNo - 1) * pagedParams.pageSize;
            var searchString = "%" + pagedParams.searchString + "%";
            var lookups = db.Query<LookUpModel>(sql, new {  categoryid, searchString, pageNo, pagedParams.pageSize });

            return new PagedResult<LookUpModel>(lookups, pagedParams, this.GetTotalCount(sqlWhereClause,categoryid,searchString));
        }

        public int GetTotalCount(string whereClause,int categoryid, string searchString)
        {
            var sql = @"SELECT COUNT(l.id) 
                        FROM lookup l
                        INNER JOIN category c ON l.categoryid = c.id " + 
                        whereClause;
            return db.Query<int>(sql,new { categoryid, searchString } ).Single();
        }

        public LookUpModel GetById(int id)
        {
            var sql = @"SELECT id,name FROM lookup
                        WHERE id = @id";
            return db.Query<LookUpModel>(sql, new { id }).SingleOrDefault();
        }

        public IEnumerable<DataLookUpModel> GetByCategory(List<int> categories)
        {
            var ids = string.Join(",", categories);

            var sql = @"SELECT l.id,l.name 
                        FROM lookup l
                        INNER JOIN category c ON l.CategoryId = c.Id
                        WHERE c.code IN (" + ids + ")";
            return db.Query<DataLookUpModel>(sql);
        }


        public LookUpModel Add(LookUpModel lookup)
        {
            var sql = @"INSERT INTO lookup(name,categoryid)
                        VALUES (@Name,@CategoryId)
                        SELECT CAST(SCOPE_IDENTITY() AS int)";
            var id = db.Query<int>(sql, lookup).Single();
            lookup.Id = id;
            return lookup;
        }

        public LookUpModel Update(LookUpModel lookup)
        {            
            var sql = @"UPDATE lookup SET 
                        name = @Name
                    WHERE id = @id";
            db.Execute(sql, lookup);
            return lookup;
        }

        public void Remove(int id)
        {
            var sql = @"DELETE FROM lookup
                        WHERE id = @id";
            db.Query<LookUpModel>(sql, new { id });
        }
    }
}
