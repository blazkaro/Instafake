namespace Instafake.BFF.Extensions;

public static class DateTimeExtensions
{
    extension(DateTime dateTime)
    {
        public DateTime AsUtc()
        {
            return new DateTime(dateTime.Ticks, DateTimeKind.Utc);
        }
    }
}
