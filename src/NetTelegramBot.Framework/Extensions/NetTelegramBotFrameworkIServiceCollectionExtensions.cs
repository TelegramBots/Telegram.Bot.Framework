using System;
using System.Collections.Generic;
using System.Linq;
using NetTelegramBot.Framework;
using NetTelegramBot.Framework.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class NetTelegramBotFrameworkIServiceCollectionExtensions
    {
        public static IServiceCollection Services;

        public static ITelegramBotFrameworkBuilder<TBot> AddTelegramBot<TBot>
            (this IServiceCollection services, IBotOptions<TBot> botOptions)
            where TBot : class, IBot
        {
            Services = services;
            Services.AddScoped(_ => botOptions);
            return new TelegramBotFrameworkBuilder<TBot>();
        }

        public interface ITelegramBotFrameworkBuilder<TBot>
            where TBot : class, IBot
        {
            ITelegramBotFrameworkBuilder<TBot> AddUpdateHandler<T>()
                where T : class, IUpdateHandler;

            IServiceCollection Configure();
        }

        public class TelegramBotFrameworkBuilder<TBot> : ITelegramBotFrameworkBuilder<TBot>
            where TBot : class, IBot
        {
            private readonly List<Type> _handlerTypes = new List<Type>();

            public ITelegramBotFrameworkBuilder<TBot> AddUpdateHandler<T>()
                where T : class, IUpdateHandler
            {
                Services.AddTransient<T>();
                _handlerTypes.Add(typeof(T));
                return this;
            }

            public IServiceCollection Configure()
            {
                Services.AddScoped<IUpdateHandlersAccessor<TBot>>(factory =>
                {
                    var handlers = _handlerTypes.Select(x => (IUpdateHandler)factory.GetRequiredService(x)).ToArray();
                    return new UpdateHanldersAccessor<TBot>(handlers);
                });

                Services.AddScoped<IUpdateParser<TBot>, UpdateParser<TBot>>();
                Services.AddScoped<TBot>();
                Services.AddScoped<IBotUpdateManager<TBot>, BotUpdateManager<TBot>>();

                return Services;
            }
        }
    }
}
