using System;

namespace NetTelegramBot.Framework
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
