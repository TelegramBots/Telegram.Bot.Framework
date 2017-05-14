using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NetTelegram.Bot.Framework.Abstractions;
using NetTelegramBotApi.Types;
using Newtonsoft.Json;

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
            if (!string.Equals(context.Request.Path, _botManager.WebhookRoute, StringComparison.OrdinalIgnoreCase)
                ||
                !context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                await _next.Invoke(context);
                return;
            }

            string data;
            using (var reader = new StreamReader(context.Request.Body))
            {
                data = await reader.ReadToEndAsync();
            }

            Update update;
            try
            {
                update = JsonConvert.DeserializeObject<Update>(data);
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
