namespace Telegram.Bot.Framework.Abstractions
{
    /// <summary>
    /// Indicates the result of an update processing action
    /// </summary>
    public enum UpdateHandlingResult
    {
        /// <summary>
        /// Handling the update should continue with the next available handler
        /// </summary>
        Continue,

        /// <summary>
        /// Update is handled completely
        /// </summary>
        Handled,
    }
}
