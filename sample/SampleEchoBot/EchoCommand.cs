//using System.Threading.Tasks;
//using Telegram.Bot.Abstractions;
//using Telegram.Bot.Framework;
//using Telegram.Bot.Types;
//
//namespace SampleEchoBot
//{
//    public class EchoCommand : CommandBase<CommandArgs>
//    {
//        public EchoCommand() : base(name: "echo")
//        {
//        }
//
//        public override async Task HandleCommandAsync(Update update, CommandArgs args)
//        {
//            string replyText = string.IsNullOrWhiteSpace(args.ArgsInput) ? "Echo What?" : args.ArgsInput;
//
//            await Bot.Client.SendTextMessageAsync(
//                update.Message.Chat.Id,
//                replyText,
//                replyToMessageId: update.Message.MessageId);
//        }
//    }
//
//    public class CommandArgs : ICommandArgs
//    {
//        public string RawInput { get; set; }
//        public string ArgsInput { get; set; }
//    }
//}