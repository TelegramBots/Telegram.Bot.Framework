NetTelegramBot Framework
========================

Simple framework for building Telegram bots.

Key features:

1. Multiple bots inside one app
2. All chat logs and settings are saved in Azure Storage Tables
3. Chat logs are saved in different tables by year - just drop old tables when you are out of space
4. Flexible command handling: `/command arg1 arg2` and `/command_arg1_arg2`

Based on [NetTelegramBotApi](https://github.com/justdmitry/NetTelegramBotApi) library.
