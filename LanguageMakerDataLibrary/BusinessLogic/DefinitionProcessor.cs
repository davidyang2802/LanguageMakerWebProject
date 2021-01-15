using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageMakerDataLibrary.DataModels;

namespace LanguageMakerDataLibrary.BusinessLogic
{
    public static class DefinitionProcessor
    {
        public static int CreateDefinition(string text, int wordid)
        {
            DefinitionDataModel data = new DefinitionDataModel
            {
                Text = text,
                WordId = wordid
            };

            string sql = @"INSERT INTO dbo.Definitions (Text, WordId)
                           VALUES (@Text, @WordId);";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<DefinitionDataModel> LoadDefinitions()
        {
            string sql = @"SELECT Id, Text, WordId FROM dbo.Definitions;";

            return SqlDataAccess.LoadData<DefinitionDataModel>(sql);
        }
    }
}
