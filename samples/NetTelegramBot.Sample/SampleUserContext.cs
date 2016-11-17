namespace NetTelegramBot.Sample
{
    using System;

    public class SampleUserContext
    {
        public DateTimeOffset FirstContact { get; set; }

        public long ChatId { get; set; }

        public bool IsChat { get; set; }

        public bool IsPowerUser { get; set; }
    }
}
