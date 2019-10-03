using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.Functions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BotFunctionAppWithAdapterAndDI
{
    public class Adapter : BotFrameworkFunctionsAdapter
    {
        public Adapter(IConfiguration configuration, ILogger<BotFrameworkFunctionsAdapter> logger)
            : base(configuration, logger)
        {
            OnTurnError = (ctx, ex) => {
                // Log any leaked exception from the application.
                logger.LogError(ex, "Exception caught with activity {0}", JsonConvert.SerializeObject(ctx.Activity));

                // Send a catch-all appology to the user.
                return ctx.SendActivityAsync(MessageFactory.Text("Oooops! I didn't catch that"));
            };
        }
    }
}
