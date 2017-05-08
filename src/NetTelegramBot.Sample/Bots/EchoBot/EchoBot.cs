using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NetTelegramBot.Framework;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Sample.Bots.EchoBot
{
    public class EchoBot : BotBase<EchoBot>
    {
        public EchoBot(IOptions<BotOptions<EchoBot>> botOptions)
            : base(botOptions)
        {

        }

        public override Task HandleUnknownMessageAsync(Update update)
        {
            throw new NotImplementedException();
        }
    }
}
