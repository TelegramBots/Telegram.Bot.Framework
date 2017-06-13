using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NetTelegram.Bot.Framework;
using NetTelegramBotApi.Types;

namespace NetTelegram.Bot.Sample.Bots.EchoBot
{
    public class EchoerBot : BotBase<EchoerBot>
    {
        public EchoerBot(IOptions<BotOptions<EchoerBot>> botOptions)
            : base(botOptions)
        {

        }

        public override Task HandleUnknownMessage(Update update)
        {
            throw new NotImplementedException();
        }

        public override Task HandleFaultedUpdate(Update update, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
