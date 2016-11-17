namespace System
{
    public static class DateTimeOffsetExtensions
    {
        public static string GetInvertedTicks(this DateTimeOffset value)
        {
            return string.Format("{0:D19}", DateTimeOffset.MaxValue.Ticks - value.UtcTicks);
        }
    }
}
