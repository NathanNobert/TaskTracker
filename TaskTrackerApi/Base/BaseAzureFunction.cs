using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using AutoMapper;
using TaskTrackerData;
using TaskTrackerData.Repository;
using TaskTrackerData.Utility;

namespace TaskTrackerApi.Base
{

    public class BaseAzureFunction
    {
        protected TaskTrackerData.ApplicationContext ApplicationContext { get; }
        protected LookupHelper LookupHelper { get; }
        protected LookupRepository LookupRepository { get; }
        protected CommonRepository CommonRepository { get; }
        protected IMapper Mapper { get; }

        public BaseAzureFunction(IApplicationConfigurationService applicationConfigurationService, IMemoryCache memoryCache, IMapper autoMapper)
        {
            ApplicationContext = new TaskTrackerData.ApplicationContext(applicationConfigurationService);
            LookupHelper = new LookupHelper(memoryCache, ApplicationContext);
            LookupRepository = new LookupRepository(ApplicationContext);
            CommonRepository = new CommonRepository(ApplicationContext);
            Mapper = autoMapper;
        }

        protected Object HandleExceptionAndReturnObject(ILogger log, Exception ex, string functionName, Object returnObject)
        {
            log.LogError(ex, $"Unhandled exception caught in {this.GetType().Name + '.' + functionName}\n{ex.Message}");
            return returnObject;
        }

        protected void HandleExceptionNoReturn(ILogger log, Exception ex, string functionName)
        {
            log.LogError(ex, $"Unhandled exception caught in {this.GetType().Name + '.' + functionName}");
        }

        /// <summary>
        /// must be called at the start of each function because user identity is not available until function is called
        /// </summary>
        /// <param name="req"></param>
        protected void InitializeContext(HttpRequest req)
        {
            if (req.HttpContext.User.Identity.IsAuthenticated)
                ApplicationContext.SetUserLanId(req.HttpContext.User.Identity.Name.Split('@').First());
            else
                ApplicationContext.SetUserLanId("TaskTrackerApi");
        }
    }
}
