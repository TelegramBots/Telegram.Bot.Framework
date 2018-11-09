using System.IO;
using System.Threading;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Telegram.Bot.Framework.Abstractions;

namespace Quickstart.AspNetCore.Handlers
{
    class UpdateLogger : IUpdateHandler
    {
        private readonly ILogger<UpdateLogger> _logger;

        public UpdateLogger(
            ILogger<UpdateLogger> logger
        )
        {
            _logger = logger;
        }

        public Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            bool isWebhook = context.Items.ContainsKey(nameof(HttpContext));

            if (isWebhook)
            {
                var httpContext = (HttpContext) context.Items[nameof(HttpContext)];
                string update = new StreamReader(httpContext.Request.Body).ReadToEnd();

                _logger.LogInformation("New webhook update received at {0}: {1}", httpContext.Request.Host, update);
            }
            else
            {
                string update = JsonConvert.SerializeObject(context.Update, Formatting.Indented);
                _logger.LogInformation("New update received: {0}", update);
            }

            return next(context, cancellationToken);
        }
    }
}