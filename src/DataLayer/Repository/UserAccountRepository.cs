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
    public class UserAccountRepository : IUserAccountRepository
    {
        private IDbConnection db;

        public UserAccountRepository(IConfiguration appconfig)
        {            
            db = new SqlConnection(appconfig.GetConnectionString(AppConfigConstants.DefaultConnection));
        }

        public PagedResult<UserAccount> GetAll(PagedParams pagedParams)
        {
            var sql = "SELECT id,name,email,username,password,token,isactive FROM useraccount";
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
            var useraccounts = db.Query<UserAccount>(sql, new { searchString, pageNo, pagedParams.pageSize });

            return new PagedResult<UserAccount>(useraccounts, pagedParams, this.GetTotalCount(sqlWhereClause,searchString));
        }

        public int GetTotalCount(string whereClause,string searchString)
        {
            var sql = "SELECT COUNT(id) FROM useraccount " + whereClause;
            return db.Query<int>(sql,new { searchString } ).Single();
        }

        public UserAccount GetById(int id)
        {
            var sql = @"SELECT id,name,email,username,password,token,isactive FROM useraccount
                        WHERE id = @id";
            return db.Query<UserAccount>(sql, new { id }).SingleOrDefault();
        }

        public bool AuthenticateToken(string token)
        {
            var sql = @"SELECT id WHERE token = @token";
            var id = db.Query<int>(sql, new { token }).SingleOrDefault();
            return id > 0;
        }

        public UserAccount Add(UserAccount useraccount)
        {
            var sql = @"INSERT INTO UserAccount(name,email,username,password,token,isactive)
                    VALUES (@Name,@Email,@Username,@Password,@Token,@IsActive)
                    SELECT CAST(SCOPE_IDENTITY() AS int)";
            var id = db.Query<int>(sql, useraccount).Single();
            useraccount.Id = id;
            return useraccount;
        }

        public UserAccount Update(UserAccount useraccount)
        {            
            var sql = @"UPDATE UserAccount SET 
                        name = @Name,
                        email = @Email,
                        username = @Username,
                        password = @Password,
                        isactive = @IsActive
                    WHERE id = @id";
            db.Execute(sql, useraccount);
            return useraccount;
        }

        public void Remove(int id)
        {
            var sql = @"DELETE FROM useraccount
                        WHERE id = @id";
            db.Query<UserAccount>(sql, new { id });
        }

        public string Authenticate(Authenticate authenticate)
        {
            var sql = @"SELECT id FROM useraccount 
                        WHERE username = @username AND password = @password";

            var id = db.Query<int>(sql, new { authenticate.Username, authenticate.Password }).SingleOrDefault();
            var token = string.Empty;
            if (id > 0)
            {
                token = Guid.NewGuid().ToString();
                sql = "UPDATE useraccount SET token = @token WHERE id = @id";
                db.Execute(sql, new { token, id });
            }

            return token;
        }

        #region Miscellaneous

        public IEnumerable<DataLookUpModel> Lists()
        {
            var lists = db.Query<DataLookUpModel>("SELECT id,name,email,isactive FROM useraccount ORDER by name");
            return lists;
        }

        #endregion
    }
}
