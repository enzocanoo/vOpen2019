using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs.Declarative.Resources;
using Microsoft.Bot.Builder.Dialogs.Declarative.Types;
using Microsoft.Bot.Builder.Integration.ApplicationInsights.Core;
using Microsoft.Bot.Builder.Integration.Functions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(SecretProject.Startup))]
namespace SecretProject
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton(s => {
                var env = s.GetService<IHostingEnvironment>();
                var config = s.GetService<IConfiguration>();

                TypeFactory.Configuration = config;
                var explorer = new ResourceExplorer();
                explorer.AddFolder(env.ContentRootPath, true, true);

                return explorer;
            });

            builder.Services.AddTransient<IBotFrameworkFunctionsAdapter, Adapter>();
            builder.Services.AddSingleton<IStorage, MemoryStorage>();
            builder.Services.AddSingleton<UserState>();
            builder.Services.AddSingleton<ConversationState>();
            builder.Services.AddTransient<IBot, BaseBot>();
            builder.Services.AddBotApplicationInsights();
        }
    }
}
