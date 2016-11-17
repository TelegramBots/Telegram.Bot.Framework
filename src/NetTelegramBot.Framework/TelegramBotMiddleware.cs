namespace NetTelegramBot.Framework
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    public class TelegramBotMiddleware<TBot>
        where TBot : BotBase
    {
        private readonly RequestDelegate next;

        private readonly string requestPath;

        private TBot bot;

        public TelegramBotMiddleware(RequestDelegate next, PathString requestPath)
        {
            this.next = next;
            this.requestPath = requestPath;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!string.Equals(context.Request.Path, requestPath, StringComparison.OrdinalIgnoreCase))
            {
                await next.Invoke(context);
                return;
            }

            if (bot == null)
            {
                bot = context.RequestServices.GetRequiredService<TBot>();
            }

            await bot.ProcessIncomingWebhookAsync(context.Request.Body);

            context.Response.StatusCode = StatusCodes.Status200OK;
        }
    }
}
