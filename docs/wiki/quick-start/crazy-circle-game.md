# Crazy Circle Game

Crazy Circle 🔴 is a sample game to demo this framework's features.

You can 🎮 [play it on Telegram](https://telegram.me/CrazyCircleBot?game=crazycircle) 🎮.

Telegram games are just a HTML5 page loaded in browser inside Telegram client
app or in user's browser. Luckily, you can just go ahead and **use Crazy Circle sample**
to begin with.

This guide assumes that:

- You already completed [Echo Bot](./echo-bot.md) quick start
- You are able to deploy the project. See [deployment guides](../README.md#deployment)
- Your bot's user name is `CrazyCircleBot` (Replace it with your own bot's user name)

## New Telegarm Game

Have a chat with **[BotFather](http://t.me/botfather)** to create a new game for your bot and
**set its short_name to `crazycircle`**.

## Project Setup

Create a new ASP.NET Core app and add the following NuGet packages to it:

- `Telegram.Bot.Framework`
- `Microsoft.AspNetCore.StaticFiles`
- `Microsoft.AspNetCore.DataProtection`

Copy files from [`sample/SampleGames/wwwroot/bots`](../../../sample/SampleGames/wwwroot/bots/)
into this project's `wwwroot/bots/` directory.

Rename the bot's direcotry name according to this format: `/wwwroot/bots/{your-bot-user-name}/`.

## Code

### Bot

Create your bot class:

```c#
public class CrazyCircleBot : BotBase<CrazyCircleBot> {
    public CrazyCircleBot(IOptions<BotOptions<CrazyCircleBot>> botOptions)
        : base(botOptions) { }
    public override Task HandleUnknownMessage(Update update) => Task.CompletedTask;
    public override Task HandleFaultedUpdate(Update update, Exception e) => Task.CompletedTask;
}
```

### Update Handlers

Create a `/start` command that replies with the CrazyCircle game:

```c#
public class StartCommandArgs : ICommandArgs {
    public string RawInput { get; set; }
    public string ArgsInput { get; set; }
}

public class StartCommand : CommandBase<StartCommandArgs> {
    public StartCommand() : base(name: "start") {}

    public override async Task<UpdateHandlingResult> HandleCommand(Update update, StartCommandArgs args) {
        await Bot.Client.SendGameAsync(update.Message.Chat.Id, gameShortName: "crazycircle");
        return UpdateHandlingResult.Handled;
    }
}
```

And add the game handler:

```c#
public class CrazyCircleGameHandler : GameHandlerBase {
    public CrazyCircleGameHandler(IDataProtectionProvider protectionProvider)
        : base(protectionProvider, shortName: "crazycircle") {}
}
```

### Startup

In `Startup` class, add the following code:

```c#
public void ConfigureServices(IServiceCollection services) {
    services.AddDataProtection();
    services.AddTelegramBot<CrazyCircleBot>(_configuration.GetSection("CrazyCircleBot"))
        .AddUpdateHandler<StartCommand>()
        .AddUpdateHandler<CrazyCircleGameHandler>()
        .Configure();
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
    app.UseStaticFiles();
    app.UseTelegramGame<CrazyCircleBot>();
    app.UseTelegramBotWebhook<CrazyCircleBot>();
}
```

## Configurations

Add the following configurations to your `appsettings.json` or use any other similar method.

```json
{
  "CrazyCircleBot": {
    "ApiToken": "",
    "BotUserName": "CrazyCircleBot",
    "PathToCertificate": "",
    "BaseUrl": "https://example.com/bots/",
    "GameOptions": [ { "ShortName": "crazycircle" } ]
  }
}
```

## Deploy

Deploy your application and enjoy playing! 😃