using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetTelegramBot.Framework;
using NetTelegramBot.Framework.Abstractions;
using NetTelegramBot.Sample.Bots.EchoBot;
using NetTelegramBot.Sample.Bots.GreeterBot;

namespace NetTelegramBot.Sample
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables("SampleEchoBot_")
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var echoBotOptions = new BotOptions<EchoBot>();
            _configuration.GetSection("EchoBot").Bind(echoBotOptions);

            services.AddTelegramBot(echoBotOptions)
                .AddUpdateHandler<TextMessageEchoer>()
                .Configure();

            var greeterBotOptions = new BotOptions<GreeterBot>();
            _configuration.GetSection("GreeterBot").Bind(greeterBotOptions);

            services.AddTelegramBot(greeterBotOptions)
                .AddUpdateHandler<StartCommand>()
                .AddUpdateHandler<PhotoForwarder>()
                .AddUpdateHandler<HiCommand>()
                .AddUpdateHandler<TextMessageEchoer>()
                .Configure();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });

            Task.Factory.StartNew(async () =>
               {
                   while (true)
                   {
                       using (var scope = app.ApplicationServices.CreateScope())
                       {
                           //var opts = scope.ServiceProvider.GetRequiredService<IBotOptions<IEchoBot>>();
                           //var accessor = scope.ServiceProvider
                           //    .GetRequiredService<IMessageHandlersAccessor<IEchoBot>>();
                           //var parser = scope.ServiceProvider.GetRequiredService<IMessageParser<IEchoBot>>();
                           //var bot = scope.ServiceProvider.GetRequiredService<IEchoBot>();

                           //var mgr = scope.ServiceProvider.GetRequiredService<IBotUpdateManager<IEchoBot>>();
                           var mgr = scope.ServiceProvider.GetRequiredService<IBotUpdateManager<GreeterBot>>();
                           mgr.Run(null);
                       }
                       await Task.Delay(2000);
                   }
               })
            .ContinueWith(t =>
            {
                Debug.WriteLine(t.Exception.InnerExceptions.First().Message);
            }, TaskContinuationOptions.OnlyOnFaulted)
            .ConfigureAwait(false);

        }
    }
}
