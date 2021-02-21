using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageMakerDataLibrary.DataModels;

namespace LanguageMakerDataLibrary.BusinessLogic
{
    public static class TagProcessor
    {
        public static int CreateTag(string name, int languageid, string description)
        {
            TagDataModel data = new TagDataModel
            {
                Name = name,
                LanguageId = languageid,
                Description = description
            };

            string sql = @"INSERT INTO dbo.Tags (Name, LanguageId, Description)
                           VALUES (@Name, @LanguageId, @Description);";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<TagDataModel> LoadTags(int languageid)
        {
            string sql = @"SELECT Id, Name, LanguageId, Description FROM dbo.Tags WHERE LanguageId = @LanguageId;";
            var parameters = new { LanguageId = languageid };

            return SqlDataAccess.LoadData<TagDataModel>(sql, parameters).ToList();
        }

        public static int getTagsCount(int languageid)
        {
            string sql = "SELECT COUNT(Id) FROM dbo.Tags WHERE LanguageId = " + languageid.ToString() + ";";

            return SqlDataAccess.getTableCount(sql);
        }
    }
}
