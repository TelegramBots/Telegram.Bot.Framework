using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Framework.Extensions;
using Telegram.Bot.Framework.Middlewares;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Framework
{
    /// <summary>
    /// Extentions for adding Telegram Bot framework to the ASP.NET Core middleware
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
        public static IApplicationBuilder UseTelegramBotWebhook<TBot>(this IApplicationBuilder app, bool ensureWebhookEnabled = true)
            where TBot : BotBase<TBot>
        {
            IBotManager<TBot> botManager = FindBotManager<TBot>(app);

            if (ensureWebhookEnabled)
            {
                botManager.SetWebhookStateAsync(true).Wait();
            }

            return app.UseMiddleware<TelegramBotMiddleware<TBot>>();
        }

        /// <summary>
        /// Removes and disables webhooks for bot
        /// </summary>
        /// <typeparam name="TBot">Type of bot</typeparam>
        /// <param name="app">Instance of IApplicationBuilder</param>
        /// <param name="ensureWebhookDisabled">If true, a request is immediately made to delete webhook</param>
        /// <returns>Instance of IApplicationBuilder</returns>
        public static IApplicationBuilder UseTelegramBotLongPolling<TBot>(this IApplicationBuilder app, bool ensureWebhookDisabled = true)
            where TBot : BotBase<TBot>
        {
            IBotManager<TBot> botManager = FindBotManager<TBot>(app);

            if (ensureWebhookDisabled)
            {
                botManager.SetWebhookStateAsync(false).Wait();
            }

            return app;
        }

        /// <summary>
        /// Add a Telegram game score middleware to the app
        /// </summary>
        /// <typeparam name="TBot">Type of bot</typeparam>
        /// <param name="app">Instance of IApplicationBuilder</param>
        /// <returns>Instance of IApplicationBuilder</returns>
        public static IApplicationBuilder UseTelegramGame<TBot>(this IApplicationBuilder app)
            where TBot : BotBase<TBot>
        {
            app.UseMiddleware<TelegramGameScoreMiddleware<TBot>>();

            return app;
        }

        private static IBotManager<TBot> FindBotManager<TBot>(IApplicationBuilder app)
            where TBot : BotBase<TBot>
        {
            IBotManager<TBot> botManager;
            try
            {
                botManager = app.ApplicationServices.GetRequiredService<IBotManager<TBot>>();
                if (botManager == null)
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception)
            {
                throw new ConfigurationException(
                    "Bot Manager service is not available", string.Format("Use services.{0}<{1}>()",
                        nameof(TelegramBotFrameworkIServiceCollectionExtensions.AddTelegramBot), typeof(TBot).Name));
            }
            return botManager;
        }
    }
}
