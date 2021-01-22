using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageMakerDataLibrary.DataModels;

namespace LanguageMakerDataLibrary.BusinessLogic
{
    public static class LanguageProcessor
    {
        public static int CreateLanguage(string name, int userid, string description)
        {
            LanguageDataModel data = new LanguageDataModel
            {
                Name = name,
                UserId = userid,
                Description = description
            };

            string sql = @"INSERT INTO dbo.Languages (Name, UserId, Description)
                           VALUES (@Name, @UserId, @Description);";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<LanguageDataModel> LoadLanguages()
        {
            string sql = @"SELECT Id, Name, UserId, Description FROM dbo.Languages;";

            return SqlDataAccess.LoadData<LanguageDataModel>(sql).ToList();
        }

        public static List<LanguageDataModel> LoadLanguages(int userid)
        {
            string sql = @"SELECT Id, Name, UserId, Description FROM dbo.Languages WHERE UserId = @UserId";

            object[] parameters = { userid };

            return SqlDataAccess.LoadData<LanguageDataModel>(sql, parameters).ToList();
        }

        public static bool CheckDistinct(int userid, string name)
        {
            string sql = @"SELECT Id FROM dbo.Languages WHERE Userid = @Userid AND Name = @Name;";
            var parameters = new { Userid = userid, Name = name };
            return SqlDataAccess.CheckTableDataFromParameters(sql, parameters);
        }

        public static int getLanguageId(int userid, string name)
        {
            string sql = @"SELECT Id FROM dbo.Languages WHERE Userid = @Userid AND Name = @Name;";
            var parameters = new { Userid = userid, Name = name };

            return SqlDataAccess.GetFirstTableDataFromParameters<int>(sql, parameters);
        }
    }
}
