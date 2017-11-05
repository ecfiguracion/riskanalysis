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

namespace TYRISKANALYSIS.DataLayer.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private IDbConnection db;

        public CategoryRepository(IConfiguration appconfig)
        {            
            db = new SqlConnection(appconfig.GetConnectionString(AppConfigConstants.DefaultConnection));
        }

        public PagedResult<Category> GetAll(PagedParams pagedParams)
        {
            var sql = "SELECT id,code,name FROM category";
            var sqlWhereClause = string.Empty;

            // add filtering
            if (!string.IsNullOrEmpty(pagedParams.searchString))
            {
                sqlWhereClause += " WHERE name LIKE @searchString ";
                sql += sqlWhereClause;
            }

            // implement paging
            sql += @" ORDER BY code 
                      OFFSET @pageNo ROWS
                      FETCH NEXT @pageSize ROWS ONLY";

            pagedParams.SetPageNo();
            var pageNo = (pagedParams.pageNo - 1) * pagedParams.pageSize;
            var searchString = "%" + pagedParams.searchString + "%";
            var categories = db.Query<Category>(sql, new { searchString, pageNo, pagedParams.pageSize });

            return new PagedResult<Category>(categories, pagedParams, this.GetTotalCount(sqlWhereClause,searchString));
        }

        public int GetTotalCount(string whereClause,string searchString)
        {
            var sql = "SELECT COUNT(id) FROM category " + whereClause;
            return db.Query<int>(sql,new { searchString } ).Single();
        }

        public Category GetById(int id)
        {
            var sql = @"SELECT id,code,name FROM Category
                        WHERE id = @id";
            return db.Query<Category>(sql, new { id }).SingleOrDefault();
        }

        public Category Add(Category category)
        {
            var sql = @"INSERT INTO Category(name)
                        VALUES (@Name)
                        SELECT CAST(SCOPE_IDENTITY() AS int)";
            var id = db.Query<int>(sql, category).Single();
            category.Id = id;
            return category;
        }

        public Category Update(Category category)
        {            
            var sql = @"UPDATE Category SET 
                        name = @Name
                    WHERE id = @id";
            db.Execute(sql, category);
            return category;
        }

        public void Remove(int id)
        {
            var sql = @"DELETE FROM Category
                        WHERE id = @id";
            db.Query<Category>(sql, new { id });
        }
    }
}
