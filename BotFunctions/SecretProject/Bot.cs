using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Adaptive;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Actions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Conditions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Recognizers;
using Microsoft.Bot.Builder.LanguageGeneration.Generators;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SecretProject
{
    public class Bot : ActivityHandler
    {
        private const string JokeIntent = "joke";
        private const string FortuneIntent = "fortune";
        private readonly DialogManager dialogManager;

        public Bot()
        {
            var adaptiveDialog = new AdaptiveDialog(nameof(AdaptiveDialog))
            {
                Generator = new TemplateEngineLanguageGenerator(),
                Recognizer = new RegexRecognizer()
                {
                    Intents = new List<IntentPattern>() {
                        new IntentPattern(JokeIntent, "(?i)joke"),
                        new IntentPattern(FortuneIntent, "(?i)fortune|future")
                    }
                },
                Triggers = new List<OnCondition>()
                {
                    new OnConversationUpdateActivity()
                    {
                        Actions = new List<Dialog>()
                        {
                            new Foreach()
                            {
                                ItemsProperty = "turn.activity.membersAdded",
                                Actions = new List<Dialog>()
                                {
                                    new IfCondition()
                                    {
                                        Condition = "dialog.foreach.value.id != turn.activity.recipient.id",
                                        Actions = new List<Dialog>()
                                        {
                                            new SendActivity("Good morning {dialog.foreach.value.name}"),
                                            new SendActivity("This sample uses AdaptiveDialog"),
                                            new SendActivity(IntroActivity("What can I help you with today?"))
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new OnIntent()
                    {
                        Intent = JokeIntent,
                        Actions = new List<Dialog>()
                        {
                            new SendActivity("Why did the chicken cross the road?"),
                            new EndTurn(),
                            new SendActivity("Because I need to show a multi-step dialog"),
                            new SendActivity(IntroActivity("What else can I do for you?"))
                        }
                    },
                    new OnIntent()
                    {
                        Intent = FortuneIntent,
                        Actions = new List<Dialog>()
                        {
                            new SendActivity("Seeing into the future..."),
                            new SendActivity("I see great things happening..."),
                            new SendActivity("Perhaps even a successful bot demo"),
                            new SendActivity(IntroActivity("What else can I do for you?"))
                        }
                    },
                    new OnUnknownIntent()
                    {
                        Actions = new List<Dialog>()
                        {
                            new SendActivity("Sorry, I didn't get that."),
                            new SendActivity(IntroActivity("What else can I do for you?"))
                        }
                    }
                }
            };
            dialogManager = new DialogManager(adaptiveDialog);
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await dialogManager.OnTurnAsync(turnContext, cancellationToken: cancellationToken);
        }

        private Activity IntroActivity(string text)
        {
            IEnumerable<string> cardActions = new string[] { "Joke", "Future" };
            return MessageFactory.SuggestedActions(cardActions, text, text, InputHints.ExpectingInput) as Activity;
        }
    }
}
