using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageMakerDataLibrary.DataModels;

namespace LanguageMakerDataLibrary.BusinessLogic
{
    /// <summary>
    /// Static class to be used for data access to the Classifications database table
    /// </summary>
    public static class ClassificationProcessor
    {
        /// <summary>
        /// Method to add a classification to the database
        /// </summary>
        /// <param name="name">Name of the classification</param>
        /// <param name="languageid">Language Id associated with the classification</param>
        /// <param name="description">Description of the classification</param>
        /// <returns>Returns the number of records changed</returns>
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

        /// <summary>
        /// Method to load all the classifications of a specific language as a list of Classification Data Models
        /// </summary>
        /// <param name="languageid">Language Id associated with the classification</param>
        /// <returns>Returns a list of ClassificationDataModel objects</returns>
        public static List<ClassificationDataModel> LoadClassifications(int languageid)
        {
            string sql = @"SELECT Id, Name, LanguageId, Description FROM dbo.Classifications WHERE LanguageId = @LanguageId;";
            var parameters = new { LanguageId = languageid };

            return SqlDataAccess.LoadData<ClassificationDataModel>(sql, parameters).ToList();
        }

        /// <summary>
        /// Method to determine the number of classifications of a specific language
        /// </summary>
        /// <param name="languageid">Language Id associated with the classification</param>
        /// <returns>Returns the number of classifications of a specific language as an int</returns>
        public static int getClassificationsCount(int languageid)
        {
            string sql = "SELECT COUNT(Id) FROM dbo.Classifications WHERE LanguageId = " + languageid.ToString() + ";";

            return SqlDataAccess.getTableCount(sql);
        }
    }
}
