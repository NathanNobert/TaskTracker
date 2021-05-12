using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.SqlClient;
using TaskTrackerData.Model.Base;

namespace TaskTrackerData.Repository
{
    public class LookupRepository
    {
        private ApplicationContext ApplicationContext { get; set; }
        private readonly SqlConnection _dataProvider;
        private Assembly _assembly = Assembly.GetAssembly(typeof(LookupItem));

        public LookupRepository(ApplicationContext applicationContext)
        {
            ApplicationContext = applicationContext;
            _dataProvider = ApplicationContext.GetSqlConnection();
        }


        public List<LookupItem> GetAllTableNames()
        {
            var tablenames = new List<LookupItem>();
            using (var conn = _dataProvider)
            {
                conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME like 'CMP_LKP_%' and TABLE_NAME not like '%_HISTORY'";

                using var rdr = cmd.ExecuteReader();
                int counter = 0;
                while (rdr.Read())
                {
                    var LookupItem = new LookupItem();
                    LookupItem.Id = counter;
                    LookupItem.Active = true;
                    LookupItem.Description = rdr.GetSafeString("TABLE_NAME");
                    LookupItem.Code = rdr.GetSafeString("TABLE_NAME");
                    tablenames.Add(LookupItem);
                    counter++;
                }
            }
            return tablenames;
        }

        #region Testing
        public List<LookupItem> GetLookupTable(string lookupTableName)
        {
            return GetLookupTableAsync(lookupTableName).Result;
        }

        public async Task<List<LookupItem>> GetLookupTableAsync(string lookupTableName)
        {
            Type lookupType = null;
            List<LookupItem> lookupTable = null;

            lookupType = _assembly.GetType("Crm.Data.Model.Lookup." + lookupTableName);

            if (!(Activator.CreateInstance(lookupType, new Object[] { ApplicationContext }) is Lookup lookup))
                throw new InvalidCastException($"lookupTableName '{lookupTableName}' is not the correct type, must be Crm.Data.Model.Base.Lookup type");

            lookupTable = await GetLookupTableAsync(lookupTableName, lookup);

            return lookupTable;
        }

        private async Task<List<LookupItem>> GetLookupTableAsync(string tableName, Lookup lookup)
        {
            List<LookupItem> lookupListItems;

            lookupListItems = await lookup.GetLookupListAsync();

            var sortedList = lookupListItems.OrderBy(x => x.Description.ToLower()).ToList();

            return sortedList;
        }

        #endregion

        #region UnUsed
        private Lookup CreateLookup<T>() where T : Lookup
        {
            return Activator.CreateInstance(typeof(T), new Object[] { ApplicationContext }) as Lookup;
        }
        #endregion


    }
}
