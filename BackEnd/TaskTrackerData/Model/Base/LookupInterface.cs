using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TaskTrackerData.Model.Base
{
    public class LookupInterface : SQLinterface
    {
        #region properties
        protected string TablePrefix { get; }
        #endregion

        #region constructor
        public LookupInterface(ApplicationContext applicationContext, string sqlTablePrefix,
            string sqlSpNameLoad, string sqlSpNameUpdate, string sqlSpNameInsert, string sqlSpNameDelete)
            : base(applicationContext, sqlTablePrefix + "_ID", sqlSpNameLoad,
                sqlSpNameUpdate, sqlSpNameInsert, sqlSpNameDelete)
        {
            TablePrefix = sqlTablePrefix;
        }
        #endregion

        // Return a list of LookupItems
        public List<LookupItem> GetList<T>(Dictionary<string, object> storedProcParams, bool includeInactive, string spName = "")
        {
            return GetListAsync<T>(storedProcParams, includeInactive, spName).Result;
        }

        public async Task<List<LookupItem>> GetListAsync<T>(Dictionary<string, object> storedProcParams, bool includeInactive, string spName = "")
        {
            var items = new List<LookupItem>();

            if (typeof(Lookup) == typeof(T) && String.IsNullOrEmpty(spName))
                spName = SqlSpLoad;
            else if (String.IsNullOrEmpty(spName))
                spName = SqlSpLoad;

            using (var conn = SqlConnection)
            {
                conn.Open();
                using (var cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (storedProcParams != null)
                    {
                        foreach (string spParam in storedProcParams.Keys)
                        {
                            cmd.Parameters.AddWithValue(spParam, storedProcParams[spParam]);
                        }
                    }

                    using (var rdr = await cmd.ExecuteReaderAsync())
                    {
                        while (await rdr.ReadAsync())
                        {
                            var item = new LookupItem();
                            Populate(rdr, item);

                            // prevent adding items with an ID that already exists
                            if (!items.Any(x => x.Id == item.Id))
                            {
                                // only include inactive fields in includeInactive is true
                                if (includeInactive || (!includeInactive && item.Active == true))
                                {
                                    items.Add(item);
                                }
                            }
                        }
                    }
                }
            }
            return items;
        }


        public LookupItem GetItem(string code = "", int id = 0)
        {
            var item = new LookupItem();
            if (String.IsNullOrEmpty(code) && id == 0)
                return item;

            using (var conn = SqlConnection)
            {
                conn.Open();
                using (var cmd = new SqlCommand(SqlSpLoad, conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add(new SqlParameter(null, id > 0 ? id : (int?)null));
                    //parameters.add sets default parameter name to ParameterN, so clear it for unnamed param
                    cmd.Parameters[0].ParameterName = null;
                    cmd.Parameters.Add(new SqlParameter(null, String.IsNullOrEmpty(code) ? null : code));
                    cmd.Parameters[1].ParameterName = null;
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Populate(rdr, item);
                        }
                    }
                }
            }
            return item;
        }

        public async Task<LookupItem> GetItemAsync(string code = "", int id = 0)
        {
            var item = new LookupItem();
            if (String.IsNullOrEmpty(code) && id == 0)
                return item;

            using (var conn = SqlConnection)
            {
                conn.Open();
                using (var cmd = new SqlCommand(SqlSpLoad, conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add(new SqlParameter(null, id > 0 ? id : (int?)null));
                    //parameters.add sets default parameter name to ParameterN, so clear it for unnamed param
                    cmd.Parameters[0].ParameterName = null;
                    cmd.Parameters.Add(new SqlParameter(null, String.IsNullOrEmpty(code) ? null : code));
                    cmd.Parameters[1].ParameterName = null;
                    using (var rdr = await cmd.ExecuteReaderAsync())
                    {
                        while (rdr.Read())
                        {
                            Populate(rdr, item);
                        }
                    }
                }
            }
            return item;
        }


        protected virtual void Populate(SqlDataReader rdr, LookupItem item)
        {
            var fieldNames = Enumerable.Range(0, rdr.FieldCount).Select(i => rdr.GetName(i)).ToArray();
            item.Id = rdr.GetSafeInt(TablePrefix + "ID");
            item.Code = rdr.GetSafeStringNotNull(TablePrefix + "CODE");
            item.Description = rdr.GetSafeStringNotNull(TablePrefix + "DESC");

            if (fieldNames.Contains(TablePrefix + "ACTIVE"))
                item.Active = rdr.GetSafeBool(TablePrefix + "ACTIVE");
            else
                item.Active = true;

        }

        protected List<SqlParameter> GetUpdateParams(LookupItem item)
        {
            if (item.SortOrder == null)
            {
                item.SortOrder = 0;
            }

            var sqlParams = new List<SqlParameter>();

            if (item.Id != 0)
                sqlParams.Add(DataProvider.CreateParameter("@p_" + TablePrefix.ToLower() + "id", item.Id));

            sqlParams.AddRange(new SqlParameter[] {
                DataProvider.CreateParameter("@p_" + TablePrefix.ToLower() + "code", item.Code),
                DataProvider.CreateParameter("@p_" + TablePrefix.ToLower() + "desc", item.Description),
                DataProvider.CreateParameter("@p_" + TablePrefix.ToLower() + "active", item.Active),
                DataProvider.CreateParameter("@p_" + TablePrefix.ToLower() + "sortorder", item.SortOrder),
                DataProvider.CreateParameter("@p_user_id", ApplicationContext.CurrentUserLanId),
                DataProvider.CreateOutIntParameter("@p_retval")
            });
            return sqlParams;
        }


        public void Update(LookupItem lookupItem)
        {
            var sqlParams = GetUpdateParams(lookupItem);

            using (var conn = SqlConnection)
            {
                conn.Open();
                using (var cmd = new SqlCommand(SqlSpUpdate, conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.AddRange(sqlParams.ToArray());
                    cmd.ExecuteNonQuery();

                    var outParam = sqlParams.FirstOrDefault(x => x.Direction == System.Data.ParameterDirection.Output);
                    lookupItem.Id = outParam.Value as int? ?? default(int);
                }
            }
        }

        public void Insert(LookupItem lookupItem)
        {
            var sqlParams = GetUpdateParams(lookupItem);

            using (var conn = SqlConnection)
            {
                conn.Open();
                using (var cmd = new SqlCommand(SqlSpInsert, conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.AddRange(sqlParams.ToArray());
                    cmd.ExecuteNonQuery();

                    var outParam = sqlParams.FirstOrDefault(x => x.Direction == System.Data.ParameterDirection.Output);
                    lookupItem.Id = outParam.Value as int? ?? default(int);
                }
            }
        }



        public void Delete(LookupItem item)
        {
            item.Active = false;
            Update(item);
        }




    }
}



