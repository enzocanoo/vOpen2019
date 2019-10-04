using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.ApplicationInsights.Core;
using Microsoft.Bot.Builder.Integration.Functions;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(BotFunctionAppWithAdapterAndDI.Startup))]
namespace BotFunctionAppWithAdapterAndDI
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IBotFrameworkFunctionsAdapter, Adapter>();
            builder.Services.AddTransient<IBot, Bot>();
            builder.Services.AddBotApplicationInsights();
        }
    }
}
