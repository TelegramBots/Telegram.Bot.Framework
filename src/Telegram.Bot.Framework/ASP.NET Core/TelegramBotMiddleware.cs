#if !NETFRAMEWORK

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework
{
    internal class TelegramBotMiddleware<TBot>
        where TBot : BotBase
    {
        private readonly RequestDelegate _next;

        private readonly UpdateDelegate _updateDelegate;

        private readonly ILogger<TelegramBotMiddleware<TBot>> _logger;

        /// <summary>
        /// Initializes an instance of middleware
        /// </summary>
        /// <param name="next">Instance of request delegate</param>
        /// <param name="logger">Logger for this middleware</param>
        public TelegramBotMiddleware(
            RequestDelegate next,
            UpdateDelegate updateDelegate,
            ILogger<TelegramBotMiddleware<TBot>> logger
        )
        {
            _next = next;
            _updateDelegate = updateDelegate;
            _logger = logger;
        }

        /// <summary>
        /// Gets invoked to handle the incoming request
        /// </summary>
        /// <param name="context"></param>
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method != HttpMethods.Post)
            {
                await _next.Invoke(context)
                    .ConfigureAwait(false);
                return;
            }

            string payload;
            using (var reader = new StreamReader(context.Request.Body))
            {
                payload = await reader.ReadToEndAsync()
                    .ConfigureAwait(false);
            }

            _logger.LogDebug("Update payload:\n{0}", payload);

            Update update = null;
            try
            {
                update = JsonConvert.DeserializeObject<Update>(payload);
            }
            catch (JsonException e)
            {
                _logger.LogError($"Unable to deserialize update payload. {e.Message}");
            }

            if (update == null)
            {
                // it is unlikely of Telegram to send an invalid update object.
                // respond with "404 Not Found" in case an attacker is trying to find the webhook URL
                context.Response.StatusCode = 404;
                return;
            }

            using (var scope = context.RequestServices.CreateScope())
            {
                var bot = scope.ServiceProvider.GetRequiredService<TBot>();
                var updateContext = new UpdateContext(bot, update, scope.ServiceProvider);
                updateContext.Items.Add(nameof(HttpContext), context);

                try
                {
                    await _updateDelegate(updateContext)
                        .ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error occured while handling update `{update.Id}`. {e.Message}");
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                }
            }

            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = 201;
            }
        }
    }
}

#endif
