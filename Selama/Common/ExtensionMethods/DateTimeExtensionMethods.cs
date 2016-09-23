using System;

namespace Selama.Common.ExtensionMethods
{
    public static class DateTimeExtensionMethods
    {
        private const int SEC_IN_MIN = 60;
        private const int SEC_IN_HR = SEC_IN_MIN * 60;
        private const int SEC_IN_DAY = SEC_IN_HR * 24;
        private const int SEC_IN_WEEK = SEC_IN_DAY * 7;
        private const int SEC_IN_MONTH = SEC_IN_DAY * 30;
        private const int SEC_IN_YEAR = SEC_IN_DAY * 365;

        public static string ToRelativeDateTimeString(this DateTime dateToFormat)
        {
            DateTime now = DateTime.Now;
            if (dateToFormat.Kind == DateTimeKind.Utc)
            {
                now = DateTime.Now.ToUniversalTime();
            }

            TimeSpan timeDiff = new TimeSpan(now.Ticks - dateToFormat.Ticks);
            bool isFutureTime = timeDiff.TotalSeconds < 0;
            double absoluteTimeDiffInSeconds = Math.Abs(timeDiff.TotalSeconds);

            if (isFutureTime)
            {
                return FormatForFutureTime(dateToFormat, absoluteTimeDiffInSeconds);
            }
            else
            {
                return FormatForPastTime(dateToFormat, absoluteTimeDiffInSeconds);
            }
            
        }

        #region Future times
        private static string FormatForFutureTime(DateTime dateToFormat, double timeDeltaInSeconds)
        {
            DateTime now = DateTime.Now;
            if (dateToFormat.Kind == DateTimeKind.Utc)
            {
                now = DateTime.Now.ToUniversalTime();
            }

            if (timeDeltaInSeconds < SEC_IN_DAY)
            {
                return FormatFutureRelativeDateForToday(timeDeltaInSeconds);
            }

            // Recalc time diff using Date without hh:mm:ss
            TimeSpan timeDiff = new TimeSpan(now.Date.Ticks - dateToFormat.Date.Ticks);
            timeDeltaInSeconds = Math.Abs(timeDiff.TotalSeconds);
            return FormatRelativeDateForTomorrowAndBeyond(timeDeltaInSeconds, dateToFormat);
        }

        private static string FormatFutureRelativeDateForToday(double timeDeltaInSeconds)
        {
            if (timeDeltaInSeconds < SEC_IN_MIN)
            {
                return "In a minute";
            }
            else if (timeDeltaInSeconds < SEC_IN_HR)
            {
                return FormatFutureTimeString("minute", "minutes", (int)timeDeltaInSeconds / SEC_IN_MIN);
            }
            else
            {
                return FormatFutureTimeString("hour", "hours", (int)timeDeltaInSeconds / SEC_IN_HR);
            }
        }

        private static string FormatRelativeDateForTomorrowAndBeyond(double timeDeltaInSeconds, DateTime dateToFormat)
        {
            if (timeDeltaInSeconds == SEC_IN_DAY)
            {
                return string.Format("Tomorrow at {0:t}", dateToFormat);
            }
            else if (timeDeltaInSeconds < SEC_IN_WEEK)
            {
                return string.Format("In {0} days", (int)timeDeltaInSeconds / SEC_IN_DAY);
            }
            else if (timeDeltaInSeconds < SEC_IN_MONTH)
            {
                return FormatFutureTimeString("week", "weeks", (int)timeDeltaInSeconds / SEC_IN_WEEK);
            }
            else if (timeDeltaInSeconds < SEC_IN_YEAR)
            {
                return FormatFutureTimeString("month", "months", (int)timeDeltaInSeconds / SEC_IN_MONTH);
            }
            else
            {
                return FormatFutureTimeString("year", "years", (int)timeDeltaInSeconds / SEC_IN_YEAR);
            }
        }

        private static string FormatFutureTimeString(string singularFormOfTime, string pluralFormOfTime, int count)
        {
            string formatStr = "In {0} {1}";
            if (count == 1)
            {
                return string.Format(formatStr, count, singularFormOfTime);
            }
            return string.Format(formatStr, count, pluralFormOfTime);
        }
        #endregion

        private static string FormatForPastTime(DateTime dateToFormat, double timeDeltaInSeconds)
        {
            DateTime now = DateTime.Now;
            if (dateToFormat.Kind == DateTimeKind.Utc)
            {
                now = DateTime.Now.ToUniversalTime();
            }
            if (timeDeltaInSeconds < SEC_IN_DAY)
            {
                return FormatRelativeDateForToday(timeDeltaInSeconds);
            }

            // Recalc time diff using Date without hh:mm:ss
            TimeSpan timeDiff = new TimeSpan(now.Date.Ticks - dateToFormat.Date.Ticks);
            timeDeltaInSeconds = Math.Abs(timeDiff.TotalSeconds);
            return FormatRelativeDateForYesterdayAndBeyond(timeDeltaInSeconds, dateToFormat);
        }

        private static string FormatRelativeDateForToday(double timeDeltaInSeconds)
        {
            if (timeDeltaInSeconds < SEC_IN_MIN)
            {
                return "Less than a minute ago";
            }
            else if (timeDeltaInSeconds < SEC_IN_HR)
            {
                return FormatTimeAgoString("minute", "minutes", (int)timeDeltaInSeconds / SEC_IN_MIN);
            }
            else
            {
                return FormatTimeAgoString("hour", "hours", (int)timeDeltaInSeconds / SEC_IN_HR);
            }
        }

        private static string FormatTimeAgoString(string singularFormOfTime, string pluralFormOfTime, int count)
        {
            string formatString = "{0} {1} ago";
            if (count == 1)
            {
                return string.Format(formatString, count, singularFormOfTime);
            }
            return string.Format(formatString, count, pluralFormOfTime);
        }

        private static string FormatRelativeDateForYesterdayAndBeyond(double timeDeltaInSeconds, DateTime dateToFormat)
        {
            if (timeDeltaInSeconds == SEC_IN_DAY)
            {
                return string.Format("Yesterday at {0:t}", dateToFormat);
            }
            else if (timeDeltaInSeconds < SEC_IN_WEEK)
            {
                return string.Format("{0} days ago", (int)timeDeltaInSeconds / SEC_IN_DAY);
            }
            else if (timeDeltaInSeconds < SEC_IN_MONTH)
            {
                return FormatTimeAgoString("week", "weeks", (int)timeDeltaInSeconds / SEC_IN_WEEK);
            }
            else if (timeDeltaInSeconds < SEC_IN_YEAR)
            {
                return FormatTimeAgoString("month", "months", (int)timeDeltaInSeconds / SEC_IN_MONTH);
            }
            else
            {
                return FormatTimeAgoString("year", "years", (int)timeDeltaInSeconds / SEC_IN_YEAR);
            }
        }
    }
}