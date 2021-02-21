﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageMakerDataLibrary.DataModels;

namespace LanguageMakerDataLibrary.BusinessLogic
{
    public static class WordPatternProcessor
    {
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

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<WordPatternDataModel> LoadWordPatterns(int languageid)
        {
            string sql = @"SELECT Id, Name, Pattern, LanguageId FROM dbo.WordPatterns WHERE LanguageId = @LanguageId;";
            var parameters = new { LanguageId = languageid };

            return SqlDataAccess.LoadData<WordPatternDataModel>(sql, parameters).ToList();
        }

        public static int getWordPatternsCount(int languageid)
        {
            string sql = "SELECT COUNT(Id) FROM dbo.WordPatterns WHERE LanguageId = " + languageid.ToString() + ";";

            return SqlDataAccess.getTableCount(sql);
        }
    }
}
