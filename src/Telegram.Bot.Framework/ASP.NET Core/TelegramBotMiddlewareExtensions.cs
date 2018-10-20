#if !NETFRAMEWORK

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extenstion methods for adding Telegram Bot framework to the ASP.NET Core middleware
    /// </summary>
    public static class TelegramBotMiddlewareExtensions
    {
        /// <summary>
        /// Add Telegram bot webhook handling functionality to the pipeline
        /// </summary>
        /// <typeparam name="TBot">Type of bot</typeparam>
        /// <param name="app">Instance of IApplicationBuilder</param>
        /// <param name="ensureWebhookEnabled">Whether to set the webhook immediately by making a request to Telegram bot API</param>
        /// <returns>Instance of IApplicationBuilder</returns>
        public static IApplicationBuilder UseTelegramBotWebhook<TBot>(
            this IApplicationBuilder app,
            IBotBuilder botBuilder
        )
            where TBot : BotBase
        {
            var updateDelegate = botBuilder.Build();

            var options = app.ApplicationServices.GetRequiredService<IOptions<BotOptions<TBot>>>();
            app.Map(
                options.Value.WebhookPath,
                builder => builder.UseMiddleware<TelegramBotMiddleware<TBot>>(updateDelegate)
            );

            return app;
        }

        ///// <summary>
        ///// Removes and disables webhooks for bot
        ///// </summary>
        ///// <typeparam name="TBot">Type of bot</typeparam>
        ///// <param name="app">Instance of IApplicationBuilder</param>
        ///// <param name="ensureWebhookDisabled">If true, a request is immediately made to delete webhook</param>
        ///// <returns>Instance of IApplicationBuilder</returns>
        //public static IApplicationBuilder UseTelegramBotLongPolling<TBot>(this IApplicationBuilder app, bool ensureWebhookDisabled = true)
        //    where TBot : BotBase<TBot>
        //{
        //    IBotManager<TBot> botManager = FindBotManager<TBot>(app);

        //    if (ensureWebhookDisabled)
        //    {
        //        botManager.SetWebhookStateAsync(false).Wait();
        //    }

        //    return app;
        //}

        ///// <summary>
        ///// Add a Telegram game score middleware to the app
        ///// </summary>
        ///// <typeparam name="TBot">Type of bot</typeparam>
        ///// <param name="app">Instance of IApplicationBuilder</param>
        ///// <returns>Instance of IApplicationBuilder</returns>
        //public static IApplicationBuilder UseTelegramGame<TBot>(this IApplicationBuilder app)
        //    where TBot : BotBase<TBot>
        //{
        //    app.UseMiddleware<TelegramGameScoreMiddleware<TBot>>();

        //    return app;
        //}

        //private static IBotManager<TBot> FindBotManager<TBot>(IApplicationBuilder app)
        //    where TBot : BotBase<TBot>
        //{
        //    IBotManager<TBot> botManager;
        //    try
        //    {
        //        botManager = app.ApplicationServices.GetRequiredService<IBotManager<TBot>>();
        //        if (botManager == null)
        //        {
        //            throw new NullReferenceException();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw new ConfigurationException(
        //            "Bot Manager service is not available", string.Format("Use services.{0}<{1}>()",
        //                nameof(TelegramBotFrameworkIServiceCollectionExtensions.AddTelegramBot), typeof(TBot).Name));
        //    }
        //    return botManager;
        //}
    }
}

#endif