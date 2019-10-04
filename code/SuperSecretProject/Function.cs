using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Builder.Integration.Functions;
using Microsoft.Bot.Builder;

namespace SuperSecretProject
{
    public class Function
    {
        private readonly IBotFrameworkFunctionsAdapter adapter;
        private readonly IBot bot;

        public Function(IBotFrameworkFunctionsAdapter adapter, IBot bot)
        {
            this.adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            this.bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        [FunctionName("messages")]
        public Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            return adapter.ProcessAsync(req, bot);
        }
    }
}
