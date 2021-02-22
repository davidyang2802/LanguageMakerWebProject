using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageMakerDataLibrary.DataModels;

namespace LanguageMakerDataLibrary.BusinessLogic
{
    /// <summary>
    /// Static class to be used for data access to the Taggings database table
    /// </summary>
    public static class TaggingProcessor
    {
        /// <summary>
        /// Method to add a tagging to the database
        /// </summary>
        /// <param name="wordid">Word Id associated with the tagging</param>
        /// <param name="tagid">Tag Id associated with the tagging</param>
        /// <returns>Returns the number of records changed</returns>
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

            return SqlDataAccess.LoadData<TaggingDataModel>(sql).ToList();
        }
    }
}
