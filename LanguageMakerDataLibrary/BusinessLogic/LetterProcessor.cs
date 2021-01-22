using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageMakerDataLibrary.DataModels;

namespace LanguageMakerDataLibrary.BusinessLogic
{
    public static class LetterProcessor
    {
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

        public static List<LetterDataModel> LoadLetters()
        {
            string sql = @"SELECT Id, Name, LanguageId, Pronounciation, Description FROM dbo.Letters;";

            return SqlDataAccess.LoadData<LetterDataModel>(sql).ToList();
        }
    }
}
