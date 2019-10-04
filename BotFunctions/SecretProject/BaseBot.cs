using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Adaptive;
using Microsoft.Bot.Builder.Dialogs.Debugging;
using Microsoft.Bot.Builder.Dialogs.Declarative;
using Microsoft.Bot.Builder.Dialogs.Declarative.Resources;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SecretProject
{
    public class BaseBot : ActivityHandler
    {
        private readonly ResourceExplorer resourceExplorer;
        private DialogManager dialogManager;

        public BaseBot(ResourceExplorer resourceExplorer)
        {
            this.resourceExplorer = resourceExplorer;

            // auto reload dialogs when file changes
            this.resourceExplorer.Changed += (resources) =>
            {
                if (resources.Any(resource => resource.Id.EndsWith(".dialog")))
                {
                    Task.Run(() => this.LoadDialogs());
                }
            };
            LoadDialogs();
        }

        private void LoadDialogs()
        {
            System.Diagnostics.Trace.TraceInformation("Loading resources...");

            var resource = resourceExplorer.GetResource("main.dialog");
            dialogManager = new DialogManager(DeclarativeTypeLoader.Load<AdaptiveDialog>(resource, resourceExplorer, DebugSupport.SourceRegistry));

            System.Diagnostics.Trace.TraceInformation("Done loading resources.");
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await dialogManager.OnTurnAsync(turnContext, cancellationToken: cancellationToken);
        }
    }
}
