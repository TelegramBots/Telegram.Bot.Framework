namespace Telegram.Bot.Framework
{
    /// <summary>
    /// Telegram game options
    /// </summary>
    public class BotGameOption
    {
        /// <summary>
        /// Game's short name
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Url to the game's HTML5 page
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Game's callback url for getting or setting high scores
        /// </summary>
        public string ScoresUrl { get; set; }
    }
}
