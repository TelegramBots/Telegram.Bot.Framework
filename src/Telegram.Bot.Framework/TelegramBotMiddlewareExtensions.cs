using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Framework.Extensions;

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
        public static IApplicationBuilder UseTelegramBotWebhook<TBot>(this IApplicationBuilder app, bool ensureWebhookEnabled = false)
            where TBot : BotBase<TBot>
        {
            IBotManager<TBot> botManager = FindBotManager<TBot>(app);

            if (ensureWebhookEnabled)
            {
                botManager.SetWebhook().Wait();
            }

            return app.UseMiddleware<TelegramBotMiddleware<TBot>>();
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
                        nameof(NetTelegramBotFrameworkIServiceCollectionExtensions.AddTelegramBot), typeof(TBot).Name));
            }
            return botManager;
        }
    }
}
