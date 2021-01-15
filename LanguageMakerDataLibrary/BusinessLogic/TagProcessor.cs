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
        public static int CreateTag(string name, int languageid)
        {
            TagDataModel data = new TagDataModel
            {
                Name = name,
                LanguageId = languageid
            };

            string sql = @"INSERT INTO dbo.Tags (Name, LanguageId)
                           VALUES (@Name, @LanguageId);";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<TagDataModel> LoadTags()
        {
            string sql = @"SELECT Id, Name, LanguageId FROM dbo.Tags;";

            return SqlDataAccess.LoadData<TagDataModel>(sql);
        }
    }
}
