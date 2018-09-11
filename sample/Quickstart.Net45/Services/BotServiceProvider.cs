using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Linq;
using Telegram.Bot.Abstractions;
using Telegram.Bot.Framework;

namespace Quickstart.Net45
{
    class BotServiceProvider<TBot> : IBotServiceProvider<TBot>
        where TBot : class, IBot
    {
        private readonly Container _container;

        private Scope _scope;

        public BotServiceProvider(Container container)
        {
            _container = container;
        }

        public IBotServiceProvider<TBot> CreateScope()
        {
            _scope = ThreadScopedLifestyle.BeginScope(_container);
            return this;
        }

        public TBot GetBot() => _container.GetInstance<TBot>();

        public bool TryGetBotOptions(out IBotOptions options)
        {
            var reg = _container.GetCurrentRegistrations()
                .SingleOrDefault(x => x.ServiceType == typeof(BotOptions<TBot>));
            if (reg != null)
            {
                options = (IBotOptions)reg.GetInstance();
            }
            else
            {
                options = null;
            }

            return options != null;
        }

        public IHandlersAccessor<TBot> GetHandlerTypes() => _container.GetInstance<IHandlersAccessor<TBot>>();

        public IUpdateHandler GetHandler(Type t) => (IUpdateHandler)_scope.GetInstance(t);

        public void Dispose() => _scope?.Dispose();
    }
}
