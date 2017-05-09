using System;

namespace NetTelegram.Bot.Framework.Extensions
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException()
        {

        }

        public ConfigurationException(string message)
            : base(message)
        {

        }

        public ConfigurationException(string message, string hint)
            : base(message + "\n" + hint)
        {

        }
    }
}
