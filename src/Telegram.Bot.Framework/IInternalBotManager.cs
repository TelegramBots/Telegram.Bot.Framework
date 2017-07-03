using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.Framework
{
    internal interface IInternalBotManager<TBot> : IBotManager<TBot>
        where TBot : class, IBot
    {
        /// <summary>
        /// Array of game options for this bot's games
        /// </summary>
        BotGameOption[] BotGameOptions { get; }

        /// <summary>
        /// Instance of bot under management
        /// </summary>
        IBot Bot { get; }

        /// <summary>
        /// Finds a handler for game by its short name
        /// </summary>
        /// <param name="gameShortName">Game's short name</param>
        /// <returns>
        /// A tuple with Success indicating presense of a game handler, and GameHandler, instance of
        /// game handler for that game
        /// </returns>
        (bool Success, IGameHandler GameHandler) TryFindGameHandler(string gameShortName);

        /// <summary>
        /// Replaces tokens, if any, in a game url
        /// </summary>
        /// <param name="urlFormat">A url with possibly tokens such as {game}</param>
        /// <param name="gameShortName">Game's short name</param>
        /// <returns>A url with all tokens replaced by their respective values</returns>
        string ReplaceGameUrlTokens(string urlFormat, string gameShortName);
    }
}
