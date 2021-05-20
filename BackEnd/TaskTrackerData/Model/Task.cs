using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TaskTrackerData.Model.Base;
namespace TaskTrackerData.Model
{
    public class TaskObject : ListableDataObject
    {
        #region DB constants

        private const string SqlPkName = "@p_tsk_id";
        private const string SqlSpNameLoad = "TTR_GET_TASK";
        private const string SqlSpNameInsert = "TTR_INSERT_TASK";
        private const string SqlSpNameUpdate = "TTR_UPDATE_TASK";
        private const string SqlSpNameDelete = "TTR_DELETE_TASK";
        #endregion

        #region Constructor
        internal TaskObject(ApplicationContext applicationContext)
            : base(applicationContext, SqlPkName, SqlSpNameLoad, SqlSpNameUpdate, SqlSpNameInsert, SqlSpNameDelete)
        { }

        internal TaskObject(ApplicationContext applicationContext, int tskId)
            : this(applicationContext)
        {
            if (TskId > 0)
                Load(tskId);
        }
        #endregion

        #region Unique Properties
        public int TskId { get; set; }
        public string TskCode { get; set; }
        public string TskDesc { get; set; }
        public bool TskActive { get; set; }
        #endregion

        #region DataObject functions

        protected override void Populate(SqlDataReader rdr)
        {
            TskId = rdr.GetSafeInt("TSK_ID");
            TskCode = rdr.GetSafeString("TSK_CODE");
            TskDesc = rdr.GetSafeString("TSK_DESC");
            TskActive = rdr.GetSafeBool("TSK_ACTIVE");
        }

        protected override List<SqlParameter> GetUpdateParams()
        {
            var sqlParams = new List<SqlParameter>();
            if (GetPk() > 0)
                sqlParams.Add(DataProvider.CreateParameter("p_tus_id", TskId));

            sqlParams.AddRange(new SqlParameter[] {
                DataProvider.CreateParameter("p_tsk_code", TskCode),
                DataProvider.CreateParameter("p_tsk_desc", TskDesc),
                DataProvider.CreateParameter("p_tsk_active", TskActive),
                DataProvider.CreateOutIntParameter("@p_retval")
            });
            return sqlParams;

        }

        protected override T GetNewInstance<T>()
        {
            return new TaskObject(ApplicationContext) as T;
        }

        protected override void SetIdentityValue(int pkId)
        {
            TskId = pkId;
        }

        protected override int GetPk()
        {
            return TskId;
        }

        #endregion

        #region type overrides
        public void Load(int pk)
        {
            base.Load(pk);
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public List<TaskObject> GetList()
        {
            return GetList<TaskObject>();
        }

        public Task<List<TaskObject>> GetListAsync()
        {
            return GetListAsync<TaskObject>();
        }

        #endregion
    }
}
