using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageMakerDataLibrary.DataModels;

namespace LanguageMakerDataLibrary.BusinessLogic
{
    public static class WordProcessor
    {
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

        public static List<WordDataModel> LoadWords(int languageid)
        {
            string sql = @"SELECT Id, Text, LanguageId, ClassficationId, Description, Pronounciation FROM dbo.Words
                           WHERE LanguageId = @LanguageId;";
            var parameters = new { LanguageId = languageid };

            return SqlDataAccess.LoadData<WordDataModel>(sql, parameters).ToList();
        }

        public static int getWordsCount(int languageid)
        {
            string sql = "SELECT COUNT(Id) FROM dbo.Words WHERE LanguageId = " + languageid.ToString() + ";";

            return SqlDataAccess.getTableCount(sql);
        }
    }
}
