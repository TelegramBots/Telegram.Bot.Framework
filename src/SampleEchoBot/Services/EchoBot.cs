using System.Threading.Tasks;
using NetTelegramBot.Framework;
using NetTelegramBotApi.Types;

namespace SampleEchoBot.Services
{
    public interface IEchoBot : IBot
    {

    }

    public class EchoBot : BotBase<EchoBot>, IEchoBot
    {
        public EchoBot(IBotOptions<EchoBot> botOptions, IMessageParser<EchoBot> messageParser)
            : base(botOptions, messageParser)
        {

        }

        public override Task HandleUnknownMessageAsync(Message message)
        {
            throw new System.NotImplementedException();
        }
    }
}
