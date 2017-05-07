using System;
using System.Collections.Generic;
using System.Linq;
using NetTelegramBot.Framework;
using NetTelegramBot.Framework.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TelegramBotFrameworkIServiceCollectionExtensions
    {
        public static IServiceCollection Services;

        public static IFBuilder1<TIBot, TBot> AddTelegramBot<TIBot, TBot>
            (this IServiceCollection services)
            where TBot : class, TIBot
            where TIBot : class, IBot
        {
            Services = services;
            Services.AddScoped<TIBot, TBot>();
            return new FBuilder1<TIBot, TBot>();
        }

        #region Interfaces

        public interface IFBuilder1<TIBot, TBot>
            where TBot : TIBot
            where TIBot : IBot
        {
            IFBuilder2<TIBot, TBot> AddBotOptions(IBotOptions<TIBot> botOptions);
        }

        public interface IFBuilder2<TIBot, TBot>
            where TBot : TIBot
            where TIBot : IBot
        {
            IFBuilder3<TIBot, TBot> AddDefaultMessageParser();

            IFBuilder3<TIBot, TBot> AddMessageParser<TMessageParser>()
                where TMessageParser : IMessageParser<TIBot>;
        }

        public interface IFBuilder3<TIBot, TBot>
            where TBot : TIBot
            where TIBot : IBot
        {
            IFBuilder3<TIBot, TBot> AddMessageHandler<TIMessageHandler, TMessageHandler>()
                where TIMessageHandler : class, IMessageHandler<TIBot>
                where TMessageHandler : class, TIMessageHandler;

            IServiceCollection AddBotManager<TBotManager>()
                where TBotManager : IBotUpdateManager<TIBot>;

            IServiceCollection AddDefaultUpdateManager();
        }

        #endregion

        #region Implementations

        public class FBuilder1<TIBot, TBot> : IFBuilder1<TIBot, TBot>
            where TBot : TIBot
            where TIBot : IBot
        {
            public IFBuilder2<TIBot, TBot> AddBotOptions(IBotOptions<TIBot> botOptions)
            {
                Services.AddTransient(_ => botOptions);
                return new FBuilder2<TIBot, TBot>();
            }
        }

        public class FBuilder2<TIBot, TBot> : IFBuilder2<TIBot, TBot>
            where TBot : TIBot
            where TIBot : IBot
        {
            public IFBuilder3<TIBot, TBot> AddDefaultMessageParser()
            {
                Services.AddScoped<IMessageParser<TIBot>, MessageParser<TIBot>>();
                return new FBuilder3<TIBot, TBot>();
            }

            IFBuilder3<TIBot, TBot> IFBuilder2<TIBot, TBot>.AddMessageParser<TMessageParser>()
            {
                // ToDo
                throw new NotImplementedException();
            }
        }

        public class FBuilder3<TIBot, TBot> : IFBuilder3<TIBot, TBot>
            where TBot : TIBot
            where TIBot : IBot
        {
            private readonly List<Type> _handlerTypes = new List<Type>();

            public IFBuilder3<TIBot, TBot> AddMessageHandler<TIMessageHandler, TMessageHandler>()
                where TIMessageHandler : class, IMessageHandler<TIBot>
                where TMessageHandler : class, TIMessageHandler
            {
                Services.AddTransient<TIMessageHandler, TMessageHandler>();
                _handlerTypes.Add(typeof(TIMessageHandler));
                return this;
            }

            public IServiceCollection AddBotManager<TBotManager>() where TBotManager : IBotUpdateManager<TIBot>
            {
                // ToDo 
                throw new NotImplementedException();
            }

            public IServiceCollection AddDefaultUpdateManager()
            {
                Services.AddScoped<IMessageHandlersAccessor<TIBot>>(factory =>
                {
                    var handlers = _handlerTypes.Select(x => (IMessageHandler<TIBot>)factory.GetRequiredService(x)).ToArray();
                    return new MessageHanlderAccessor<TIBot>(handlers);
                });

                Services.AddScoped<IBotUpdateManager<TIBot>, BotUpdateManager<TIBot>>();

                return Services;
            }
        }

        #endregion
    }
}
