using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageMakerDataLibrary.DataModels;

namespace LanguageMakerDataLibrary.BusinessLogic
{
    /// <summary>
    /// Static class to be used for data access to the Words database table
    /// </summary>
    public static class WordProcessor
    {
        /// <summary>
        /// Method to add a word to the database
        /// </summary>
        /// <param name="text">Text of the word</param>
        /// <param name="languageid">Language Id associated with the word</param>
        /// <param name="classificationid">Classification Id of the classification of the word</param>
        /// <param name="description">Description of the word</param>
        /// <param name="pronnounciation">Pronounciation of the word</param>
        /// <returns>Returns the number of records changed</returns>
        public static int CreateWord(string text, int languageid, int classificationid, string description, string pronnounciation)
        {
            WordDataModel data = new WordDataModel
            {
                Text = text,
                LanguageId = languageid,
                ClassificationId = classificationid,
                Description = description,
                Pronounciation = pronnounciation
            };

            string sql = @"INSERT INTO dbo.Words (Text, LanguageId, ClassificationId, Description, Pronounciation)
                           VALUES (@Text, @LanguageId, @ClassificationId, @Description, @Pronounciation);";

            return SqlDataAccess.SaveData(sql, data);
        }

        /// <summary>
        /// Method to load all the words of a specific language as a list of Word Data Models
        /// </summary>
        /// <param name="languageid">Language Id associated with the word</param>
        /// <returns>Returns a list of WordDataModel objects</returns>
        public static List<WordDataModel> LoadWords(int languageid)
        {
            string sql = @"SELECT Id, Text, LanguageId, ClassficationId, Description, Pronounciation FROM dbo.Words
                           WHERE LanguageId = @LanguageId;";
            var parameters = new { LanguageId = languageid };

            return SqlDataAccess.LoadData<WordDataModel>(sql, parameters).ToList();
        }

        /// <summary>
        /// Method to determine the number of words of a specific language
        /// </summary>
        /// <param name="languageid">Language Id associated with the word</param>
        /// <returns>Returns the number of words of a specific language as an int</returns>
        public static int getWordsCount(int languageid)
        {
            string sql = "SELECT COUNT(Id) FROM dbo.Words WHERE LanguageId = " + languageid.ToString() + ";";

            return SqlDataAccess.getTableCount(sql);
        }
    }
}
