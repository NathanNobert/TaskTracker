using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TaskTrackerData.Model;
using TaskTrackerData.Repository;
using Microsoft.Extensions.Caching.Memory;
using AutoMapper;
using TaskTrackerData;

namespace TaskTrackerApi
{
    public class TestFunction : Base.BaseAzureFunction
    {
        private readonly CommonRepository _commonRepository;
        private readonly ApplicationContext _appContext;

        public TestFunction(IApplicationConfigurationService applicationConfigurationService, IMemoryCache memoryCache, IMapper autoMapper)
            : base(applicationConfigurationService, memoryCache, autoMapper)
        {
            _commonRepository = new CommonRepository(ApplicationContext);
            _appContext = ApplicationContext;
        }

        [FunctionName("TestFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("GetTasks")]
        public static async Task<IActionResult> GetTasks(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetTasks/{taskName}")] HttpRequest req,
            string taskName,
            ILogger log)
        {

            var tasks = taskName;

            return new OkObjectResult(tasks);
        }

        [FunctionName("GetUsers")]
        public async Task<IActionResult> GetUsers(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetUsers")] HttpRequest req,
            ILogger log)
        {
            var user = CommonRepository.User.GetList();

            var users = user;

            return new OkObjectResult(users);
        }
    }
}
