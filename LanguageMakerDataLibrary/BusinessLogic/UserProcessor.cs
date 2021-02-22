using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageMakerDataLibrary.DataModels;

namespace LanguageMakerDataLibrary.BusinessLogic
{
    /// <summary>
    /// Static class to be used for data access to the Users database table
    /// </summary>
    public static class UserProcessor
    {
        /// <summary>
        /// Method to add a user to the database
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Returns the number of records changed</returns>
        public static int CreateUser(string username)
        {
            UserDataModel data = new UserDataModel
            {
                UserName = username
            };

            string sql = @"INSERT INTO dbo.Users (Username)
                           VALUES (@Username);";

            return SqlDataAccess.SaveData(sql, data);
        }

        /// <summary>
        /// Method to load all the users
        /// </summary>
        /// <returns>Returns a list of UserDataModel objects</returns>
        public static List<UserDataModel> LoadUsers()
        {
            string sql = @"SELECT Id, Username FROM dbo.Users;";

            return SqlDataAccess.LoadData<UserDataModel>(sql).ToList();
        }

        /// <summary>
        /// Method to check if a username already exists
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Returns true if the username exists, otherwise false</returns>
        public static bool CheckUsername(string username)
        {
            string sql = @"SELECT Id FROM dbo.Users WHERE Username = @Username;";
            var parameters = new { Username = username };
            return SqlDataAccess.CheckTableDataFromParameters(sql, parameters);
        }

        /// <summary>
        /// Method to get the user id based on the username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Returns the user id of a username</returns>
        public static int getUserId(string username)
        {
            string sql = @"SELECT Id FROM dbo.Users WHERE Username = @Username;";
            var parameters = new { Username = username };

            return SqlDataAccess.GetFirstTableDataFromParameters<int>(sql, parameters);
        }
    }
}
