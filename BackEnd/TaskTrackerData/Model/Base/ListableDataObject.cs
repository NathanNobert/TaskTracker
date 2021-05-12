using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TaskTrackerData.Model.Base
{
    public abstract class ListableDataObject : DataObject
    {
        protected ListableDataObject(ApplicationContext applicationContext,
            string sqlPkParameterName, string sqlSpLoad, string sqlSpUpdate, string sqlSpInsert, string sqlSpDelete)
            : base(applicationContext, sqlPkParameterName, sqlSpLoad, sqlSpUpdate, sqlSpInsert, sqlSpDelete)
        { }

        protected abstract T GetNewInstance<T>() where T : DataObject;

        protected virtual List<T> GetList<T>() where T : ListableDataObject
        {
            return GetNewList<T>(null);
        }

        protected virtual List<T> GetListFromQuery<T>(string queryString) where T : ListableDataObject
        {
            return GetNewListFromQuery<T>(queryString);
        }

        protected List<T> GetList<T>(List<SqlParameter> queryParams, string spName = "") where T : ListableDataObject
        {
            return GetNewList<T>(queryParams, spName);
        }

        protected virtual Task<List<T>> GetListAsync<T>() where T : ListableDataObject
        {
            return GetNewListAsync<T>(null, "");
        }

        protected Task<List<T>> GetListAsync<T>(List<SqlParameter> queryParams, string spName = "") where T : ListableDataObject
        {
            return GetNewListAsync<T>(queryParams, spName);
        }

        private List<T> GetNewList<T>(List<SqlParameter> queryParams, string spName = "")
            where T : ListableDataObject
        {
            var newList = new List<T>();


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
                        while (rdr.Read())
                        {
                            newList.Add(GetNewInstance<T>());
                            newList[newList.Count - 1].Populate(rdr);
                        }
                    }
                }

            }
            return newList;
        }

        private List<T> GetNewListFromQuery<T>(string queryString = "")
            where T : ListableDataObject
        {
            var newList = new List<T>();

            using (var conn = SqlConnection)
            {
                conn.Open();

                using var cmd = new SqlCommand(queryString, conn);
                using var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    newList.Add(GetNewInstance<T>());
                    newList[newList.Count - 1].Populate(rdr);
                }

            }
            return newList;
        }

        private async Task<List<T>> GetNewListAsync<T>(List<SqlParameter> queryParams, string spName = "")
            where T : ListableDataObject
        {
            var newList = new List<T>();

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

                    while (await rdr.ReadAsync())
                    {
                        newList.Add(GetNewInstance<T>());
                        newList[newList.Count - 1].Populate(rdr);
                    }
                }

            }
            return newList;
        }
    }

}
