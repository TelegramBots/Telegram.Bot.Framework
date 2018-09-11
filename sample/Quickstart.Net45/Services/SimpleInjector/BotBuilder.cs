using SimpleInjector;
using System;
using System.Collections.Generic;
using Telegram.Bot.Abstractions;
using Telegram.Bot.Framework;

namespace Quickstart.Net45.Services.SimpleInjector
{
    class BotBuilder<TBot>
        where TBot : class, IBot
    {
        private readonly Container _container;

        private Func<TBot> _botCreator;

        private IList<IHandlerPredicate> _handlerPredicates;

        private IList<Action<Container>> _handlerRegisters;

        public BotBuilder(Container container)
        {
            _container = container;
        }

        public BotBuilder<TBot> Bot(Func<TBot> instanceCreator)
        {
            _botCreator = instanceCreator;

            _handlerPredicates = new List<IHandlerPredicate>();
            _handlerRegisters = new List<Action<Container>>();

            return this;
        }

        public BotBuilder<TBot> Use<THandler>()
            where THandler : class, IUpdateHandler
        {
            _handlerPredicates.Add(new HandlerPredicate(typeof(THandler), (bot, context) => true));
            _handlerRegisters.Add(
                c => c.Register(typeof(THandler))
            );
            return this;
        }

        public BotBuilder<TBot> UseCommand<TCommand>()
            where TCommand : class, IBotCommand
        {
            var type = typeof(TCommand);
            string commandName = type.Name.ToLower();
            if (commandName.EndsWith("command"))
            {
                commandName = commandName.Remove(commandName.Length - 7, 7);
            }

            _handlerPredicates.Add(new HandlerPredicate(
                type,
                (bot, context) => CommandBase.CanHandle(bot, context, commandName)
            ));
            _handlerRegisters.Add(
                c => c.Register(typeof(TCommand))
            );
            return this;
        }

        public BotBuilder<TBot> Use<THandler>(Func<THandler> creator)
            where THandler : class, IUpdateHandler
        {
            _handlerPredicates.Add(new HandlerPredicate(typeof(THandler), (bot, context) => true));
            _handlerRegisters.Add(
                c => c.Register(creator)
            );
            return this;
        }

        public BotBuilder<TBot> UseWhen<THandler>(Func<IBot, IUpdateContext, bool> predicate)
            where THandler : class, IUpdateHandler
        {
            _handlerPredicates.Add(new HandlerPredicate(typeof(THandler), predicate));
            _handlerRegisters.Add(c => c.Register(typeof(THandler)));
            return this;
        }

        public BotBuilder<TBot> UseWhen<THandler>(Func<THandler> creator, Func<IBot, IUpdateContext, bool> predicate)
            where THandler : class, IUpdateHandler
        {
            _handlerPredicates.Add(new HandlerPredicate(typeof(THandler), predicate));
            _handlerRegisters.Add(c => c.Register(creator));
            return this;
        }

        public BotUpdateManager<TBot> Register()
        {
            _container.Register(_botCreator);

            foreach (var action in _handlerRegisters)
            {
                action(_container);
            }
            _container.Register<IHandlersCollection<TBot>>(() => new HandlersCollection<TBot>(_handlerPredicates));

            _container.Verify();

            var provider = new BotServiceProvider<TBot>(_container);

            return new BotUpdateManager<TBot>(provider);
        }
    }
}
