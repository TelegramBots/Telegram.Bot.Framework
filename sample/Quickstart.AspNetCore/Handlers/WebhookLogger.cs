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
            _logger.LogInformation(
                "Received update {0} as a webhook from {1}.",
                context.Update.Id,
                ((HttpContext)context.HttpContext).Request.Host
            );

            return next(context);
        }
    }
}
