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
        /// <returns>Returns the Id of the new tagging</returns>
        public static int CreateTagging(int wordid, int tagid)
        {
            TaggingDataModel data = new TaggingDataModel
            {
                WordId = wordid,
                TagId = tagid
            };

            string sql = @"INSERT INTO dbo.Taggings (WordId, TagId)
                           VALUES (@Name, @LanguageId);";

            SqlDataAccess.SaveData(sql, data);

            sql = "SELECT * FROM dbo.Taggings WHERE WordId = @WordId AND TagId = @TagId;";

            data = SqlDataAccess.GetFirstTableDataFromParameters<TaggingDataModel>(sql, new { WordId = wordid, TagId = tagid });

            return data.Id;
        }

        public static List<TaggingDataModel> LoadTaggings()
        {
            string sql = @"SELECT Id, WordId, TagId FROM dbo.Taggings;";

            return SqlDataAccess.LoadData<TaggingDataModel>(sql).ToList();
        }

        /// <summary>
        /// Method to delete a tagging based on the word Id & tag Id
        /// </summary>
        /// <param name="wordid">Word Id</param>
        /// <param name="tagid">Tag Id</param>
        /// <returns>Returns the number of records changed</returns>
        public static int DeleteTagging(int wordid, int tagid)
        {
            string sql = "DELETE FROM dbo.Taggins WHERE WordId = @WordId AND TagId = @TagId;";

            return SqlDataAccess.UpdateData(sql, new { WordId = wordid, TagId = tagid });
        }
    }
}
