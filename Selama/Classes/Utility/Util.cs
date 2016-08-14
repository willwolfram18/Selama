using MarkdownDeep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Selama.Classes.Utility
{
    public static class Util
    {
        public const string WOW_CHARACTER_THUMBNAIL_URL = "https://render-api-us.worldofwarcraft.com/static-render/us/";

        public static List<TResult> ConvertLists<TIn, TResult>(IEnumerable<TIn> src, Func<TIn, TResult> ctor)
        {
            List<TResult> dest = new List<TResult>();

            foreach (TIn item in src)
            {
                dest.Add(ctor(item));
            }

            return dest;
        }

        public static string RelativeDate(DateTime date)
        {
            DateTime now = DateTime.Now.Date;
            TimeSpan timeDiff = new TimeSpan(now.Date.Ticks - date.Date.Ticks);
            double delta = Math.Abs(timeDiff.TotalSeconds);

            const int SEC_IN_HR = 60 * 60; // 60 sec per min * 60 min per hr
            const int SEC_IN_DAY = SEC_IN_HR * 24;
            const int SEC_IN_WEEK = SEC_IN_DAY * 7;
            const int SEC_IN_MONTH = SEC_IN_DAY * 30;
            const int SEC_IN_YEAR = SEC_IN_DAY * 365;

            if (delta == 0)
            {
                return string.Format("Today at {0:t}", date);
            }
            else if (delta == SEC_IN_DAY)
            {
                return string.Format("Yesterday at {0:t}", date);
            }
            else if (delta < SEC_IN_WEEK)
            {
                return string.Format("{0} days ago", delta / SEC_IN_DAY);
            }
            else if (delta < SEC_IN_MONTH)
            {
                string weeks = "weeks";
                int weekDiff = (int)delta / SEC_IN_WEEK;
                if (weekDiff == 1)
                {
                    weeks = "week";
                }
                return string.Format("{0} {1} ago", weekDiff, weeks);
            }
            else if (delta < SEC_IN_YEAR)
            {
                string months = "months";
                int monthDiff = (int)delta / SEC_IN_MONTH;
                if (monthDiff == 1)
                {
                    months = "month";
                }
                return string.Format("{0} {1} ago", monthDiff, months);
            }
            else
            {
                string years = "years";
                int yearDiff = (int)delta / SEC_IN_YEAR;
                if (yearDiff == 1)
                {
                    years = "year";
                }
                return string.Format("{0} {1} ago", yearDiff, years);
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