using Microsoft.AspNetCore.DataProtection;
using Telegram.Bot.Framework;

namespace SampleGames.Bots.CrazyCircle
{
    public class CrazyCircleGameHandler : GameHandlerBase
    {
        public CrazyCircleGameHandler(IDataProtectionProvider protectionProvider)
            : base(protectionProvider, Constants.GameShortName)
        {

        }

        private static class Constants
        {
            public const string GameShortName = "crazycircle";
        }
    }
}
