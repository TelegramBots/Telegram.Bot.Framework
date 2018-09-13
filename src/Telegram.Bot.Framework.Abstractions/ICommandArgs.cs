namespace Telegram.Bot.Framework.Abstractions
{
    /// <summary>
    /// Stores arguments for a bot command
    /// </summary>
    public interface ICommandArgs
    {
        string Name { get; }

        /// <summary>
        /// Text input whiteout the command part
        /// </summary>
        /// <example>
        /// "args1 arg2..." in "/command@bot args1 arg2..."
        /// </example>
        string Arguments { get; }
    }
}
