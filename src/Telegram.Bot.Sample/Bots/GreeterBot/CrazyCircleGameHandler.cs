using Microsoft.AspNetCore.DataProtection;
using Telegram.Bot.Framework;

namespace Telegram.Bot.Sample.Bots.GreeterBot
{
    public class CrazyCircleGameHandler : GameUpdateHandlerBase
    {
        public CrazyCircleGameHandler(IDataProtectionProvider protectionProvider)
            : base(protectionProvider,
                  Constants.GameBaseUrl, Constants.GameShortName)
        {

        }

        private static class Constants
        {
            public const string GameBaseUrl = "https://pouladpld.github.io/tgGame/index.html";           

            public const string GameShortName = "crazycircle";
        }
    }
}
