﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageMakerDataLibrary.DataModels;

namespace LanguageMakerDataLibrary.BusinessLogic
{
    /// <summary>
    /// Static class to be used for data access to the Classifications database table
    /// </summary>
    public static class ClassificationProcessor
    {
        /// <summary>
        /// Method to add a classification to the database
        /// </summary>
        /// <param name="name">Name of the classification</param>
        /// <param name="languageid">Language Id associated with the classification</param>
        /// <param name="description">Description of the classification</param>
        /// <returns>Returns the Id of the new classification</returns>
        public static int CreateClassification(string name, int languageid, string description)
        {
            ClassificationDataModel data = new ClassificationDataModel
            {
                Name = name,
                LanguageId = languageid,
                Description = description
            };

            string sql = @"INSERT INTO dbo.Classifications (Name, LanguageId, Description)
                           VALUES (@Name, @LanguageId, @Description);";

            SqlDataAccess.SaveData(sql, data);

            sql = @"SELECT * FROM dbo.Classification WHERE Name = @Name AND LanguageId = @LanguageId;";

            data = SqlDataAccess.GetFirstTableDataFromParameters<ClassificationDataModel>(sql, new { Name = name, LanguageId = languageid });

            return data.Id;
        }

        /// <summary>
        /// Method to load all the classifications of a specific language as a list of Classification Data Models
        /// </summary>
        /// <param name="languageid">Language Id associated with the classification</param>
        /// <returns>Returns a list of ClassificationDataModel objects</returns>
        public static List<ClassificationDataModel> LoadClassifications(int languageid)
        {
            string sql = @"SELECT Id, Name, LanguageId, Description FROM dbo.Classifications WHERE LanguageId = @LanguageId;";
            var parameters = new { LanguageId = languageid };

            return SqlDataAccess.LoadData<ClassificationDataModel>(sql, parameters).ToList();
        }

        /// <summary>
        /// Method to determine the number of classifications of a specific language
        /// </summary>
        /// <param name="languageid">Language Id associated with the classification</param>
        /// <returns>Returns the number of classifications of a specific language as an int</returns>
        public static int getClassificationsCount(int languageid)
        {
            string sql = "SELECT COUNT(Id) FROM dbo.Classifications WHERE LanguageId = " + languageid.ToString() + ";";

            return SqlDataAccess.getTableCount(sql);
        }

        /// <summary>
        /// Method to delete a classification based on the id
        /// </summary>
        /// <param name="classificationid">Classification Id</param>
        /// <returns>Returns the number of records changed</returns>
        public static int DeleteClassification(int classificationid)
        {
            string sql = "DELETE FROM dbo.Classifications WHERE Id = @ClassificationId;";

            return SqlDataAccess.UpdateData(sql, new { ClassificationId = classificationid });
        }
    }
}