using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LostArkLogger.LarkCustom.Utility
{
    public static class NumberStringFormatter
    {
        public static string FormatLargeInt(UInt64 num)
        {
            string numStr = num.ToString();
            if (num > 1000)
            {
                string numStr2 = $"{numStr[0..3]}.{numStr[3..5]} ";
                if (num >= 1000000000000) return numStr2 + "T";
                else if (num >= 1000000000) return numStr2 + "B";
                else if (num >= 1000000) return numStr2 + "M";
                else return numStr2 + "K";
            }
            return numStr;
        }

        public static string FormatLargeInt(UInt32 num)
        {
            string numStr = num.ToString();
            if (num > 1000)
            {
                string numStr2 = $"{numStr[0..3]}.{numStr[3..6]} ";
                if (num >= 1000000000) return numStr2 + "B";
                else if (num >= 1000000) return numStr2 + "M";
                else return numStr2 + "K";
            }
            return numStr;
        }

        public static string FormatRoundPercent(double num)
        {
            return $"{Math.Round(num, 2)}%";
        }

        public static string FormatPercent(double num)
        {
            return $"{Math.Round(num * 100, 2)}%";
        }

        public static string TimeFormatter(TimeSpan timeSpan)
        {
            string min = timeSpan.Minutes < 10 ? $"0{timeSpan.Minutes}" : timeSpan.Minutes.ToString();
            string seconds = timeSpan.Seconds < 10 ? $"0{timeSpan.Seconds}" : timeSpan.Seconds.ToString();
            string mills;

            if (timeSpan.Milliseconds < 10)
            {
                mills = $"00{timeSpan.Milliseconds}";
            }
            else if (timeSpan.Milliseconds < 100)
            {
                mills = $"0{timeSpan.Milliseconds}";
            }
            else
            {
                mills = timeSpan.Milliseconds.ToString();
            }
            return $"{min}:{seconds}.{mills}";
        }
    }
}
