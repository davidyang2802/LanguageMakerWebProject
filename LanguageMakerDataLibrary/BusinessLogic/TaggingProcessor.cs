using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageMakerDataLibrary.DataModels;

namespace LanguageMakerDataLibrary.BusinessLogic
{
    public static class TaggingProcessor
    {
        public static int CreateTagging(int wordid, int tagid)
        {
            TaggingDataModel data = new TaggingDataModel
            {
                WordId = wordid,
                TagId = tagid
            };

            string sql = @"INSERT INTO dbo.Taggings (WordId, TagId)
                           VALUES (@Name, @LanguageId);";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<TaggingDataModel> LoadTaggings()
        {
            string sql = @"SELECT Id, WordId, TagId FROM dbo.Taggings;";

            return SqlDataAccess.LoadData<TaggingDataModel>(sql);
        }
    }
}
