using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Adaptive;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Recognizers;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Rules;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Steps;
using Microsoft.Bot.Builder.LanguageGeneration;
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
                    Intents = new Dictionary<string, string>()
                    {
                        { JokeIntent, "(?i)joke" },
                        { FortuneIntent, "(?i)fortune|future" }
                    }
                },
                Rules = new List<IRule>()
                {
                    new ConversationUpdateActivityRule()
                    {
                        Steps = new List<IDialog>()
                        {
                            new Foreach()
                            {
                                ListProperty = "turn.activity.membersAdded",
                                ValueProperty = "turn.memberAdded",
                                Steps = new List<IDialog>()
                                {
                                    new IfCondition()
                                    {
                                        Condition = "turn.memberAdded.id != turn.activity.recipient.id",
                                        Steps = new List<IDialog>()
                                        {
                                            new SendActivity("Good morning {turn.memberAdded.name}"),
                                            new SendActivity(IntroActivity("What can I help you with today?"))
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new IntentRule()
                    {
                        Intent = JokeIntent,
                        Steps = new List<IDialog>()
                        {
                            new SendActivity("Why did the chicken cross the road?"),
                            new EndTurn(),
                            new SendActivity("Because I need to show a multi-step dialog"),
                            new SendActivity(IntroActivity("What else can I do for you?"))
                        }
                    },
                    new IntentRule()
                    {
                        Intent = FortuneIntent,
                        Steps = new List<IDialog>()
                        {
                            new SendActivity("Seeing into the future..."),
                            new SendActivity("I see great things happening..."),
                            new SendActivity("Perhaps even a successful bot demo"),
                            new SendActivity(IntroActivity("What else can I do for you?"))
                        }
                    },
                    new UnknownIntentRule()
                    {
                        Steps = new List<IDialog>()
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
