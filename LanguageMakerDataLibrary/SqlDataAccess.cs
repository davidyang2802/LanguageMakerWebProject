using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace LanguageMakerDataLibrary
{
    /// <summary>
    /// Static class to be used by Business Logic processor classes
    /// Contains methods to connect to SQL data
    /// Note that methods require a sql string
    /// </summary>
    public static class SqlDataAccess
    {
        /// <summary>
        /// Method to get the connection string to the SQL database
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public static string getConnectionString(string connectionName = "LanguageMakerDB")
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        /// <summary>
        /// Method to load data from the database, based on the SQL statement passed as sql and returning a list of type T as specified
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<T> LoadData<T>(string sql)
        {
            using (IDbConnection cnn = new SqlConnection(getConnectionString()))
            {
                return cnn.Query<T>(sql).ToList();
            }
        }

        /// <summary>
        /// Method to load data from the database, based on the SQL statement passed as sql with parameters passed as an object
        /// Returns a list of type T as specified
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static List<T> LoadData<T>(string sql, object parameters)
        {
            using (IDbConnection cnn = new SqlConnection(getConnectionString()))
            {
                return cnn.Query<T>(sql, parameters).ToList();
            }
        }

        /// <summary>
        /// Method to save data into the database, based on the SQL statement passed as sql and data of type T as specified
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int SaveData<T>(string sql, T data)
        {
            using (IDbConnection cnn = new SqlConnection(getConnectionString()))
            {
                return cnn.Execute(sql, data);
            }
        }

        /// <summary>
        /// Method to determine the table count, based on the SQL statement passed as sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int getTableCount(string sql)
        {
            using (IDbConnection cnn = new SqlConnection(getConnectionString()))
            {
                //return Int32.Parse(cnn.Query(sql).ToString());
                return cnn.QuerySingle<int>(sql);
            }
        }

        /// <summary>
        /// Method to determine if table data exists, based on the SQL statement passed as sql and parameters passed as an object
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static bool CheckTableDataFromParameters(string sql, object parameters)
        {
            using (IDbConnection cnn = new SqlConnection(getConnectionString()))
            {
                List<int> data = cnn.Query<int>(sql, parameters).ToList();

                if (data.Count > 0) { return true; }
                else { return false; }
            }
        }

        /// <summary>
        /// Method to get a single table data row as type T as specified, based on the SQL statement passed as sql and parameters passed as an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T GetFirstTableDataFromParameters<T>(string sql, object parameters)
        {
            using (IDbConnection cnn = new SqlConnection(getConnectionString()))
            {
                return cnn.QuerySingle<T>(sql, parameters);
            }
        }
    }
}
