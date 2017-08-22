using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Middlewares
{
    /// <summary>
    /// Middleware for handling Telegram bot's webhook requests
    /// </summary>
    /// <typeparam name="TBot">Type of bot</typeparam>
    public class TelegramBotMiddleware<TBot>
        where TBot : BotBase<TBot>
    {
        private readonly RequestDelegate _next;

        private readonly IBotManager<TBot> _botManager;

        private readonly ILogger<TelegramBotMiddleware<TBot>> _logger;

        /// <summary>
        /// Initializes an instance of middleware
        /// </summary>
        /// <param name="next">Instance of request delegate</param>
        /// <param name="botManager">Bot manager for the bot</param>
        /// <param name="logger">Logger for this middleware</param>
        public TelegramBotMiddleware(RequestDelegate next, 
            IBotManager<TBot> botManager,
            ILogger<TelegramBotMiddleware<TBot>> logger)
        {
            _next = next;
            _botManager = botManager;
            _logger = logger;
        }

        /// <summary>
        /// Gets invoked to handle the incoming request
        /// </summary>
        /// <param name="context"></param>
        public async Task Invoke(HttpContext context)
        {
            if (!(
                context.Request.Method == HttpMethods.Post &&
                _botManager.WebhookUrl.EndsWith(context.Request.Path)
                ))
            {
                await _next.Invoke(context);
                return;
            }
            
            string data;
            using (var reader = new StreamReader(context.Request.Body))
            {
                data = await reader.ReadToEndAsync();
            }

            _logger.LogTrace($"Update Data:`{data}`");

            Update update = null;
            try
            {
                update = JsonConvert.DeserializeObject<Update>(data);
            }
            catch (JsonException e)
            {
                _logger.LogWarning($"Unable to deserialize update payload. {e.Message}");
            }
            if (update == null)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }
            
            try
            {
                await _botManager.HandleUpdateAsync(update);
                context.Response.StatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error occured while handling update `{update.Id}`. {e.Message}");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}
