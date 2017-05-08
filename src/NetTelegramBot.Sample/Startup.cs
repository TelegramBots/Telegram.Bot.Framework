using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetTelegramBot.Framework;
using NetTelegramBot.Sample.Bots.EchoBot;
using NetTelegramBot.Sample.Bots.GreeterBot;
using RecurrentTasks;

namespace NetTelegramBot.Sample
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables("SampleEchoBot_")
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var echoBotOptions = new BotOptions<EchoBot>();
            Configuration.GetSection("EchoBot").Bind(echoBotOptions);

            services.AddTelegramBot(echoBotOptions)
                .AddUpdateHandler<TextMessageEchoer>()
                .Configure();

            services.AddTelegramBot<GreeterBot>(Configuration.GetSection("GreeterBot"))
                .AddUpdateHandler<StartCommand>()
                .AddUpdateHandler<PhotoForwarder>()
                .AddUpdateHandler<HiCommand>()
                .AddUpdateHandler<TextMessageEchoer>()
                .Configure();

            services.AddTask<SampleTask>();
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

            app.StartTask<SampleTask>(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));
        }
    }
}
