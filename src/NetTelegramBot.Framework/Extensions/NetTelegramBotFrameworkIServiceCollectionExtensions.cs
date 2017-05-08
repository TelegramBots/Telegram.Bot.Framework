using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NetTelegramBot.Framework;
using NetTelegramBot.Framework.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class NetTelegramBotFrameworkIServiceCollectionExtensions
    {
        public static IServiceCollection Services;

        public static ITelegramBotFrameworkBuilder<TBot> AddTelegramBot<TBot>
            (this IServiceCollection services, BotOptions<TBot> botOptions)
            where TBot : BotBase<TBot>
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (botOptions == null)
            {
                throw new ArgumentNullException(nameof(botOptions));
            }

            Services = services;
            return new TelegramBotFrameworkBuilder<TBot>(botOptions);
        }

        public static ITelegramBotFrameworkBuilder<TBot> AddTelegramBot<TBot>
            (this IServiceCollection services, IConfiguration config)
            where TBot : BotBase<TBot>
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            Services = services;
            return new TelegramBotFrameworkBuilder<TBot>(config);
        }

        public interface ITelegramBotFrameworkBuilder<TBot>
            where TBot : BotBase<TBot>
        {
            ITelegramBotFrameworkBuilder<TBot> AddUpdateHandler<T>()
                where T : class, IUpdateHandler;

            IServiceCollection Configure();
        }

        public class TelegramBotFrameworkBuilder<TBot> : ITelegramBotFrameworkBuilder<TBot>
            where TBot : BotBase<TBot>
        {
            private readonly List<Type> _handlerTypes = new List<Type>();

            private readonly BotOptions<TBot> _botOptions;

            private readonly IConfiguration _configuration;

            public TelegramBotFrameworkBuilder(BotOptions<TBot> botOptions)
            {
                _botOptions = botOptions;
            }

            public TelegramBotFrameworkBuilder(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public ITelegramBotFrameworkBuilder<TBot> AddUpdateHandler<T>()
                where T : class, IUpdateHandler
            {
                _handlerTypes.Add(typeof(T));
                return this;
            }

            public IServiceCollection Configure()
            {
                EnsureValidConfiguration();

                if (_botOptions != null)
                {
                    Services.Configure<BotOptions<TBot>>(x =>
                    {
                        x.ApiToken = _botOptions.ApiToken;
                        x.BotName = _botOptions.BotName;
                        x.UseWebhook = _botOptions.UseWebhook;
                        x.WebhookUrl = _botOptions.WebhookUrl;
                    });
                }
                else
                {
                    Services.Configure<BotOptions<TBot>>(_configuration);
                }

                Services.AddScoped<TBot>();

                _handlerTypes.ForEach(x => Services.AddTransient(x));

                Services.AddScoped<IUpdateHandlersAccessor<TBot>>(factory =>
                {
                    var handlers = _handlerTypes.Select(x => (IUpdateHandler)factory.GetRequiredService(x)).ToArray();
                    return new UpdateHanldersAccessor<TBot>(handlers);
                });

                Services.AddScoped<IUpdateParser<TBot>, UpdateParser<TBot>>();

                Services.AddScoped<IBotManager<TBot>, BotManager<TBot>>();

                return Services;
            }

            private void EnsureValidConfiguration()
            {
                if (!_handlerTypes.Any())
                {
                    throw new ConfigurationException("No update handler is provided", $"Use {nameof(AddUpdateHandler)} method");
                }
                // ToDo: Validate others
            }
        }
    }
}
