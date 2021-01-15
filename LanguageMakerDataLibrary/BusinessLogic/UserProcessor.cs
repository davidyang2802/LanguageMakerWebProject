using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageMakerDataLibrary.DataModels;

namespace LanguageMakerDataLibrary.BusinessLogic
{
    public static class UserProcessor
    {
        public static int CreateUser(string username)
        {
            UserDataModel data = new UserDataModel
            {
                UserName = username
            };

            string sql = @"INSERT INTO dbo.Users (Username)
                           VALUES (@Username);";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<UserDataModel> LoadUsers()
        {
            string sql = @"SELECT Id, Username FROM dbo.Users;";

            return SqlDataAccess.LoadData<UserDataModel>(sql).ToList();
        }

        public static bool CheckUsername(string username)
        {
            string sql = @"SELECT Id FROM dbo.Users WHERE Username = @Username;";
            var parameters = new { Username = username };
            return SqlDataAccess.CheckTableDataFromColumn(sql, parameters);
        }

        public static int getUserId(string username)
        {
            string sql = @"SELECT Id FROM dbo.Users WHERE Username = @Username;";
            var parameters = new { Username = username };

            return SqlDataAccess.GetFirstTableDataFromColumnValue<int>(sql, parameters);
        }
    }
}
