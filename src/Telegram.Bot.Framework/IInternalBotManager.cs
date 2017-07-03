using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.Framework
{
    internal interface IInternalBotManager<TBot> : IBotManager<TBot>
        where TBot : class, IBot
    {
        BotGameOption[] BotGameOptions { get; }

        IBot Bot { get; }

        (bool Success, IUpdateHandler gameUpdateHandler) TryFindGameUpdateHandler(string gameShortName);

        string ReplaceGameUrlTokens(string urlFormat, string gameShortName);
    }
}
