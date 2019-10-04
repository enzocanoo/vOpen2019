using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Bot.Builder.Integration.Functions
{
    /// <summary>
    /// Channel provider which uses <see cref="Microsoft.Extensions.Configuration.IConfiguration"/> to lookup the channel service property.
    /// </summary>
    /// <remarks>
    /// This will populate the <see cref="SimpleChannelProvider.ChannelService"/> from a configuration entry with the key of <see cref="ChannelServiceKey"/>.
    ///
    /// NOTE: if the keys are not present, a <c>null</c> value will be used.
    /// </remarks>
    public sealed class ConfigurationChannelProvider : SimpleChannelProvider
    {
        /// <summary>
        /// The key for ChannelService.
        /// </summary>
        public const string ChannelServiceKey = "ChannelService";

        public ConfigurationChannelProvider(IConfiguration configuration)
        {
            ChannelService = configuration.GetSection(ChannelServiceKey)?.Value;
        }
    }
}
