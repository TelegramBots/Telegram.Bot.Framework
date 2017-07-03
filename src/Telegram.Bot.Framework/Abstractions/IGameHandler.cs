using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstractions
{
    /// <summary>
    /// Update handler for Telegram game updates and game Score requests from game HTML5 page
    /// </summary>
    public interface IGameHandler : IUpdateHandler
    {
        /// <summary>
        /// Game's short name
        /// </summary>
        string ShortName { get; }

        /// <summary>
        /// Game configuration options
        /// </summary>
        BotGameOption Options { get; set; }

        /// <summary>
        /// Set game score for user based on encrypted playerid
        /// </summary>
        /// <param name="bot">Instance of the bot</param>
        /// <param name="playerid">Encoded and protected player id</param>
        /// <param name="score">User's score</param>
        Task SetGameScoreAsync(IBot bot, string playerid, int score);

        /// <summary>
        /// Get game scores for chat based on encrypted playerid
        /// </summary>
        /// <param name="bot">Instance of the bot</param>
        /// <param name="playerid">Encoded and protected player id</param>
        /// <returns>Array of scores for chat</returns>
        Task<GameHighScore[]> GetHighestScoresAsync(IBot bot, string playerid);
    }
}
