# Telegram Bot Framework for .NET Core

[![Build Status](https://travis-ci.org/pouladpld/NetTelegram.Bot.Framework.svg?branch=master)](https://travis-ci.org/pouladpld/NetTelegram.Bot.Framework)
[![NuGet](https://img.shields.io/nuget/v/NetTelegram.Bot.Framework.svg)](https://www.nuget.org/packages/NetTelegram.Bot.Framework)

<img src="./docs/icon.png" alt="Telegram Bot Framework Logo" width=200 height=200 />

Simple framework for building Telegram bots. Ideal for running multiple chat bots inside a single ASP.NET Core app.

You can see a sample bot using this framework in action here: [https://t.me/NTBF_SampleBot](https://t.me/NTBF_SampleBot)

## Screenshots

--> Add here <--

## Getting Started

### Requirements

- Visual Studio 2017 or [.NET Core 1.1](https://www.microsoft.com/net/download/core#/current).

> Talk to **[BotFather](http://t.me/botfather)** to get a token for your Telegram bot.

### Implementation

Getting your bot to work is very easy with this framework. Following code snippets show that:

#### Bot Type

```c#
class EchoBot : BotBase<EchoBot>
{
    ...
}
```

#### Message Handler

```c#
class TextMessageEchoer : UpdateHandlerBase
{
    public override bool CanHandleUpdate(IBot bot, Update update)
    {
        // Can handle it only if the update is a text message
    }

    public override async Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update)
    {
        // Echo back the text to user
        return UpdateHandlingResult.Handled;
    }
}
```

#### Configure the Bot

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddTelegramBot<EchoBot>(Configuration.GetSection("EchoBot"))
        .AddUpdateHandler<TextMessageEchoer>()
        .Configure();
}
```

## Examples

Have a look at the [Sample app](./src/NetTelegramBot.Sample/) to see some examples.

## Credits

Special thanks to [Dmitry Popov](https://github.com/justdmitry) for building [NetTelegramBotApi](https://github.com/justdmitry/NetTelegramBotApi) library.
