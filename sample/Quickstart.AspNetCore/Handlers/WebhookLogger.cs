using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace Quickstart.AspNetCore.Handlers
{
    class WebhookLogger : IUpdateHandler
    {
        private readonly ILogger<WebhookLogger> _logger;

        public WebhookLogger(
            ILogger<WebhookLogger> logger
        )
        {
            _logger = logger;
        }

        public Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            var httpContext = (HttpContext)context.Items[nameof(HttpContext)];

            _logger.LogInformation(
                "Received update {0} in a webhook at {1}.",
                context.Update.Id,
                httpContext.Request.Host
            );

            return next(context);
        }
    }
}
