using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageMakerDataLibrary.DataModels;

namespace LanguageMakerDataLibrary.BusinessLogic
{
    public static class ClassificationProcessor
    {
        public static int CreateClassfication(string name, int languageid, string description)
        {
            ClassificationDataModel data = new ClassificationDataModel
            {
                Name = name,
                LanguageId = languageid,
                Description = description
            };

            string sql = @"INSERT INTO dbo.Classifications (Name, LanguageId, Description)
                           VALUES (@Name, @LanguageId, @Description);";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<ClassificationDataModel> LoadClassifications(int languageid)
        {
            string sql = @"SELECT Id, Name, LanguageId, Description FROM dbo.Classifications WHERE LanguageId = @LanguageId;";
            var parameters = new { LanguageId = languageid };

            return SqlDataAccess.LoadData<ClassificationDataModel>(sql, parameters).ToList();
        }

        public static int getClassificationsCount(int languageid)
        {
            string sql = "SELECT COUNT(Id) FROM dbo.Classifications WHERE LanguageId = " + languageid.ToString() + ";";

            return SqlDataAccess.getTableCount(sql);
        }
    }
}
