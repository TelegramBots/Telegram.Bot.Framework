//using System.Threading.Tasks;
//using Telegram.Bot.Abstractions;

//namespace Quickstart.Net45.Handlers.Commands
//{
//    class StartCommand : CommandBase
//    {
//        public override async Task HandleAsync(IBot bot, IUpdateContext context, UpdateDelegate next, string[] args)
//        {
//            await bot.Client.SendTextMessageAsync(context.Update.Message.Chat, "Hello, World!");
//            await next(context);
//        }
//    }
//}
