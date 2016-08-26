using MarkdownDeep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Selama.Common.Utility
{
    public static class Util
    {
        public const string WOW_CHARACTER_THUMBNAIL_URL = "https://render-api-us.worldofwarcraft.com/static-render/us/{0}-avatar.jpg";
        public const string WOW_CHARCTER_MODEL_URL = "https://render-api-us.worldofwarcraft.com/static-render/us/{0}-profilemain.jpg";

        const int SEC_IN_MIN = 60;
        const int SEC_IN_HR = SEC_IN_MIN * 60; // 60 sec per min * 60 min per hr
        const int SEC_IN_DAY = SEC_IN_HR * 24;
        const int SEC_IN_WEEK = SEC_IN_DAY * 7;
        const int SEC_IN_MONTH = SEC_IN_DAY * 30;
        const int SEC_IN_YEAR = SEC_IN_DAY * 365;

        public static List<TResult> ConvertLists<TIn, TResult>(IEnumerable<TIn> src, Func<TIn, TResult> ctor)
        {
            List<TResult> dest = new List<TResult>();

            foreach (TIn item in src)
            {
                dest.Add(ctor(item));
            }

            return dest;
        }

        public static string RelativeDate(DateTime dateToFormat)
        {
            DateTime now = DateTime.Now;
            TimeSpan timeDiff = new TimeSpan(now.Ticks - dateToFormat.Ticks);
            double timeDeltaInSeconds = Math.Abs(timeDiff.TotalSeconds);

            if (timeDeltaInSeconds < SEC_IN_DAY)
            {
                return FormatRelativeDateForToday(timeDeltaInSeconds);
            }

            // Recalc time diff using Date without hh:mm:ss
            timeDiff = new TimeSpan(now.Date.Ticks - dateToFormat.Date.Ticks);
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

        public static Markdown Markdown = new Markdown
        {
            SafeMode = true,
            ExtraMode = true,
            MarkdownInHtml = true,
        };
    }
}