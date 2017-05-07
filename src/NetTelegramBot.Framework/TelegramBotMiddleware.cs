using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace NetTelegramBot.Framework
{
    public class TelegramBotMiddleware<TBot>
//        where TBot : BotBase
    {
        private readonly RequestDelegate _next;

        private readonly string _requestPath;

        private TBot _bot;

        public TelegramBotMiddleware(RequestDelegate next, PathString requestPath)
        {
            this._next = next;
            this._requestPath = requestPath;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!string.Equals(context.Request.Path, _requestPath, StringComparison.OrdinalIgnoreCase))
            {
                await _next.Invoke(context);
                return;
            }

            if (_bot == null)
            {
                _bot = context.RequestServices.GetRequiredService<TBot>();
            }

            //await _bot.ProcessIncomingWebhookAsync(context.Request.Body);

            context.Response.StatusCode = StatusCodes.Status200OK;
        }
    }
}
