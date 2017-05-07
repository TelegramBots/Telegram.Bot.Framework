using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NetTelegramBot.Framework.Abstractions;

namespace NetTelegramBot.Framework.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseTelegramBotFramework<TBot>(this IApplicationBuilder builder)
            where TBot : IBot
        {
            var botFrameworkBuilder =
                builder.ApplicationServices.GetRequiredService<ITelegramBotFrameworkBuilder<TBot>>();
            
        }
    }
}
