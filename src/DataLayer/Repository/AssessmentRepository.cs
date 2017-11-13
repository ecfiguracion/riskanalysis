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

namespace TYRISKANALYSIS.DataLayer.Repository
{
    public class AssessmentRepository : IAssessmentRepository
    {
        private IDbConnection db;

        public AssessmentRepository(IConfiguration appconfig)
        {            
            db = new SqlConnection(appconfig.GetConnectionString(AppConfigConstants.DefaultConnection));
        }

        public PagedResult<AssessmentModel> GetAll(PagedParams pagedParams)
        {
            var sql = @"SELECT a.id, t.name as typhoonName, a.remarks
                        FROM Assessment a
                        INNER JOIN Typhoon t ON a.typhoonId = t.Id";
            var sqlWhereClause = string.Empty;
            // add filtering
            if (!string.IsNullOrEmpty(pagedParams.searchString))
            {
                sqlWhereClause += " AND t.name LIKE @searchString ";
            }
            sql += sqlWhereClause;


            // implement paging
            sql += @" ORDER BY t.name
                      OFFSET @pageNo ROWS
                      FETCH NEXT @pageSize ROWS ONLY";

            pagedParams.SetPageNo();
            var pageNo = (pagedParams.pageNo - 1) * pagedParams.pageSize;
            var searchString = "%" + pagedParams.searchString + "%";
            var assessments = db.Query<AssessmentModel>(sql, new {  searchString, pageNo, pagedParams.pageSize });

            return new PagedResult<AssessmentModel>(assessments, pagedParams, this.GetTotalCount(sqlWhereClause,searchString));
        }

        public int GetTotalCount(string whereClause, string searchString)
        {
            var sql = @"SELECT COUNT(a.id)
                        FROM Assessment a
                        INNER JOIN Typhoon t ON a.typhoonId = t.Id " + 
                        whereClause;
            return db.Query<int>(sql,new { searchString } ).Single();
        }

        public AssessmentModel GetById(int id)
        {
            var sql = @"SELECT id,remarks FROM assessment
                        WHERE id = @id";
            return db.Query<AssessmentModel>(sql, new { id }).SingleOrDefault();
        }

        public AssessmentModel Add(AssessmentModel assessment)
        {
            var sql = @"INSERT INTO assessment(typhoonid,remarks)
                        VALUES (@Name,@TyphoonId)
                        SELECT CAST(SCOPE_IDENTITY() AS int)";
            var id = db.Query<int>(sql, assessment).Single();
            assessment.Id = id;
            return assessment;
        }

        public AssessmentModel Update(AssessmentModel assessment)
        {            
            var sql = @"UPDATE assessment SET 
                            remarks = @Remarks
                        WHERE id = @id";
            db.Execute(sql, assessment);
            return assessment;
        }

        public void Remove(int id)
        {
            var sql = @"DELETE FROM assessment
                        WHERE id = @id";
            db.Query<AssessmentModel>(sql, new { id });
        }
    }
}
