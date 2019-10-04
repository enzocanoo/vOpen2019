using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace MainProject.Dialogs
{
    public class FortuneDialog : Dialog
    {
        public FortuneDialog() : base(nameof(FortuneDialog))
        {
        }

        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            await dc.Context.SendActivityAsync(MessageFactory.Text("Seeing into the future..."), cancellationToken);
            await Wait(dc.Context, 1500, cancellationToken);
            await dc.Context.SendActivityAsync(MessageFactory.Text("I see great things happening..."), cancellationToken);
            await Wait(dc.Context, 2000, cancellationToken);
            await dc.Context.SendActivityAsync(MessageFactory.Text("Perhaps even a successful bot demo"), cancellationToken);

            return await dc.EndDialogAsync(EndOfTurn, cancellationToken);
        }

        private async Task Wait(ITurnContext context, int millisecondsDelay, CancellationToken cancellationToken)
        {
            var typingActivity = new Activity
            {
                Type = ActivityTypes.Typing,
                RelatesTo = context.Activity.RelatesTo,
            };

            await context.SendActivityAsync(typingActivity, cancellationToken);
            await Task.Delay(millisecondsDelay);
        }
    }
}
