using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Builder.Integration.Functions;

namespace BotFunctionAppWithAdapter
{
    public static class Function
    {
        [FunctionName("messages")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger bot function processed a message.");

            var appId = Environment.GetEnvironmentVariable("MicrosoftAppId", EnvironmentVariableTarget.Process);
            var appPassword = Environment.GetEnvironmentVariable("MicrosoftAppPassword", EnvironmentVariableTarget.Process);

            var credentialProvider = new SimpleCredentialProvider(appId, appPassword);
            var adapter = new BotFrameworkFunctionsAdapter(credentialProvider);
            var bot = new Bot();
            return await adapter.ProcessAsync(req, bot);
        }
    }
}
