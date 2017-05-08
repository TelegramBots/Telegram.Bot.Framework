using System;
using System.Threading.Tasks;
using NetTelegramBot.Framework;
using NetTelegramBot.Framework.Abstractions;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Sample.Bots.EchoBot
{
    public class EchoBot : BotBase<EchoBot>
    {
        public EchoBot(IBotOptions<EchoBot> botOptions, IUpdateParser<EchoBot> updateParser) 
            : base(botOptions, updateParser)
        {
        }

        public override Task HandleUnknownMessageAsync(Update update)
        {
            throw new NotImplementedException();
        }
    }
}
