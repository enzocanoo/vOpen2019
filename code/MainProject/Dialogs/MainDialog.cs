using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MainProject.Recognizers;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace MainProject.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly double intentThreshold = 0.1;
        private readonly IRecognizer recognizer;

        public MainDialog(IRecognizer recognizer, JokeDialog jokeDialog, FortuneDialog fortuneDialog) : base(nameof(MainDialog))
        {
            this.recognizer = recognizer;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(jokeDialog);
            AddDialog(fortuneDialog);
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[] { IntroStepAsync, ActStepAsync, EndStepASync }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var messageText = stepContext.Options?.ToString() ?? "What can I help you with today?";
            IEnumerable<string> cardActions = new string[] { "Joke", "Future" };
            var promptMessage = MessageFactory.SuggestedActions(cardActions, messageText, messageText, InputHints.ExpectingInput) as Activity;

            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var intentResult = await recognizer.RecognizeAsync(stepContext.Context, cancellationToken);
            var (topIntent, topScore) = intentResult.GetTopScoringIntent();

            if (topScore > intentThreshold)
            {
                switch (topIntent)
                {
                    case RegExRecognizer.JokeIntent:
                        return await stepContext.BeginDialogAsync(nameof(JokeDialog), cancellationToken: cancellationToken);
                    case RegExRecognizer.FortuneIntent:
                        return await stepContext.BeginDialogAsync(nameof(FortuneDialog), cancellationToken: cancellationToken);
                }
            }

            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Sorry, I didn't get that."), cancellationToken);

            return await stepContext.NextAsync(EndOfTurn, cancellationToken);
        }

        private async Task<DialogTurnResult> EndStepASync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (EndOfTurn.Equals(stepContext.Result))
            {
                var promptMessage = "What else can I do for you?";
                return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
