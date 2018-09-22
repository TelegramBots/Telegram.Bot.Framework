using Telegram.Bot.Types;

namespace Telegram.Bot.Framework
{
    public static class UpdateExtensions
    {
        public static bool IsOfType(this Update update, string type)
        {
            if (type == "message")
                return update.Message != null;
            if (type == "edited_message")
                return update.EditedMessage != null;
            if (type == "channel_post")
                return update.ChannelPost != null;
            if (type == "edited_channel_post")
                return update.EditedChannelPost != null;
            if (type == "inline_query")
                return update.InlineQuery != null;
            if (type == "chosen_inline_result")
                return update.ChosenInlineResult != null;
            if (type == "callback_query")
                return update.CallbackQuery != null;
            if (type == "shipping_query")
                return update.ShippingQuery != null;
            if (type == "pre_checkout_query")
                return update.PreCheckoutQuery != null;

            return false;
        }
    }
}
