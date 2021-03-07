using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageMakerDataLibrary.DataModels;

namespace LanguageMakerDataLibrary.BusinessLogic
{
    /// <summary>
    /// Static class to be used for data access to the LetterTypes database table
    /// </summary>
    public static class LetterTypeProcessor
    {
        /// <summary>
        /// Method to add a letter type to the database
        /// </summary>
        /// <param name="name">Name of the letter type</param>
        /// <param name="languageid">Langauge Id associated with the letter type</param>
        /// <param name="description">Description of the letter type</param>
        /// <param name="pattern">Pattern of the letter type</param>
        /// <returns>Returns the number of records changed</returns>
        public static int CreateLetterType(string name, int languageid, string description, char pattern)
        {
            LetterTypeDataModel data = new LetterTypeDataModel
            {
                Name = name,
                LanguageId = languageid,
                Description = description,
                Pattern = pattern
            };

            string sql = @"INSERT INTO dbo.LetterTypes (Name, LanguageId, Description, Pattern)
                           VALUES (@Name, @LanguageId, @Description, @Pattern);";

            return SqlDataAccess.SaveData(sql, data);
        }

        /// <summary>
        /// Method to load all the letter types of a specific language as a list of Letter Type Data Models
        /// </summary>
        /// <param name="languageid">Langauge Id associated with the letter type</param>
        /// <returns>Returns a list of the LetterTypeDataModel objects</returns>
        public static List<LetterTypeDataModel> LoadLetterTypes(int languageid)
        {
            string sql = @"SELECT Id, Name, LanguageId, Description, Pattern FROM dbo.LetterTypes WHERE LanguageId = @LanguageId;";
            var parameters = new { LanguageId = languageid };

            List<LetterTypeDataModel> lettertypes = SqlDataAccess.LoadData<LetterTypeDataModel>(sql, parameters).ToList();
            foreach (LetterTypeDataModel lt in lettertypes)
            {
                lt.Name = lt.Name.TrimEnd();
            }

            return lettertypes;
        }

        /// <summary>
        /// Method to determine the number of letter types of a specific language
        /// </summary>
        /// <param name="languageid">Langauge Id associated with the letter type</param>
        /// <returns>Returns the number of letter types of a specific language as an int</returns>
        public static int getLetterTypesCount(int languageid)
        {
            string sql = "SELECT COUNT(Id) FROM dbo.LetterTypes WHERE LanguageId = " + languageid.ToString() + ";";

            return SqlDataAccess.getTableCount(sql);
        }

        /// <summary>
        /// Method to delete a letter type based on the id
        /// </summary>
        /// <param name="lettertypeid">Letter type Id</param>
        /// <returns>Returns the number of records changed</returns>
        public static int DeleteLetterType(int lettertypeid)
        {
            string sql = "DELETE FROM dbo.LetterTypes WHERE Id = @LetterTypeId;";

            return SqlDataAccess.UpdateData(sql, new { LetterTypeId = lettertypeid });
        }
    }
}
