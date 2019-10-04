using Microsoft.Bot.Builder;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SecretProject.Recognizers
{
    public class RegExRecognizer : IRecognizer
    {
        public const string JokeIntent = "joke";
        public const string FortuneIntent = "fortune";
        public const string NoneIntent = "none";

        private static readonly Regex jokeIntent = new Regex("(?i)joke", RegexOptions.Compiled);
        private static readonly Regex fortuneIntent = new Regex("(?i)fortune|future", RegexOptions.Compiled);

        public Task<RecognizerResult> RecognizeAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var text = turnContext?.Activity?.Text ?? string.Empty;
            var result = new RecognizerResult
            {
                Text = text,
                Intents = GetIntentsResult(text)
            };

            return Task.FromResult(result);
        }

        public async Task<T> RecognizeAsync<T>(ITurnContext turnContext, CancellationToken cancellationToken) where T : IRecognizerConvert, new()
        {
            var resultRaw = await this.RecognizeAsync(turnContext, cancellationToken).ConfigureAwait(false);
            var result = new T();
            result.Convert(resultRaw);

            return result;
        }


        private IDictionary<string, IntentScore> GetIntentsResult(string text)
        {
            var result = new Dictionary<string, IntentScore>
            {
                { JokeIntent, new IntentScore() { Score = GetScore(jokeIntent, text) } },
                { FortuneIntent, new IntentScore() { Score = GetScore(fortuneIntent, text) } }
            };

            // Special case for None intent
            result.Add(NoneIntent, result.Values.Aggregate(new IntentScore() { Score = 1 }, (acc, curr) => {
                acc.Score -= curr.Score;
                if (acc.Score < 0) acc.Score = 0;
                return acc;
            }));

            return result;
        }

        private double? GetScore(Regex intent, string text)
        {
            var match = intent.Match(text);
            return (text.Length - match.Length) / text.Length;
        }
    }
}
