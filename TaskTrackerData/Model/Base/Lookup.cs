using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskTrackerData.Model.Base
{
    public abstract class Lookup
    {
        #region properties
        protected string TablePrefix { get; }
        protected LookupInterface LookupInterface;
        protected ApplicationContext ApplicationContext { get; set; }
        #endregion

        #region constructor
        protected Lookup(ApplicationContext applicationContext, string sqlTablePrefix, string sqlSpNameLoad,
            string sqlSpNameUpdate, string sqlSpNameInsert, string sqlSpNameDelete)
        {
            TablePrefix = sqlTablePrefix;
            ApplicationContext = applicationContext;
            LookupInterface = new LookupInterface(applicationContext, sqlTablePrefix,
                sqlSpNameLoad, sqlSpNameUpdate, sqlSpNameInsert, sqlSpNameDelete);
        }
        #endregion

        public void Update(LookupItem lookupItem)
        {
            if (lookupItem.Id != 0)
                LookupInterface.Update(lookupItem);
            else
                LookupInterface.Insert(lookupItem);
        }


        // Get the Lookup List for the specific Type, Including inactive be default
        public virtual Task<List<LookupItem>> GetLookupListAsync(bool includeInactive = true)
        {
            return LookupInterface.GetListAsync<Lookup>(null, includeInactive);
        }


        #region UnUsed
        // Get the Lookup List for the specific Type, Including inactive be default
        public virtual List<LookupItem> GetLookupList(bool includeInactive = true)
        {
            return LookupInterface.GetList<Lookup>(null, includeInactive);
        }

        public int? GetId(string code)
        {
            if (String.IsNullOrEmpty(code))
            {
                return null;
            }
            var item = LookupInterface.GetItem(code: code);
            return item.Id > 0 ? item.Id : (int?)null;
        }

        public async Task<int?> GetIdAsync(string code)
        {
            if (String.IsNullOrEmpty(code))
                return null;
            var item = await LookupInterface.GetItemAsync(code: code);
            return item.Id > 0 ? item.Id : (int?)null;
        }

        public string GetCode(int id)
        {
            return LookupInterface.GetItem(id: id).Code;
        }

        public async Task<string> GetCodeAsync(int id)
        {
            return (await LookupInterface.GetItemAsync(id: id)).Code;
        }



        //public void Delete(LookupItem item)
        //{
        //    item.Active = false;
        //    //Update(item);
        //}

        #endregion


    }
}
