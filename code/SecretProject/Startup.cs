using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.ApplicationInsights.Core;
using Microsoft.Bot.Builder.Integration.Functions;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(SecretProject.Startup))]

namespace SecretProject
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IStorage, MemoryStorage>();
            builder.Services.AddSingleton<UserState>();
            builder.Services.AddSingleton<ConversationState>();
            builder.Services.AddSingleton<IBotFrameworkFunctionsAdapter, Adapter>();
            builder.Services.AddSingleton<IBot, Bot>();
            builder.Services.AddBotApplicationInsights();
        }
    }
}
