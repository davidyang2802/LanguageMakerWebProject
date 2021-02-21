using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageMakerDataLibrary.DataModels;

namespace LanguageMakerDataLibrary.BusinessLogic
{
    public static class LetterTypeProcessor
    {
        public static int CreateLetterType(string name, int languageid, string description)
        {
            LetterTypeDataModel data = new LetterTypeDataModel
            {
                Name = name,
                LanguageId = languageid,
                Description = description
            };

            string sql = @"INSERT INTO dbo.LetterTypes (Name, LanguageId, Description)
                           VALUES (@Name, @LanguageId, @Description);";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<LetterTypeDataModel> LoadLetterTypes(int languageid)
        {
            string sql = @"SELECT Id, Name, LanguageId, Description FROM dbo.LetterTypes WHERE LanguageId = @LanguageId;";
            var parameters = new { LanguageId = languageid };

            return SqlDataAccess.LoadData<LetterTypeDataModel>(sql, parameters).ToList();
        }

        public static int getLetterTypesCount(int languageid)
        {
            string sql = "SELECT COUNT(Id) FROM dbo.LetterTypes WHERE LanguageId = " + languageid.ToString() + ";";

            return SqlDataAccess.getTableCount(sql);
        }
    }
}
