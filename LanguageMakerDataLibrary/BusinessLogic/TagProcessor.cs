using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageMakerDataLibrary.DataModels;

namespace LanguageMakerDataLibrary.BusinessLogic
{
    /// <summary>
    /// Static class to be used for data access to the Tags database table
    /// </summary>
    public static class TagProcessor
    {
        /// <summary>
        /// Method to add a tag to the database
        /// </summary>
        /// <param name="name">Name of the tag</param>
        /// <param name="languageid">Language Id associated with the tag</param>
        /// <param name="description">Description of the tag</param>
        /// <returns>Returns the Id of the new tag</returns>
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

            SqlDataAccess.SaveData(sql, data);

            sql = "SELECT * FROM dbo.Tags WHERE Name = @Name AND LanguageId = @LanguageId;";

            data = SqlDataAccess.GetFirstTableDataFromParameters<TagDataModel>(sql, new { Name = name, LanguageId = languageid });

            return data.Id;
        }

        /// <summary>
        /// Method to load all the tags of a specific language as a list of Tag Data Models
        /// </summary>
        /// <param name="languageid">Language Id associated with the tag</param>
        /// <returns>Returns a list of TagDataModel objects</returns>
        public static List<TagDataModel> LoadTags(int languageid)
        {
            string sql = @"SELECT Id, Name, LanguageId, Description FROM dbo.Tags WHERE LanguageId = @LanguageId;";
            var parameters = new { LanguageId = languageid };

            return SqlDataAccess.LoadData<TagDataModel>(sql, parameters).ToList();
        }

        /// <summary>
        /// Method to determine the number of tags of a specific language
        /// </summary>
        /// <param name="languageid">Language Id associated with the tag</param>
        /// <returns>Returns the number of tags of a specific language as an int</returns>
        public static int getTagsCount(int languageid)
        {
            string sql = "SELECT COUNT(Id) FROM dbo.Tags WHERE LanguageId = " + languageid.ToString() + ";";

            return SqlDataAccess.getTableCount(sql);
        }

        /// <summary>
        /// Method to delete a tag based on the id
        /// </summary>
        /// <param name="tagid">Tag Id</param>
        /// <returns>Returns the number of records changed</returns>
        public static int DeleteTag(int tagid)
        {
            string sql = "DELETE FROM dbo.Tags WHERE Id = @TagId;";

            return SqlDataAccess.UpdateData(sql, new { TagId = tagid });
        }
    }
}
