using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Microsoft.Bot.Builder.Integration.Functions
{
    public class BotFrameworkFunctionsAdapter : BotFrameworkAdapter, IBotFrameworkFunctionsAdapter
    {
        public BotFrameworkFunctionsAdapter(ICredentialProvider credentialProvider = null, IChannelProvider channelProvider = null, ILogger<BotFrameworkFunctionsAdapter> logger = null)
            : base(credentialProvider ?? new SimpleCredentialProvider(), channelProvider, null, null, null, logger)
        {
        }

        public BotFrameworkFunctionsAdapter(ICredentialProvider credentialProvider, IChannelProvider channelProvider, HttpClient httpClient, ILogger<BotFrameworkFunctionsAdapter> logger)
            : base(credentialProvider ?? new SimpleCredentialProvider(), channelProvider, null, httpClient, null, logger)
        {
        }

        protected BotFrameworkFunctionsAdapter(IConfiguration configuration, ILogger<BotFrameworkFunctionsAdapter> logger = null)
            : base(new ConfigurationCredentialProvider(configuration), new ConfigurationChannelProvider(configuration), customHttpClient: null, middleware: null, logger: logger)
        {
            var openIdEndpoint = configuration.GetSection(AuthenticationConstants.BotOpenIdMetadataKey)?.Value;

            if (!string.IsNullOrEmpty(openIdEndpoint))
            {
                // Indicate which Cloud we are using, for example, Public or Sovereign.
                ChannelValidation.OpenIdMetadataUrl = openIdEndpoint;
                GovernmentChannelValidation.OpenIdMetadataUrl = openIdEndpoint;
            }
        }

        public async Task<IActionResult> ProcessAsync(HttpRequest httpRequest, IBot bot, CancellationToken cancellationToken = default)
        {
            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            if (bot == null)
            {
                throw new ArgumentNullException(nameof(bot));
            }

            // deserialize the incoming Activity
            var activity = HttpHelper.ReadRequest(httpRequest);

            if (string.IsNullOrEmpty(activity?.Type))
            {
                return new StatusCodeResult((int) HttpStatusCode.BadRequest);
            }

            // grab the auth header from the inbound http request
            var authHeader = httpRequest.Headers["Authorization"];

            try
            {
                // process the inbound activity with the bot
                var invokeResponse = await ProcessActivityAsync(authHeader, activity, bot.OnTurnAsync, cancellationToken).ConfigureAwait(false);

                // write the response, potentially serializing the InvokeResponse
                return HttpHelper.WriteResponse(invokeResponse);
            }
            catch (UnauthorizedAccessException)
            {
                // handle unauthorized here as this layer creates the http response
                return new UnauthorizedResult();
            }
        }
    }
}
