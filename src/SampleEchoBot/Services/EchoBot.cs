using System.Threading.Tasks;
using NetTelegramBot.Framework;
using NetTelegramBot.Framework.Abstractions;
using NetTelegramBotApi.Types;

namespace SampleEchoBot.Services
{
    public interface IEchoBot : IBot
    {

    }

    public class EchoBot : BotBase<IEchoBot>, IEchoBot
    {
        public EchoBot(IBotOptions<IEchoBot> botOptions, IMessageParser<IEchoBot> messageParser)
            : base(botOptions, messageParser)
        {

        }

        public override Task HandleUnknownMessageAsync(Message message)
        {
            throw new System.NotImplementedException();
        }
    }
}
