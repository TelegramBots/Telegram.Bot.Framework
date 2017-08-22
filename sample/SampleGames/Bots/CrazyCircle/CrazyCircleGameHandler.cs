using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Framework;

namespace SampleGames.Bots.CrazyCircle
{
    public class CrazyCircleGameHandler : GameHandlerBase
    {
        public CrazyCircleGameHandler(IDataProtectionProvider protectionProvider, ILogger<CrazyCircleBot> logger)
            : base(protectionProvider, Constants.GameShortName, logger)
        {

        }

        private static class Constants
        {
            public const string GameShortName = "crazycircle";
        }
    }
}
