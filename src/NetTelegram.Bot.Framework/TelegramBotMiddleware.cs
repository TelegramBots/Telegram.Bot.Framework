using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NetTelegram.Bot.Framework.Abstractions;
using NetTelegramBotApi.Types;
using NetTelegramBotApi.Util;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;

namespace NetTelegram.Bot.Framework
{
    /// <summary>
    /// Middleware for handling Telegram bot's webhook requests in an ASP.NET Core app
    /// </summary>
    /// <typeparam name="TBot">Type of bot</typeparam>
    public class TelegramBotMiddleware<TBot>
        where TBot : BotBase<TBot>
    {
        private readonly RequestDelegate _next;

        private readonly IBotManager<TBot> _botManager;

        /// <summary>
        /// Initialize and instance of middleware
        /// </summary>
        /// <param name="next">Instance of request delegate</param>
        /// <param name="botManager">Bot manager for the bot</param>
        public TelegramBotMiddleware(RequestDelegate next, IBotManager<TBot> botManager)
        {
            _next = next;
            _botManager = botManager;
        }

        /// <summary>
        /// Gets invoked to handle the incoming request
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            if (!_botManager.WebhookUrl.Contains(context.Request.Path)
                ||
                !context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                await _next.Invoke(context);
                return;
            }

            ILogger logger = context.RequestServices.GetRequiredService<ILogger<TelegramBotMiddleware<TBot>>>();

            string data;
            using (var reader = new StreamReader(context.Request.Body))
            {
                data = await reader.ReadToEndAsync();
            }

            logger.LogTrace($"Update Data:`{data}`");

            Update update;

            try
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new JsonLowerCaseUnderscoreContractResolver(),
                    Converters = new List<JsonConverter> { new UnixDateTimeConverter() }
                };
                update = JsonConvert.DeserializeObject<Update>(data, settings);
                if (update == null)
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            try
            {
                var botManager = context.RequestServices.GetRequiredService<IBotManager<TBot>>();
                await botManager.HandleUpdateAsync(update);
                context.Response.StatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}
