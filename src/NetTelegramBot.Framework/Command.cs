namespace NetTelegramBot.Framework
{
    using System;

    public class Command : ICommand
    {
        /// <summary>
        /// Command name (e.g. "start" for "/start")
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Bot name (after @), if any
        /// </summary>
        public string BotName { get; set; }

        /// <summary>
        /// Command args
        /// </summary>
        public string[] Args { get; set; }
    }
}
