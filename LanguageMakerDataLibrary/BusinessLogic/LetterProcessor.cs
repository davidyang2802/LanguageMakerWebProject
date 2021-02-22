using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageMakerDataLibrary.DataModels;

namespace LanguageMakerDataLibrary.BusinessLogic
{
    /// <summary>
    /// Static class to be used for data access to the Letters database table
    /// </summary>
    public static class LetterProcessor
    {
        /// <summary>
        /// Method to add a letter to the database
        /// </summary>
        /// <param name="name">Name of the letter</param>
        /// <param name="languageid">Language Id associated with the letter</param>
        /// <param name="pronounciation">Pronounciation of the letter</param>
        /// <param name="description">Description of the letter</param>
        /// <returns>Returns the number of records changed</returns>
        public static int CreateLetter(string name, int languageid, string pronounciation, string description)
        {
            LetterDataModel data = new LetterDataModel
            {
                Name = name,
                LanguageId = languageid,
                Pronounciation = pronounciation,
                Description = description
            };

            string sql = @"INSERT INTO dbo.Letters (Name, LanguageId, Pronounciation, Description)
                           VALUES (@Name, @LanguageId, @Pronounciation, @Description);";

            return SqlDataAccess.SaveData(sql, data);
        }

        /// <summary>
        /// Method to load all the letters of a specific langauge as a list of Letter Data Models
        /// </summary>
        /// <param name="languageid">Language Id associated with the letter</param>
        /// <returns>Returns a list of LetterDataModel objects</returns>
        public static List<LetterDataModel> LoadLetters(int languageid)
        {
            string sql = @"SELECT Id, Name, LanguageId, Pronounciation, Description FROM dbo.Letters WHERE LanguageId = @LanguageId;";
            var parameters = new { LangaugeId = languageid };

            return SqlDataAccess.LoadData<LetterDataModel>(sql, parameters).ToList();
        }

        /// <summary>
        /// Method to determine the number of letters of a specific language
        /// </summary>
        /// <param name="languageid">Language Id associated with the letter</param>
        /// <returns>Returns the number of letters of a specific language as an int</returns>
        public static int getLettersCount(int languageid)
        {
            string sql = "SELECT COUNT(Id) FROM dbo.Letters WHERE LanguageId = " + languageid.ToString() + ";";

            return SqlDataAccess.getTableCount(sql);
        }
    }
}
