using Quickstart.Net45.Handlers;
using Quickstart.Net45.Services;
using Quickstart.Net45.Services.SimpleInjector;
using SimpleInjector;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace Quickstart.Net45
{
    class Program
    {
        public static void Main(string[] args)
        {
            string token = Environment.GetEnvironmentVariable("BOT_API_TOKEN") ?? "YOUR_API_TOKEN_HERE";

            var container = new Container();
            container.Register(() => new EchoBot(token));
            container.Register<ExceptionHandler>();
            container.Register<IWeatherService, WeatherService>();
            container.Verify();

            var mgr = new UpdatePollingManager<EchoBot>(ConfigureBot(), new BotServiceProvider(container));

            var tokenSrc = new CancellationTokenSource();
            Task.Run(() =>
            {
                Console.ReadLine();
                tokenSrc.Cancel();
            });

            mgr.RunAsync(cancellationToken: tokenSrc.Token).GetAwaiter().GetResult();
        }

        static IBotBuilder ConfigureBot()
        {
            return new BotBuilder()
                    .Use<ExceptionHandler>()

                    // .Use<CustomUpdateLogger>()
                    .UseWhen<WebhookLogger>(When.Webhook)

                    .UseWhen<UpdateMembersList>(When.MembersChanged)

                    .MapWhen(When.NewMessage, msgBranch => msgBranch
                        .MapWhen(When.NewTextMessage, txtBranch => txtBranch
                                .Use<TextEchoer>()
                                .MapWhen(When.NewCommand, cmdBranch => cmdBranch
                                    .UseCommand<PingCommand>("ping")
                                    .UseCommand<StartCommand>("start")
                                )
                        //.Use<NLP>()
                        )
                        .MapWhen(When.StickerMessage, branch => branch.Use<StickerHandler>())
                        .MapWhen(When.LocationMessage, branch => branch.Use<WeatherReporter>())
                    )

                    .MapWhen<CallbackQueryHandler>(When.CallbackQuery)

                // .Use<UnhandledUpdateReporter>()
                ;
        }
    }
}