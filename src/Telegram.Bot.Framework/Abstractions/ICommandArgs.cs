namespace Telegram.Bot.Framework.Abstractions
{
    /// <summary>
    /// Stores arguments for a bot command
    /// </summary>
    public interface ICommandArgs
    {
        /// <summary>
        /// Raw user's text input
        /// </summary>
        string RawInput { get; set; }

        /// <summary>
        /// Text input whithout the command part
        /// </summary>
        /// <example>
        /// "argument" in "/command@bot argument"
        /// </example>
        string ArgsInput { get; set; }
    }
}
