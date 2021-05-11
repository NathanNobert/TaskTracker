using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TaskTrackerData.Model.Base;
using TaskTrackerData.Repository;

namespace TaskTrackerData.Utility
{
    public class LookupHelper
    {
        #region Variables
        private TaskTrackerData.ApplicationContext _applicationContext;
        private IMemoryCache _cache;
        private Assembly _assembly = Assembly.GetAssembly(typeof(LookupItem));
        public LookupRepository _lookupRepository { get; private set; }
        #endregion

        public LookupHelper(IMemoryCache memoryCache, ApplicationContext applicationContext)
        {
            _cache = memoryCache;
            _applicationContext = applicationContext;
            _lookupRepository = new LookupRepository(applicationContext);
        }

        public List<LookupItem> GetLookupTable(string lookupTableName)
        {
            return GetLookupTableAsync(lookupTableName).Result;
        }

        public async Task<List<LookupItem>> GetLookupTableAsync(string lookupTableName)
        {
            Type lookupType = null;
            List<LookupItem> lookupTable = null;

            lookupType = _assembly.GetType("TaskTrackerData.Model.Lookup." + lookupTableName);

            if (!(Activator.CreateInstance(lookupType, new Object[] { _applicationContext }) is Lookup lookup))
                throw new InvalidCastException($"lookupTableName '{lookupTableName}' is not the correct type, must be TaskTrackerData.Model.Base.Lookup type");

            lookupTable = await GetLookupTableAsync(lookupTableName, lookup);

            return lookupTable;
        }

        public List<LookupItem> GetLookupTable<T>() where T : Lookup
        {
            List<LookupItem> lookupTable = TryGetCachedTable(typeof(T).Name);

            if (lookupTable == null)
            {
                var lookup = Activator.CreateInstance(typeof(T), new Object[] { _applicationContext }) as Lookup;
                lookupTable = GetLookupTable(typeof(T).Name, lookup);
            }

            return lookupTable;
        }

        private List<LookupItem> GetLookupTable(string tableName, Lookup lookup, bool cacheTable = true)
        {
            List<LookupItem> lookupListItems;

            lookupListItems = lookup.GetLookupList();

            var sortedList = lookupListItems.OrderBy(x => x.Description.ToLower()).ToList();

            // Save data in cache.
            if (cacheTable)
                CacheTable(tableName, sortedList);

            return sortedList;
        }

        private async Task<List<LookupItem>> GetLookupTableAsync(string tableName, Lookup lookup)
        {
            List<LookupItem> lookupListItems;

            lookupListItems = await lookup.GetLookupListAsync();

            var sortedList = lookupListItems.OrderBy(x => x.Description.ToLower()).ToList();

            return sortedList;
        }

        public List<LookupItem> GetAllTableNames()
        {
            return _lookupRepository.GetAllTableNames();
        }




        //public void UpdateLookupItem(LookupItem lookupRecord, string tableName, string userLanId)
        public void UpdateLookupItem(LookupItem lookupRecord, string tableName, string userLanId = "Test")
        {
            Type lookupType = null;
            lookupType = _assembly.GetType("TaskTrackerData.Model.Lookup." + tableName);

            if (!(Activator.CreateInstance(lookupType, new Object[] { _applicationContext }) is Lookup lookup))
                throw new InvalidCastException($"lookupTableName '{tableName}' is not the correct type, must be Crm.Data.Model.Base.Lookup type");


            lookup.Update(lookupRecord);
        }


        /// <summary>
        /// Get item from lookup table with a specific ID
        /// </summary>
        /// <typeparam name="T">Lookup Table Type</typeparam>
        /// <param name="lookupItemId">Lookup Item ID</param>
        /// <returns></returns>
        public LookupItem GetLookupItem<T>(int? lookupItemId)
            where T : Lookup
        {
            if (lookupItemId.GetValueOrDefault() == 0)
                return new LookupItem();

            var lookupTable = GetLookupTable<T>();
            return lookupTable.FirstOrDefault(x => x.Id == lookupItemId) ?? new LookupItem();
        }


        /// <summary>
        /// Get the description value for a lookup item
        /// </summary>
        /// <typeparam name="T">Lookup Table Type</typeparam>
        /// <param name="lookupItemId">Lookup Item ID</param>
        /// <returns></returns>
        public string GetLookupDescription<T>(int? lookupItemId)
            where T : Lookup
        {
            return GetLookupItem<T>(lookupItemId).Description;
        }


        #region table caching methods
        /// <summary>
        /// Cache table data for faster access
        /// </summary>
        /// <param name="tableName">Name of the table (cache collection key) being cached</param>
        /// <param name="table">Table object</param>
        /// <param name="cacheDurationMinutes">Number of minute table remains in cache (default = 30 minutes)</param>
        private void CacheTable(string tableName, object table, int cacheDurationMinutes = 30)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for this time, reset time if accessed.
                .SetSlidingExpiration(TimeSpan.FromMinutes(cacheDurationMinutes));

            // Save data in cache.
            _cache.Set(tableName, table, cacheEntryOptions);
        }

        private List<LookupItem> TryGetCachedTable(string tableName)
        {
            // Look for cache key.
            _cache.TryGetValue(tableName, out List<LookupItem> lookupList);

            return lookupList;
        }

        #endregion

    }
}
