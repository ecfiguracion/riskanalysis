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
    public class BarangayRepository : IBarangayRepository
    {
        private IDbConnection db;

        public BarangayRepository(IConfiguration appconfig)
        {            
            db = new SqlConnection(appconfig.GetConnectionString(AppConfigConstants.DefaultConnection));
        }

        public PagedResult<Barangay> GetAll(PagedParams pagedParams)
        {
            var sql = "SELECT id,name,latitude,longitude FROM barangay";
            var sqlWhereClause = string.Empty;

            // add filtering
            if (!string.IsNullOrEmpty(pagedParams.searchString))
            {
                sqlWhereClause += " WHERE name LIKE @searchString ";
                sql += sqlWhereClause;
            }

            // implement paging
            sql += @" ORDER BY name 
                      OFFSET @pageNo ROWS
                      FETCH NEXT @pageSize ROWS ONLY";

            pagedParams.SetPageNo();
            var pageNo = (pagedParams.pageNo - 1) * pagedParams.pageSize;
            var searchString = "%" + pagedParams.searchString + "%";
            var barangays = db.Query<Barangay>(sql, new { searchString, pageNo, pagedParams.pageSize });

            return new PagedResult<Barangay>(barangays, pagedParams, this.GetTotalCount(sqlWhereClause,searchString));
        }

        public int GetTotalCount(string whereClause,string searchString)
        {
            var sql = "SELECT COUNT(id) FROM barangay " + whereClause;
            return db.Query<int>(sql,new { searchString } ).Single();
        }

        public Barangay GetById(int id)
        {
            var sql = @"SELECT id,name,latitude,longitude FROM barangay
                        WHERE id = @id";
            return db.Query<Barangay>(sql, new { id }).SingleOrDefault();
        }

        public Barangay Add(Barangay barangay)
        {
            var sql = @"INSERT INTO Barangay(name,latitude,longitude)
                    VALUES (@Name,@Latitude,@Longitude)
                    SELECT CAST(SCOPE_IDENTITY() AS int)";
            var id = db.Query<int>(sql, barangay).Single();
            barangay.Id = id;
            return barangay;
        }

        public Barangay Update(Barangay barangay)
        {            
            var sql = @"UPDATE Barangay SET 
                        name = @Name,
                        latitude = @Latitude,
                        longitude = @Longitude
                    WHERE id = @id";
            db.Execute(sql, barangay);
            return barangay;
        }

        public void Remove(int id)
        {
            var sql = @"DELETE FROM barangay
                        WHERE id = @id";
            db.Query<Barangay>(sql, new { id });
        }

        #region Miscellaneous

        public IEnumerable<DataLookUpModel> Lists()
        {
            var lists = db.Query<DataLookUpModel>("SELECT id,name FROM barangay ORDER by name");
            return lists;
        }

        #endregion
    }
}
