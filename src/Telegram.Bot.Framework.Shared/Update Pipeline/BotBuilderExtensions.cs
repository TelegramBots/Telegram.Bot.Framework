using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.Framework
{
    public static class BotBuilderExtensions
    {
        public static IBotBuilder UseCommand<TCommand>(this IBotBuilder builder, string command)
            where TCommand : CommandBase
            => builder
                .MapWhen(
                    ctx => ctx.Bot.CanHandleCommand(command, ctx.Update.Message),
                    botBuilder => botBuilder.Use<TCommand>()
                );
    }
}
