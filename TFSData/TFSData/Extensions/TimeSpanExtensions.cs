using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSData.Extensions
{
    public static class TimeSpanExtensions
    {
        public static int Weeks(this TimeSpan span)
        {
            return span.Duration().Days / 7;
        }

        public static int Days(this TimeSpan span)
        {
            if(span.Duration().Days >= 7)
            {
                return span.Duration().Days - span.Weeks() * 7;
            }

            return span.Duration().Days;
        }

        public static string ToReadableAgeString(this TimeSpan span)
        {
            return string.Format("{0:0}", span.Days / 365.25);
        }

        public static string ToReadableString(this TimeSpan span)
        {
            string formatted = string.Format("{0}{1}{2}{3}{4}",
            span.Weeks() > 0 ? string.Format("{0:0} Week{1}, ", span.Weeks(), span.Weeks() == 1 ? string.Empty : "s") : string.Empty,
            span.Days() > 0 ? string.Format("{0:0} Day{1}, ", span.Days(), span.Days() == 1 ? string.Empty : "s") : string.Empty,
            span.Duration().Hours > 0 ? string.Format("{0:0} Hour{1}, ", span.Hours, span.Hours == 1 ? string.Empty : "s") : string.Empty,
            span.Duration().Minutes > 0 ? string.Format("{0:0} Minute{1}, ", span.Minutes, span.Minutes == 1 ? string.Empty : "s") : string.Empty,
            span.Duration().Seconds > 0 ? string.Format("{0:0} Second{1}", span.Seconds, span.Seconds == 1 ? string.Empty : "s") : string.Empty);

            if(formatted.EndsWith(", ", StringComparison.OrdinalIgnoreCase))
            {
                formatted = formatted.Substring(0, formatted.Length - 2);
            }

            if(string.IsNullOrEmpty(formatted))
            {
                formatted = "0 seconds";
            }

            return formatted;
        }
    }
}
