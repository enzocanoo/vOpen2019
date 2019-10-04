using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MainProject
{
    public class Bot<T> : ActivityHandler where T : Dialog
    {
        private readonly UserState userState;
        private readonly ConversationState conversationState;
        private readonly BotStateSet states;
        private readonly T dialog;

        public Bot(T dialog, UserState userState, ConversationState conversationState)
        {
            this.dialog = dialog;
            this.userState = userState;
            this.conversationState = conversationState;
            states = new BotStateSet(userState, conversationState);
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);
            await states.SaveAllChangesAsync(turnContext, cancellationToken: cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var dialogState = conversationState.CreateProperty<DialogState>(nameof(DialogState));
            await dialog.RunAsync(turnContext, dialogState, cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var memeber in membersAdded.Where(m => m.Id != turnContext.Activity.Recipient.Id))
            {
                await turnContext.SendActivityAsync(MessageFactory.Text($"Good morning { memeber.Name ?? "'_you_'" }!"), cancellationToken);

                var dialogState = conversationState.CreateProperty<DialogState>(nameof(DialogState));
                await dialog.RunAsync(turnContext, dialogState, cancellationToken);
            }
        }
    }
}
