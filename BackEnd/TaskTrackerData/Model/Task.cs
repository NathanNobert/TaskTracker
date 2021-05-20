using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TaskTrackerData.Model.Base;

namespace TaskTrackerData.Model
{
    public class Task : ListableDataObject
    {
        #region DB constants

        private const string SqlPkName = "@p_tus_id";
        private const string SqlSpNameLoad = "TTR_GET_USERS";
        private const string SqlSpNameInsert = "TTR_INSERT_USERS";
        private const string SqlSpNameUpdate = "TTR_UPDATE_USERS";
        private const string SqlSpNameDelete = "TTR_DELETE_USERS";
        #endregion

        #region Constructor
        internal Task(ApplicationContext applicationContext)
            : base(applicationContext, SqlPkName, SqlSpNameLoad, SqlSpNameUpdate, SqlSpNameInsert, SqlSpNameDelete)
        { }

        internal Task(ApplicationContext applicationContext, int tusId)
            : this(applicationContext)
        {
            if (TusId > 0)
                Load(tusId);
        }
        #endregion

        #region Unique Properties
        public int TusId { get; set; }
        public string TusEmail { get; set; }
        public string TusLanName { get; set; }
        public string TusFName { get; set; }
        public string TusLName { get; set; }
        public string TusPassword { get; set; }
        public bool TusAdmin { get; set; }
        #endregion

        #region DataObject functions

        protected override void Populate(SqlDataReader rdr)
        {
            TusId = rdr.GetSafeInt("TUS_ID");
            TusEmail = rdr.GetSafeString("TUS_EMAIL");
            TusLanName = rdr.GetSafeString("TUS_LAN_NAME");
            TusFName = rdr.GetSafeString("TUS_FNAME");
            TusLName = rdr.GetSafeString("TUS_LNAME");
            TusPassword = rdr.GetSafeString("TUS_PASSWORD");
            TusAdmin = rdr.GetSafeBool("TUS_ADMIN");
        }

        protected override List<SqlParameter> GetUpdateParams()
        {
            var sqlParams = new List<SqlParameter>();
            if (GetPk() > 0)
                sqlParams.Add(DataProvider.CreateParameter("p_tus_id", TusId));

            sqlParams.AddRange(new SqlParameter[] {
                DataProvider.CreateParameter("p_tus_email", TusEmail),
                DataProvider.CreateParameter("p_tus_lan_name", TusLanName),
                DataProvider.CreateParameter("p_tus_fname", TusFName),
                DataProvider.CreateParameter("p_tus_lname", TusLName),
                DataProvider.CreateParameter("p_tus_password", TusPassword),
                DataProvider.CreateParameter("p_user_id", ApplicationContext.CurrentUserLanId),
                DataProvider.CreateOutIntParameter("@p_retval")
            });
            return sqlParams;

        }

        protected override T GetNewInstance<T>()
        {
            return new User(ApplicationContext) as T;
        }

        protected override void SetIdentityValue(int pkId)
        {
            TusId = pkId;
        }

        protected override int GetPk()
        {
            return TusId;
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

        public List<Task> GetList()
        {
            return GetList<User>();
        }

        public Task<List<Task>> GetListAsync()
        {
            return GetListAsync<User>();
        }

        #endregion
    }
}
