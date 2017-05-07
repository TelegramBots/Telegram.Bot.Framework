//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using NetTelegramBot.Framework;
//using NetTelegramBot.Framework.Extensions;

//namespace Microsoft.Extensions.DependencyInjection
//{
//    public static class ServiceCollectionExtensions
//    {
//        public static IServiceCollection Services;

//        public static ITelegramBotFrameworkMessageParserInjector<TBot> AddTelegramBotFramework<TBot>
//            (this IServiceCollection services, TBot bot, IBotOptions<TBot> botOptions)
//            where TBot : IBot
//        {
//            Services = services;
//            Services.AddTransient<IBotOptions<TBot>>(_ => botOptions);
//            //Services.AddScoped<TBot>()
//            return new TelegramBotFrameworkMessageParserInjector<TBot>();
//        }

//        public interface ITelegramBotFrameworkMessageParserInjector<TBot>
//            where TBot : IBot
//        {
//            IB AddMessageParser(IMessageParser<TBot> messageParser);

//            //ITelegramBotFrameworkBuilder<TBot> AddMessageHandlers(Func<IServiceProvider, IEnumerable<IMessageHandler<TBot>>> factory);
//        }

//        public class TelegramBotFrameworkMessageParserInjector<TBot> 
//            //: ITelegramBotFrameworkMessageParserInjector<TBot>
//            where TBot : IBot
//        {
//            public IB AddMessageParser(IMessageParser<TBot> messageParser)
//            {
//                Services.AddTransient<IMessageParser<TBot>>(_ => messageParser);
//                Services.AddScoped<>();
//            }
//        }
//        public interface IB
//        { }
//    }
//}
