using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quickstart.AspNetCore.Handlers;
using Quickstart.AspNetCore.Options;
using Quickstart.AspNetCore.Services;
using System;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace Quickstart.AspNetCore
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<EchoBot>()
                .Configure<BotOptions<EchoBot>>(Configuration.GetSection("EchoBot"))
                .Configure<CustomBotOptions<EchoBot>>(Configuration.GetSection("EchoBot"))
                .AddScoped<TextEchoer>()
                .AddScoped<PingCommand>()
                .AddScoped<StartCommand>()
                .AddScoped<WebhookLogger>()
                .AddScoped<StickerHandler>()
                .AddScoped<WeatherReporter>()
                .AddScoped<ExceptionHandler>()
                .AddScoped<UpdateMembersList>()
                .AddScoped<CallbackQueryHandler>()
            ;
            services.AddScoped<IWeatherService, WeatherService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // get bot updates from Telegram via long-polling approach during development
                // this will disable Telegram webhooks
                app.UseTelegramBotLongPolling<EchoBot>(ConfigureBot(), startAfter: TimeSpan.FromSeconds(2));
            }
            else
            {
                // use Telegram bot webhook middleware in higher environments
                app.UseTelegramBotWebhook<EchoBot>(ConfigureBot());
                // and make sure webhook is enabled
                app.EnsureWebhookSet<EchoBot>();
            }

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }

        private IBotBuilder ConfigureBot()
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
                    .MapWhen<StickerHandler>(When.StickerMessage)
                    .MapWhen<WeatherReporter>(When.LocationMessage)
                )

                .MapWhen<CallbackQueryHandler>(When.CallbackQuery)

                // .Use<UnhandledUpdateReporter>()
                ;
        }
    }
}
