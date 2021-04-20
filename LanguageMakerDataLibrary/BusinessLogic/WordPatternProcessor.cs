using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageMakerDataLibrary.DataModels;

namespace LanguageMakerDataLibrary.BusinessLogic
{
    /// <summary>
    /// Static class to be used for data access to the WordPatterns database table
    /// </summary>
    public static class WordPatternProcessor
    {
        /// <summary>
        /// Method to add a word pattern to the database
        /// </summary>
        /// <param name="name">Name of the word pattern</param>
        /// <param name="pattern">The word pattern</param>
        /// <param name="languageid">Language Id associated with the word pattern</param>
        /// <returns>Returns the id of the new word pattern</returns>
        public static int CreateWordPattern(string name, string pattern, int languageid)
        {
            WordPatternDataModel data = new WordPatternDataModel
            {
                Name = name,
                Pattern = pattern,
                LanguageId = languageid
            };

            string sql = @"INSERT INTO dbo.WordPatterns (Name, Pattern, LanguageId)
                           VALUES (@Name, @Pattern, @LanguageId);";

            SqlDataAccess.SaveData(sql, data);

            sql = "SELECT * FROM dbo.WordPatterns WHERE Name = @Name AND LanguageId = @LanguageId;";

            data = SqlDataAccess.GetFirstTableDataFromParameters<WordPatternDataModel>(sql, new { Name = name, LanguageId = languageid });

            return data.Id;
        }

        /// <summary>
        /// Method to load all the word patterns of a specific language as a list of Word Pattern Data Models
        /// </summary>
        /// <param name="languageid">Language Id associated with the word pattern</param>
        /// <returns>Return a list of WordPatternDataModel objects</returns>
        public static List<WordPatternDataModel> LoadWordPatterns(int languageid)
        {
            string sql = @"SELECT Id, Name, Pattern, LanguageId FROM dbo.WordPatterns WHERE LanguageId = @LanguageId;";
            var parameters = new { LanguageId = languageid };

            return SqlDataAccess.LoadData<WordPatternDataModel>(sql, parameters).ToList();
        }

        /// <summary>
        /// Method to determine the number of word patterns of a specific language
        /// </summary>
        /// <param name="languageid">Language Id associated with the word pattern</param>
        /// <returns>Returns the number of word patterns of a specific language as an int</returns>
        public static int getWordPatternsCount(int languageid)
        {
            string sql = "SELECT COUNT(Id) FROM dbo.WordPatterns WHERE LanguageId = " + languageid.ToString() + ";";

            return SqlDataAccess.getTableCount(sql);
        }

        /// <summary>
        /// Method to delete a word pattern based on the id
        /// </summary>
        /// <param name="wordpatternid">Word pattern Id</param>
        /// <returns>Returns the number of records changed</returns>
        public static int DeleteWordPattern(int wordpatternid)
        {
            string sql = "DELETE FROM dbo.WordPatterns WHERE Id = @WordPatternId;";

            return SqlDataAccess.UpdateData(sql, new { WordPatternId = wordpatternid });
        }
    }
}
