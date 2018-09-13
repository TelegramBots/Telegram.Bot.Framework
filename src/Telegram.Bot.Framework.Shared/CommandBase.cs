//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using Telegram.Bot.Framework.Abstractions;
//using Telegram.Bot.Types.Enums;

//namespace Telegram.Bot.Abstractions
//{
//    /// <summary>
//    /// A Telegram bot command such as /start
//    /// </summary>
//    public abstract class CommandBase : IBotCommand
//    {
//        public abstract Task HandleAsync(IUpdateContext context, UpdateDelegate next, string[] args);

//        public Task HandleAsync(IUpdateContext context, UpdateDelegate next)
//        {
//            return HandleAsync(context, next, new string[0]);
//        }

//        public static bool CanHandle(IUpdateContext context, string commandName)
//        {
//            {
//                bool isTextMessage = context.Update.Message?.Text != null;
//                if (!isTextMessage)
//                    return false;
//            }

//            {
//                bool isCommand = context.Update.Message.Entities.FirstOrDefault()?.Type == MessageEntityType.BotCommand;
//                if (!isCommand)
//                    return false;
//            }

//            return Regex.IsMatch(
//                context.Update.Message.Text,
//                $@"^/{commandName}(?:@{context.Bot.User.Username})?(?:\s+|$)",
//                RegexOptions.IgnoreCase
//            );
//        }
//    }
//}