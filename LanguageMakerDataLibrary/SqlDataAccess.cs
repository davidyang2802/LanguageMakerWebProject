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
    public static class SqlDataAccess
    {
        public static string getConnectionString(string connectionName = "LanguageMakerDB")
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        public static List<T> LoadData<T>(string sql)
        {
            using (IDbConnection cnn = new SqlConnection(getConnectionString()))
            {
                return cnn.Query<T>(sql).ToList();
            }
        }

        public static List<T> LoadData<T>(string sql, object parameters)
        {
            using (IDbConnection cnn = new SqlConnection(getConnectionString()))
            {
                return cnn.Query<T>(sql, parameters).ToList();
            }
        }

        public static int SaveData<T>(string sql, T data)
        {
            using (IDbConnection cnn = new SqlConnection(getConnectionString()))
            {
                return cnn.Execute(sql, data);
            }
        }
        public static bool CheckTableDataFromColumn(string sql, object parameters)
        {
            using (IDbConnection cnn = new SqlConnection(getConnectionString()))
            {
                List<int> data = cnn.Query<int>(sql, parameters).ToList();

                if (data.Count > 0) { return true; }
                else { return false; }
            }
        }
        public static T GetFirstTableDataFromColumnValue<T>(string sql, object parameters)
        {
            using (IDbConnection cnn = new SqlConnection(getConnectionString()))
            {
                return cnn.QuerySingle<T>(sql, parameters);
            }
        }
    }
}
