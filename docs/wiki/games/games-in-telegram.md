# Games in Telegram

Games are simply HTML5(HTML/CSS/JS) pages loaded into a Telegram client's browser. Users also
have option to open the game page in a browser. Check Telegram's post about [Gaming Platform](https://telegram.org/blog/games)
to learn more.

For example, [LumberJack](https://telegram.me/gamebot?game=Lumberjack) is a Telegram game. Go ahead and
play it!

## Creating a Game

This all happens in your chat with [BotFather](http://t.me/botfather). Enable [Inline Mode](https://core.telegram.org/bots/#inline-mode) for your bot and use `/newgame` command to create your game. You can read more about it [here](https://core.telegram.org/bots/games#creating-a-game).

Each game has a _short name_ that is game's unique id whithin a bot.

## Redirecting user to Game

In the chat, when user clicks on _Play LumberJack_, for instance, your bot receives a CallbackQuery update
containing `game_short_name`. In response, bot makes a `answerCallbackQuery` request passing the url to the
game's page. Telegram client opens the browser and the fun starts!

For example, start [LumberJack](https://telegram.me/gamebot?game=Lumberjack) game on your phone and click on
_Open in..._ while in game to open it in a browser. You will see the link to game is something like:

`https://tbot.xyz/lumber/#{some-encoded-value}&tgShareScoreUrl=tgb://share_game_score?hash={some-hash-value}`

## High Scores

Games can set high scores for users in the chat that they got opened from. This means when game is finished,
a request is usually made to backend (bot's ASP.NET Core app) and bot makes a `setGameScore` call to Telegram API.

Similarly, for getting high scores, a request from HTML page(game) could be sent to backend. Then, bot makes a `getGameHighScores` call to Bot API and passes back the high scores to the game.

## Deployment

Your Game's HTML page can be anywhere on the internet. It's worth mentioning as a web developer, you should consider many things in your deployment such as:

- Preferring HTTPS over HTTP
- Using a trusted TLS certificate
- Allowing Cross-Origin requests, if necessary
- Guarding against attacks
- Preventing cheaters
- And many more...

> Sample games in this project are kept simple and do not follow best practices.
