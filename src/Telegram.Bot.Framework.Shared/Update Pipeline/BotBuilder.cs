using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.Framework
{
    public class BotBuilder : IBotBuilder
    {
        internal UpdateDelegate UpdateDelegate { get; private set; }

        private readonly ICollection<Func<UpdateDelegate, UpdateDelegate>> _components;

        public BotBuilder()
        {
            _components = new List<Func<UpdateDelegate, UpdateDelegate>>();
        }

        public IBotBuilder Use(Func<UpdateDelegate, UpdateDelegate> middleware)
        {
            throw new NotImplementedException();
        }

        public IBotBuilder Use<THandler>()
            where THandler : IUpdateHandler
        {
            _components.Add(
                next =>
                context =>
                ((IUpdateHandler)context.Services.GetService(typeof(THandler)))
                    .HandleAsync(context, next)
            );

            return this;
        }

        public IBotBuilder Use<THandler>(THandler handler)
            where THandler : IUpdateHandler
        {
            _components.Add(next =>
                context => handler.HandleAsync(context, next)
            );

            return this;
        }

        public IBotBuilder Use(Func<IUpdateContext, UpdateDelegate> component)
        {
            throw new NotImplementedException();
        }

        public UpdateDelegate Build()
        {
            UpdateDelegate handle = context => Task.FromResult(null as object);
            foreach (var component in _components.Reverse())
            {
                handle = component(handle);
            }

            return UpdateDelegate = handle;
        }
    }
}
