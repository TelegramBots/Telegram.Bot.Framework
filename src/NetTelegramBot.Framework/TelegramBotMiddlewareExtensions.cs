namespace Microsoft.AspNetCore.Builder
{
    using System;
    using Http;
    using NetTelegramBot.Framework;

    public static class TelegramBotMiddlewareExtensions
    {
        public static IApplicationBuilder UseTelegramBot<TBot>(this IApplicationBuilder app, PathString requestPath)
            //where TBot : BotBase
        {
            return UseMiddlewareExtensions.UseMiddleware<TelegramBotMiddleware<TBot>>(
                app,
                new object[] { requestPath });
        }
    }
}
