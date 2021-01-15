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
        public static int CreateLanguage(string name, int userid)
        {
            LanguageDataModel data = new LanguageDataModel
            {
                Name = name,
                UserId = userid
            };

            string sql = @"INSERT INTO dbo.Languages (Name, UserId)
                           VALUES (@Name, @UserId);";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<LanguageDataModel> LoadLanguages()
        {
            string sql = @"SELECT Id, Name, UserId FROM dbo.Languages;";

            return SqlDataAccess.LoadData<LanguageDataModel>(sql);
        }

        public static List<LanguageDataModel> LoadLanguages(int userid)
        {
            string sql = @"SELECT Id, Name, UserId FROM dbo.Languages WHERE UserId = @UserId";

            object[] parameters = { userid };

            return SqlDataAccess.LoadData<LanguageDataModel>(sql, parameters);
        }
    }
}
