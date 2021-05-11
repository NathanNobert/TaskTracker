using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
namespace TaskTrackerData.Model.Base
{
    public abstract class DataObject : SQLinterface
    {
        protected DataObject(ApplicationContext applicationContext,
            string sqlPkParameterName, string sqlSpLoad, string sqlSpUpdate, string sqlSpInsert, string sqlSpDelete)
            : base(applicationContext, sqlPkParameterName, sqlSpLoad, sqlSpUpdate, sqlSpInsert, sqlSpDelete)
        { }


        protected abstract int GetPk();

        /// <summary>
        /// Santize the nullable date because SQL can only handle dates inside a certain range, and sometimes 
        /// the client returns '0001-01-01' instead of null so in that case set the date to null
        /// </summary>
        /// <param name="date">Date to be stored to SQL</param>
        /// <returns></returns>
        protected DateTime? NullableDate(DateTime? date)
        {
            return date == DateTime.MinValue ? null : date;
        }

        protected void Delete(int pk = 0, string pkParameterName = "")
        {
            if (pk == 0)
                pk = GetPk();

            using (var conn = SqlConnection)
            {
                conn.Open();
                using (var cmd = new SqlCommand(SqlSpDelete, conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add(new SqlParameter(
                        !String.IsNullOrEmpty(pkParameterName)
                            ? pkParameterName
                            : SqlPkParameterName,
                        pk));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected Task DeleteAsync(int pk = 0, string pkParameterName = "")
        {
            if (pk == 0)
                pk = GetPk();
            return DeleteRecordAsync(pk, pkParameterName);
        }

        private async Task DeleteRecordAsync(int pk, string pkParameterName = "")
        {
            using (var conn = SqlConnection)
            {
                using (var cmd = new SqlCommand(SqlSpDelete, conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add(new SqlParameter(
                        !String.IsNullOrEmpty(pkParameterName)
                            ? pkParameterName
                            : SqlPkParameterName,
                        pk));
                    await cmd.Connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        protected void Load(List<SqlParameter> queryParams, string spName = "")
        {
            if (String.IsNullOrEmpty(spName))
                spName = SqlSpLoad;

            using (var conn = SqlConnection)
            {
                conn.Open();
                using (var cmd = new SqlCommand(spName, conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                })
                {
                    if (queryParams != null)
                        cmd.Parameters.AddRange(queryParams.ToArray());

                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            rdr.Read();
                            Populate(rdr);
                        }
                    }
                }
            }
        }

        protected void Load(int pk, string spName = "", string pkParamName = "")
        {
            if (String.IsNullOrEmpty(spName))
                spName = SqlSpLoad;

            using (var conn = SqlConnection)
            {
                conn.Open();
                using (var cmd = new SqlCommand(spName, conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add(new SqlParameter(
                        !String.IsNullOrEmpty(pkParamName) ? pkParamName : SqlPkParameterName,
                        pk));
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            rdr.Read();
                            Populate(rdr);
                        }
                    }
                }
            }
        }

        protected void LoadSelectQuery(string queryString = "")
        {
            using (var conn = SqlConnection)
            {
                conn.Open();
                using (var cmd = new SqlCommand(queryString, conn))
                {
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            rdr.Read();
                            Populate(rdr);
                        }
                    }
                }
            }
        }

        protected async Task LoadAsync(List<SqlParameter> queryParams, string spName = "")
        {
            if (String.IsNullOrEmpty(spName))
                spName = SqlSpLoad;

            using (var conn = SqlConnection)
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(spName, conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                })
                {
                    if (queryParams != null)
                        cmd.Parameters.AddRange(queryParams.ToArray());

                    var rdr = await cmd.ExecuteReaderAsync();

                    if (rdr.HasRows)
                    {
                        await rdr.ReadAsync();
                        Populate(rdr);
                    }
                }
            }
        }

        protected async Task LoadAsync(int pk)
        {
            using (var conn = SqlConnection)
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(SqlSpLoad, conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add(new SqlParameter(SqlPkParameterName, pk));
                    var rdr = await cmd.ExecuteReaderAsync();

                    if (rdr.HasRows)
                    {
                        await rdr.ReadAsync();
                        Populate(rdr);
                    }
                }
            }
        }


        public virtual void Update()
        {
            var sqlParams = GetAllUpdateParameters();

            using (var conn = SqlConnection)
            {
                conn.Open();

                string storedProcName = GetPk() > 0 ? SqlSpUpdate : SqlSpInsert;

                using (var cmd = new SqlCommand(storedProcName, conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.AddRange(sqlParams.ToArray());
                    cmd.ExecuteNonQuery();


                    var outParam = sqlParams.FirstOrDefault(x => x.Direction == System.Data.ParameterDirection.Output);
                    if (outParam != null)
                        SetIdentityValue(outParam.Value as int? ?? default(int));
                    else
                        SetIdentityValue((int)sqlParams[0].Value);
                }
            }
        }

        public virtual async Task UpdateAsync()
        {
            var sqlParams = GetAllUpdateParameters();

            using (var conn = SqlConnection)
            {
                string storedProcName = GetPk() > 0 ? SqlSpUpdate : SqlSpInsert;

                using (var cmd = new SqlCommand(storedProcName, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(sqlParams.ToArray());

                    await cmd.Connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    var outParam = sqlParams.FirstOrDefault(x => x.Direction == System.Data.ParameterDirection.Output);
                    if (outParam != null)
                        SetIdentityValue(outParam.Value as int? ?? default(int));
                    else
                        SetIdentityValue((int)sqlParams[0].Value);
                }
            }
        }

        private List<SqlParameter> GetAllUpdateParameters()
        {
            var sqlParams = new List<SqlParameter>();
            sqlParams.AddRange(GetUpdateParams());

            return sqlParams;
        }

        protected abstract void Populate(SqlDataReader dataReader);
        protected abstract List<SqlParameter> GetUpdateParams();
        protected abstract void SetIdentityValue(int pkId);
    }

}
