using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.ApplicationInsights.Core;
using Microsoft.Bot.Builder.Integration.Functions;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(BotFunctionAppWithAdapterAndDI.Startup))]

namespace BotFunctionAppWithAdapterAndDI
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ICredentialProvider, ConfigurationCredentialProvider>();
            builder.Services.AddSingleton<IBotFrameworkFunctionsAdapter, BotFrameworkFunctionsAdapter>();
            builder.Services.AddSingleton<IBot, Bot>();
            builder.Services.AddBotApplicationInsights();
        }
    }
}
