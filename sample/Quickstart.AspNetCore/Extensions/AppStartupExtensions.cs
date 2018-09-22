using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quickstart.AspNetCore;
using System;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace Microsoft.AspNetCore.Builder
{
    static class AppStartupExtensions
    {
        public static IApplicationBuilder EnsureWebhookSet<TBot>(
            this IApplicationBuilder app,
            string baseUrl
        )
            where TBot : IBot
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
                var bot = scope.ServiceProvider.GetRequiredService<TBot>();
                var options = scope.ServiceProvider.GetRequiredService<IOptions<BotOptions<TBot>>>();
                var url = new Uri(new Uri(baseUrl), options.Value.WebhookPath);

                logger.LogInformation("Setting webhook for bot \"{0}\" to URL \"{1}\"", typeof(TBot).Name, url);

                bot.Client.SetWebhookAsync(url.AbsoluteUri)
                    .GetAwaiter().GetResult();
            }

            return app;
        }
    }
}
