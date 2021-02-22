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
    /// <summary>
    /// Static class to be used for data access to the Languages database table
    /// </summary>
    public static class LanguageProcessor
    {
        /// <summary>
        /// Method to add a language to the database
        /// </summary>
        /// <param name="name">Name of the language</param>
        /// <param name="userid">User Id associated with the language</param>
        /// <param name="description">Description of the language</param>
        /// <returns>Returns the number of records changed</returns>
        public static int CreateLanguage(string name, int userid, string description)
        {
            LanguageDataModel data = new LanguageDataModel
            {
                Name = name,
                UserId = userid,
                Description = description
            };

            string sql = @"INSERT INTO dbo.Languages (Name, UserId, Description)
                           VALUES (@Name, @UserId, @Description);";

            return SqlDataAccess.SaveData(sql, data);
        }

        /// <summary>
        /// Method to load all the languages of a specific user as a list of Language Data Models
        /// </summary>
        /// <param name="userid">User Id associated with the language</param>
        /// <returns>Returns a list of LanguageDataModel objects</returns>
        public static List<LanguageDataModel> LoadLanguages(int userid)
        {
            string sql = @"SELECT Id, Name, UserId, Description FROM dbo.Languages WHERE UserId = @UserId";

            var parameters = new { UserId = userid };

            return SqlDataAccess.LoadData<LanguageDataModel>(sql, parameters).ToList();
        }

        /// <summary>
        /// Method to check if a language name exists for a specific user id
        /// </summary>
        /// <param name="userid">User Id associated with the language</param>
        /// <param name="name">Name of the language</param>
        /// <returns>Returns true if the language name exists for the user id, otherwise false</returns>
        public static bool CheckDistinct(int userid, string name)
        {
            string sql = @"SELECT Id FROM dbo.Languages WHERE Userid = @UserId AND Name = @Name;";
            var parameters = new { UserId = userid, Name = name };
            return SqlDataAccess.CheckTableDataFromParameters(sql, parameters);
        }

        /// <summary>
        /// Method to get the language id based on the user id and language name
        /// </summary>
        /// <param name="userid">User Id associated with the language</param>
        /// <param name="name">Name of the language</param>
        /// <returns>Returns the language id</returns>
        public static int getLanguageId(int userid, string name)
        {
            string sql = @"SELECT Id FROM dbo.Languages WHERE Userid = @UserId AND Name = @Name;";
            var parameters = new { UserId = userid, Name = name };

            return SqlDataAccess.GetFirstTableDataFromParameters<int>(sql, parameters);
        }

        /// <summary>
        /// Method to determine the number of languages associated with a user id
        /// </summary>
        /// <param name="userid">User Id associated with the language</param>
        /// <returns>Returns the number of languages associated with the user id as an int</returns>
        public static int getLanguagesCount(int userid)
        {
            string sql = "SELECT COUNT(Id) FROM dbo.Languages WHERE UserId = " + userid.ToString() + ";";

            return SqlDataAccess.getTableCount(sql);
        }
    }
}
