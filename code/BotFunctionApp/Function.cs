using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Logging;

namespace BotFunctionApp
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
            var adapter = new BotFrameworkHttpAdapter(credentialProvider);
            var bot = new Bot();
            var res = new DefaultHttpResponse(req.HttpContext);
            await adapter.ProcessAsync(req, res, bot);

            return new StatusCodeResult(res.StatusCode);
        }
    }
}
