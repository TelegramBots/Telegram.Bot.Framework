namespace NetTelegramBot.Framework
{
    using System;

    public class BotCommand
    {
        public string Command { get; set; }

        public string Bot { get; set; }

        public string[] Args { get; set; }
    }
}
