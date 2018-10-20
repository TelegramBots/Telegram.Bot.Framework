//using System.Threading.Tasks;
//using Telegram.Bot;
//using Telegram.Bot.Abstractions;
//using Telegram.Bot.Types;
//
//namespace SampleEchoBot
//{
//    public class TextMessageHandler : IUpdateHandler
//    {
//        private ITelegramBotClient BotClient => _bot.Client;
//        private readonly IBot _bot;
//
//        public TextMessageHandler(IBot bot)
//        {
//            _bot = bot;
//        }
//
//        public bool CanHandle(IUpdateContext context) =>
//            context.Update.Message?.Text != null;
//
//        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
//        {
//            Message msg = context.Update.Message;
//
//            await BotClient.SendTextMessageAsync(
//                msg.Chat, "You said:\n" + msg.Text
//            );
//
//            await next(context);
//        }
//    }
//}