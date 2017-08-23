# Telegram Bot Framework for .NET Core

 [![NuGet](https://img.shields.io/nuget/v/Telegram.Bot.Framework.svg?style=flat-square&label=Telegram.Bot.Framework&maxAge=3600)](https://www.nuget.org/packages/Telegram.Bot.Framework)
 [![Build Status](https://img.shields.io/travis/pouladpld/Telegram.Bot.Framework.svg?style=flat-square&maxAge=3600)](https://travis-ci.org/pouladpld/Telegram.Bot.Framework)
 [![License](https://img.shields.io/github/license/pouladpld/Telegram.Bot.Framework.svg?style=flat-square&maxAge=2592000)](https://raw.githubusercontent.com/pouladpld/Telegram.Bot.Framework/master/LICENSE.txt)

<img src="./docs/icon.png" alt="Telegram Bot Framework Logo" width=200 height=200 />

Simple framework for building Telegram bots 🤖. Ideal for running multiple chat bots inside a single ASP.NET Core app.

See some **sample bots** in action:

- Echo bot:   [`@Sample_Echoer_Bot`](https://t.me/sample_echoer_bot)
- Games bot:  [`@CrazyCircleBot`](https://t.me/CrazyCircleBot)

## Getting Started

This project targets .NET Standard 1.6 so make sure you have Visual Studio 2017 or [.NET Core](https://www.microsoft.com/net/download/core#/current) (v1.1 or above) installed.

Creating a bot with good architecture becomes very simple using this framework. Have a look at the [**Quick Start** wiki](./docs/wiki/quick-start/echo-bot.md) to make your fist _Echo Bot_.

There is much more you can do with your bot. See what's available at [**wikis**](./docs/wiki/README.md).

## Framework Features

- Allows you to have multiple bots running inside one app
- Able to share code(update handlers) between multiple bots
- Easy to use with webhooks(specially with Docker deployments)
- Optimized for making Telegram Games
- Simplifies many repititive tasks in developing bots

## Samples

Don't wanna read wikis? Read C# code of sample projects in [samples directory](./sample/).
