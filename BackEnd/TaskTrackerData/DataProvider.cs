using System;
using System.Data.SqlClient;

namespace TaskTrackerData
{
    public class DataProvider
    {

        #region Variables
        private string _dbConnectionString;
        public SqlConnection SqlConnection { get { return new SqlConnection(_dbConnectionString); } }
        #endregion

        public DataProvider(string dbConnectionString)
        {
            _dbConnectionString = dbConnectionString;
        }

        public static SqlParameter CreateParameter(string paramName, string paramValue, bool useNullForEmptyString = false)
        {
            if (paramValue == null || String.IsNullOrEmpty(paramValue) && useNullForEmptyString)
                return new SqlParameter(paramName, DBNull.Value);
            else
                return new SqlParameter(paramName, paramValue);
        }

        public static SqlParameter CreateParameter(string paramName, object paramValue)
        {
            return new SqlParameter(paramName, paramValue ?? DBNull.Value);
        }

        public static SqlParameter CreateOutIntParameter(string paramName)
        {
            return new SqlParameter(paramName, System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
        }

        public static SqlParameter CreateOutStringParameter(string paramName, int charLength = 25)
        {
            return new SqlParameter(paramName, System.Data.SqlDbType.NChar)
            {
                Direction = System.Data.ParameterDirection.Output,
                Size = charLength
            };
        }


    }
}
