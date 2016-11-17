namespace Microsoft.AspNetCore.Builder
{
    using System;
    using Http;
    using NetTelegramBot.Framework;

    public static class TelegramBotMiddlewareExtensions
    {
        public static IApplicationBuilder UseTelegramBot<TBot>(this IApplicationBuilder app, PathString requestPath)
            where TBot : BotBase
        {
            return UseMiddlewareExtensions.UseMiddleware<TelegramBotMiddleware<TBot>>(
                app,
                new object[] { requestPath });
        }

        //public static IApplicationBuilder UseTelegramBot<TBot>(this IApplicationBuilder app, PathString requestPath, Uri endpoint, string certificatePublicKey)
        //    where TBot : BotBase
        //{
        //    var options = new TelegramBotOptions
        //    {
        //        RequestPath = requestPath,
        //        SetWebhook = true,
        //        Endpoint = endpoint,
        //        CertificatePublicKey = certificatePublicKey
        //    };

        //    return UseTelegramBot<TBot>(app, options);
        //}

        //public static IApplicationBuilder UseTelegramBot<TBot>(this IApplicationBuilder app, TelegramBotOptions options)
        //    where TBot : BotBase
        //{
        //    if (app == null)
        //    {
        //        throw new ArgumentNullException("app");
        //    }

        //    if (options == null)
        //    {
        //        throw new ArgumentNullException("options");
        //    }

        //    return UseMiddlewareExtensions.UseMiddleware<TelegramBotMiddleware<TBot>>(
        //        app,
        //        new object[] { Options.Create(options) });
        //}
    }
}
