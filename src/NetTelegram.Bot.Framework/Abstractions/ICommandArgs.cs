namespace NetTelegram.Bot.Framework.Abstractions
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
    }
}
