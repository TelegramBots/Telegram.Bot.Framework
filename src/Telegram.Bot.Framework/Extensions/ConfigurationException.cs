using System;

namespace Telegram.Bot.Framework.Extensions
{
    /// <summary>
    /// Represents errors in Telegram Bot Configurations 
    /// </summary>
    public class ConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a configuration exception with default values
        /// </summary>
        internal ConfigurationException()
        {

        }

        /// <summary>
        /// Initializes a configuration exception with the specified message
        /// </summary>
        /// <param name="message">Message for the occured error</param>
        internal ConfigurationException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Initializes a configuration exception with specified message and a hint for possible fix
        /// </summary>
        /// <param name="message">Message for the occured error</param>
        /// <param name="hint">A hint for fixing the error</param>
        internal ConfigurationException(string message, string hint)
            : base(message + "\n" + hint)
        {

        }
    }
}
