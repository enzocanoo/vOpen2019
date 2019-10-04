using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs.Declarative.Resources;
using Microsoft.Bot.Builder.Dialogs.Declarative.Types;
using Microsoft.Bot.Builder.Integration.ApplicationInsights.Core;
using Microsoft.Bot.Builder.Integration.Functions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(SuperSecretProject.Startup))]

namespace SuperSecretProject
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton(s =>
            {
                var configuration = s.GetService<IConfiguration>();
                var env = s.GetService<IHostingEnvironment>();
                TypeFactory.Configuration = configuration;
                var resourceExplorer = new ResourceExplorer();
                resourceExplorer.AddFolder(Path.Combine(env.ContentRootPath, "Resources"));

                return resourceExplorer;
            });
            builder.Services.AddSingleton<IStorage, MemoryStorage>();
            builder.Services.AddSingleton<UserState>();
            builder.Services.AddSingleton<ConversationState>();
            builder.Services.AddSingleton<IBotFrameworkFunctionsAdapter, Adapter>();
            builder.Services.AddSingleton<IBot, Bot>();
            builder.Services.AddBotApplicationInsights();
        }
    }
}
