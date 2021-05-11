using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace TaskTrackerData.Model.Base
{
    public abstract class SQLinterface
    {
        #region Parameters
        protected string SqlSpLoad { get; }
        protected string SqlSpLoadJoin { get; }
        protected string SqlSpDelete { get; }
        protected string SqlSpUpdate { get; }
        protected string SqlSpInsert { get; }
        protected SqlConnection SqlConnection { get { return ApplicationContext.GetSqlConnection(); } }
        protected string SqlPkParameterName { get; }

        protected ApplicationContext ApplicationContext { get; }
        #endregion


        protected SQLinterface(ApplicationContext applicationContext, string sqlPkParameterName,
            string sqlSpLoad, string sqlSpUpdate, string sqlSpInsert, string sqlSpDelete)
        {
            SqlSpLoad = sqlSpLoad;
            SqlSpUpdate = sqlSpUpdate;
            SqlSpDelete = sqlSpDelete;
            SqlSpInsert = sqlSpInsert;
            SqlPkParameterName = sqlPkParameterName;
            ApplicationContext = applicationContext;
        }




        protected Dictionary<string, string> GetProcedureParams(string sqlProcName)
        {
            var sqlParams = new Dictionary<string, string>();
            var sql = GetSqlProcedureParamListQuery(sqlProcName);

            using (var conn = SqlConnection)
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.HasRows)
                        {
                            rdr.Read();
                            sqlParams.Add(rdr[0].ToString(), rdr[1].ToString());
                        }
                    }
                }
            }

            return sqlParams;
        }

        protected async Task<Dictionary<string, string>> GetProcedureParamsAsync(string sqlProcName)
        {
            var sqlParams = new Dictionary<string, string>();
            var sql = GetSqlProcedureParamListQuery(sqlProcName);

            using (var conn = SqlConnection)
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    var rdr = await cmd.ExecuteReaderAsync();
                    while (rdr.HasRows)
                    {
                        await rdr.ReadAsync();
                        sqlParams.Add(rdr[0].ToString(), rdr[1].ToString());
                    }
                }
            }

            return sqlParams;
        }

        private string GetSqlProcedureParamListQuery(string sqlProcName)
        {
            return @"select parameters.name as param_name, types.name as data_type
                    from sys.parameters
                    inner join sys.procedures on parameters.object_id = procedures.object_id
                    inner join sys.types on parameters.system_type_id = types.system_type_id
                        AND parameters.user_type_id = types.user_type_id
                    where procedures.name = '" + sqlProcName + "'";
        }



    }

}
