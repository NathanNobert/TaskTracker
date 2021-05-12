using System.Linq;
using System.Data.SqlClient;

namespace TaskTrackerData
{
    public class ApplicationContext
    {

        #region Variables
        private string _userLanId;
        //private IConfiguration _config;
        private DataProvider DataProvider { get; }

        #endregion


        //public ApplicationContext(IConfiguration config, IDataProvider dataProvider)
        public ApplicationContext(IApplicationConfigurationService applicationConfigurationService)
        {
            //_config = config;
            //DataProvider = dataProvider;
            DataProvider = new DataProvider(applicationConfigurationService.GetDbConnection());
        }

        public SqlConnection GetSqlConnection()
        {
            return DataProvider.SqlConnection;
        }

        public void SetUserLanId(string userLanId)
        {
            _userLanId = userLanId.Split('\\').Last();
        }

        public string CurrentUserLanId { get { return _userLanId; } }


    }
}
