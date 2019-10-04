using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.Functions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace SecretProject
{
    public class Adapter : BotFrameworkFunctionsAdapter
    {
        public Adapter(
            IConfiguration configuration,
            IStorage storage,
            UserState userState,
            ConversationState conversationState,
            ILogger<BotFrameworkFunctionsAdapter> logger)
            : base(configuration, logger)
        {
            this.UseStorage(storage);
            this.UseState(userState, conversationState);

            OnTurnError = async (ctx, ex) => {
                // Log any leaked exception from the application.
                logger.LogError(ex, "Exception caught with activity {0}", JsonConvert.SerializeObject(ctx.Activity));

                // Send a catch-all appology to the user.
                await ctx.SendActivityAsync(MessageFactory.Text("Oooops! I didn't catch that")).ConfigureAwait(false);
                await ctx.SendActivityAsync(ex.Message).ConfigureAwait(false);

                if (conversationState != null)
                {
                    try
                    {
                        // Delete the conversationState for the current conversation to prevent the
                        // bot from getting stuck in a error-loop caused by being in a bad state.
                        // ConversationState should be thought of as similar to "cookie-state" in a Web pages.
                        await conversationState.DeleteAsync(ctx).ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Exception caught on attempting to Delete ConversationState");
                    }
                }
            };
        }
    }
}
