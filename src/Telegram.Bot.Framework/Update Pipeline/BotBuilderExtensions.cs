using System;
using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.Framework
{
    public static class BotBuilderExtensions
    {
        public static IBotBuilder UseWhen(
            this IBotBuilder builder,
            Predicate<IUpdateContext> predicate,
            Action<IBotBuilder> configure
        )
        {
            builder.Use(
                next =>
                    (context, cancellationToken) =>
                    {
                        if (predicate(context))
                        {
                            var branchBuilder = new BotBuilder();
                            configure(branchBuilder);
                            branchBuilder.Use(next);
                            var branchDelegate = branchBuilder.Build();

                            return branchDelegate(context, cancellationToken);
                        }

                        return next(context, cancellationToken);
                    }
            );
            return builder;
        }

        public static IBotBuilder UseWhen<THandler>(
            this IBotBuilder builder,
            Predicate<IUpdateContext> predicate
        )
            where THandler : IUpdateHandler
        {
            builder.UseWhen(predicate, botBuilder => botBuilder.Use<THandler>());
            return builder;
        }

        public static IBotBuilder UseCommand<TCommand>(
            this IBotBuilder builder,
            string command
        )
            where TCommand : CommandBase
            => builder
                .UseWhen(
                    ctx => ctx.Bot.CanHandleCommand(command, ctx.Update.Message),
                    botBuilder => botBuilder.Use<TCommand>()
                );
    }
}