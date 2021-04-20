using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageMakerDataLibrary.DataModels;

namespace LanguageMakerDataLibrary.BusinessLogic
{
    /// <summary>
    /// Static class to be used for data access to the Definitions database table
    /// </summary>
    public static class DefinitionProcessor
    {
        /// <summary>
        /// Method to add a definition to the database
        /// </summary>
        /// <param name="text">Text of the definition</param>
        /// <param name="wordid">Word Id associated with the definition</param>
        /// <returns>Returns the number of records changed</returns>
        public static int CreateDefinition(string text, int wordid, int languageid)
        {
            DefinitionDataModel data = new DefinitionDataModel
            {
                Text = text,
                WordId = wordid,
                LanguageId = languageid
            };

            string sql = @"INSERT INTO dbo.Definitions (Text, WordId, LanguageId)
                           VALUES (@Text, @WordId, @LanguageId);";

            SqlDataAccess.SaveData(sql, data);

            sql = "SELECT * FROM dbo.Definitions WHERE Text = @Text AND LanguageId = @LanguageId;";

            data = SqlDataAccess.GetFirstTableDataFromParameters<DefinitionDataModel>(sql, new { Text = text, LanguageId = languageid });

            return data.Id;
        }

        /// <summary>
        /// Method to load all the definitions of a specific language as a list of Definition Data Models
        /// </summary>
        /// <param name="wordid">Word Id associated with the definition</param>
        /// <returns>Returns a list of DefinitionDataModel objects</returns>
        public static List<DefinitionDataModel> LoadDefinitions(int wordid)
        {
            string sql = @"SELECT Id, Text, WordId, LanguageId FROM dbo.Definitions;";

            return SqlDataAccess.LoadData<DefinitionDataModel>(sql).ToList();
        }

        /// <summary>
        /// Method to delete a definition based on the id
        /// </summary>
        /// <param name="definitionid">Definition Id</param>
        /// <returns>Returns the number of records changed</returns>
        public static int DeleteDefinition(int definitionid)
        {
            string sql = "DELETE FROM dbo.Definitions WHERE Id = @DefinitionId;";

            return SqlDataAccess.UpdateData(sql, new { DefinitionId = definitionid });
        }
    }
}
