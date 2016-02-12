namespace Invisual.AppCore.Extensions
{
    public static class IntExtensions
    {
        public static string AddOrdinal(this int number)
        {
            if (!string.IsNullOrWhiteSpace(number.GetOrdinal()))
                return number + number.GetOrdinal();

            return number.ToString();
        }

        public static string GetOrdinal(this int number)
        {
            if (number <= 0)
				return null;

            switch (number % 100)
            {
                case 11:
                case 12:
                case 13:
                    return "th";
            }

            switch (number % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }
    }
}
