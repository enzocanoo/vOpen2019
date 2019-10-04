using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Adaptive;
using Microsoft.Bot.Builder.Dialogs.Debugging;
using Microsoft.Bot.Builder.Dialogs.Declarative;
using Microsoft.Bot.Builder.Dialogs.Declarative.Resources;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SuperSecretProject
{
    public class Bot : ActivityHandler
    {
        private readonly ResourceExplorer resourceExplorer;
        private DialogManager dialogManager;

        public Bot(ResourceExplorer resourceExplorer)
        {
            this.resourceExplorer = resourceExplorer;
            this.resourceExplorer.Changed += ResourceChangedHandler;
            
            RefreshDialog();
        }

        private void ResourceChangedHandler(IResource[] resources)
        {
            if (resources.Any(r => r.Id.EndsWith(".dialog")))
            {
                RefreshDialog();
            }
        }

        private void RefreshDialog()
        {
            var resource = resourceExplorer.GetResource("main.dialog");
            var dialog = DeclarativeTypeLoader.Load<AdaptiveDialog>(resource, resourceExplorer, DebugSupport.SourceMap);
            dialogManager = new DialogManager(dialog);
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await dialogManager.OnTurnAsync(turnContext, cancellationToken: cancellationToken);
        }
    }
}
