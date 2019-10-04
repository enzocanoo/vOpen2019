using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Bot.Builder.Integration.Functions
{
    public interface IBotFrameworkFunctionsAdapter
    {
        /// <summary>
        /// This method can be called from inside a POST method on a Function App implementation.
        /// </summary>
        /// <param name="httpRequest">The HTTP request object, typically in a POST handler by a Http trigger.</param>
        /// <param name="bot">The bot implementation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        Task<IActionResult> ProcessAsync(HttpRequest httpRequest, IBot bot, CancellationToken cancellationToken = default);
    }
}
