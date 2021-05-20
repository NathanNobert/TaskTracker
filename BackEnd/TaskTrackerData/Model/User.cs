using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TaskTrackerData.Model.Base;
namespace TaskTrackerData.Model
{
    public class User : ListableDataObject
    {
        #region DB constants

        private const string SqlPkName = "@p_usr_id";
        private const string SqlSpNameLoad = "TTR_GET_USER";
        private const string SqlSpNameInsert = "TTR_INSERT_USER";
        private const string SqlSpNameUpdate = "TTR_UPDATE_USER";
        private const string SqlSpNameDelete = "TTR_DELETE_USER";
        #endregion

        #region Constructor
        internal User(ApplicationContext applicationContext)
            : base(applicationContext, SqlPkName, SqlSpNameLoad, SqlSpNameUpdate, SqlSpNameInsert, SqlSpNameDelete)
        { }

        internal User(ApplicationContext applicationContext, int usrId)
            : this(applicationContext)
        {
            if (UsrId > 0)
                Load(usrId);
        }
        #endregion

        #region Unique Properties
        public int UsrId { get; set; }
        public string UsrEmail { get; set; }
        public string UsrLanName { get; set; }
        public string UsrFName { get; set; }
        public string UsrLName { get; set; }
        public string UsrPassword { get; set; }
        public bool UsrAdmin { get; set; }
        #endregion

        #region DataObject functions

        protected override void Populate(SqlDataReader rdr)
        {
            UsrId = rdr.GetSafeInt("USR_ID");
            UsrEmail = rdr.GetSafeString("USR_EMAIL");
            UsrLanName = rdr.GetSafeString("USR_LAN_NAME");
            UsrFName = rdr.GetSafeString("USR_FNAME");
            UsrLName = rdr.GetSafeString("USR_LNAME");
            UsrPassword = rdr.GetSafeString("USR_PASSWORD");
            UsrAdmin = rdr.GetSafeBool("USR_ADMIN");
        }

        protected override List<SqlParameter> GetUpdateParams()
        {
            var sqlParams = new List<SqlParameter>();
            if (GetPk() > 0)
                sqlParams.Add(DataProvider.CreateParameter("p_tus_id", UsrId));

            sqlParams.AddRange(new SqlParameter[] {
                DataProvider.CreateParameter("p_usr_email", UsrEmail),
                DataProvider.CreateParameter("p_usr_lan_name", UsrLanName),
                DataProvider.CreateParameter("p_usr_fname", UsrFName),
                DataProvider.CreateParameter("p_usr_lname", UsrLName),
                DataProvider.CreateParameter("p_usr_password", UsrPassword),
                DataProvider.CreateParameter("p_usr_admin", UsrAdmin),
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
            UsrId = pkId;
        }

        protected override int GetPk()
        {
            return UsrId;
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

        public List<User> GetList()
        {
            return GetList<User>();
        }

        public Task<List<User>> GetListAsync()
        {
            return GetListAsync<User>();
        }

        #endregion
    }
}
