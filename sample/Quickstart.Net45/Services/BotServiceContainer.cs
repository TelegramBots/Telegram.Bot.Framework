using SimpleInjector;
using System;
using System.Collections.Generic;
using Telegram.Bot.Abstractions;
using Telegram.Bot.Framework;

namespace Quickstart.Net45
{
    class BotServiceContainer<TBot> : IBotServiceContainer<TBot>
        where TBot : class, IBot
    {
        private readonly Container _container;

        private Func<TBot> _botCreator;

        private IList<Type> _handlerTypes;

        private IList<Action<Container>> _handlerRegisters;

        public BotServiceContainer(Container container)
        {
            _container = container;
        }

        public IBotServiceContainer<TBot> Bot(Func<TBot> instanceCreator)
        {
            _botCreator = instanceCreator;

            _handlerTypes = new List<Type>();
            _handlerRegisters = new List<Action<Container>>();

            return this;
        }

        public IBotServiceContainer<TBot> Use<THandler>()
            where THandler : class, IUpdateHandler
        {
            _handlerTypes.Add(typeof(THandler));
            _handlerRegisters.Add(
                c => c.Register(typeof(THandler))
            );
            return this;
        }

        public IBotServiceContainer<TBot> Use<THandler>(Func<THandler> creator)
            where THandler : class, IUpdateHandler
        {
            _handlerTypes.Add(typeof(THandler));
            _handlerRegisters.Add(
                c => c.Register(creator)
            );
            return this;
        }

        public IBotUpdateManager<TBot> Register()
        {
            _container.Register(_botCreator);

            foreach (var action in _handlerRegisters)
            {
                action(_container);
            }
            _container.Register<IHandlersAccessor<TBot>>(() => new HandlersAccessor<TBot>(_handlerTypes));

            _container.Verify();

            var provider = new BotServiceProvider<TBot>(_container);

            return new BotUpdateManager<TBot>(provider);
        }
    }
}
