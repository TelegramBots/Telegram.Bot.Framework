namespace NetTelegramBot.Framework
{
    using System;

    public interface ICommand
    {
        /// <summary>
        /// Command name (e.g. "start" for "/start")
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Bot name (after @), if any
        /// </summary>
        string BotName { get; }

        /// <summary>
        /// Command args
        /// </summary>
        string[] Args { get; }
    }
}
