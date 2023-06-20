namespace ItsJackAnton.Utility
{
    using UnityEngine;
    public static class TimeUtil
    {
        #region TimeTs
        public static long NowTs()
        {
            return System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
        public static long GetEndTs(this int seconds)
        {
            return NowTs() + (seconds * 1000);
        }
        public static bool IsElapsed(this long endTs)
        {
            return NowTs() < endTs;
        }
        public static float LeftTsPerc(this long endTs, float duration)
        {
            return Mathf.Clamp01(1 - (((endTs - NowTs()) / 1000) / duration));
        }
        public static long NextMidNightTimeStamp()
        {
            long now = NowTs();
            long prev = now - (now % (86400 * 1000));
            return prev + (86400 * 1000);
        }
        //
        public static int GetSecsLeft(this long endTs)
        {
            return (int)((endTs - NowTs()) / 1000);
        }
        public static int GetMinLeft(this long endTs)
        {
            return GetSecsLeft(endTs) / 60;
        }
        public static int GetHoursLeft(this long endTs)
        {
            return GetSecsLeft(endTs) / 60 / 60;
        }
        public static int GetDaysLeft(this long endTs)
        {
            return GetSecsLeft(endTs) / 60 / 60 / 24;
        }
        public static string GetTimeLeft(this long endTs)
        {
            int daysLeft = GetDaysLeft(endTs);
            int hoursLeft = GetHoursLeft(endTs);
            int minLeft = GetMinLeft(endTs);

            if (daysLeft > 0)
            {
                return $"{daysLeft}d {GetHoursLeft(endTs) - (daysLeft * 24)}h";
            }
            else if (hoursLeft > 0)
            {
                return $"{hoursLeft}h {GetMinLeft(endTs) - (daysLeft * 24 * 60) - (hoursLeft * 60)}m";
            }
            else if (minLeft > 0)
            {
                return $"{minLeft}m {GetSecsLeft(endTs) - (daysLeft * 24 * 60) - (hoursLeft * 60) - (minLeft * 60)}s";
            }
            else
            {
                int secondsLeft = GetSecsLeft(endTs);
                if (secondsLeft < 0) return "0s";
                return $"{secondsLeft - (daysLeft * 24 * 60) - (hoursLeft * 60) - (minLeft * 60)}s";
            }
        }

        public static string GetFullTimeLeft(this long endTs)
        {
            int daysLeft = GetDaysLeft(endTs);
            int hoursLeft = GetHoursLeft(endTs);
            int minLeft = GetMinLeft(endTs);
            int secLeft = GetSecsLeft(endTs);

            int valToSubHrs = (daysLeft * 24);
            int valToSubMins = (hoursLeft * 60);
            int valToSubSecs = (minLeft * 60);

            int hrsToDisplay = hoursLeft - valToSubHrs;
            int minsToDisplay = minLeft - valToSubMins;
            int secsToDisplay = secLeft - valToSubSecs;

            if (secsToDisplay < 0) return "0s";
            else if (daysLeft > 0) return $"{daysLeft}d {hrsToDisplay}h {minsToDisplay}m {secsToDisplay}s";
            else if (hrsToDisplay > 0) return $"{hrsToDisplay}h {minsToDisplay}m {secsToDisplay}s";
            else if (minsToDisplay > 0) return $"{minsToDisplay}m {secsToDisplay}s";
            else return $"{secsToDisplay}s";
        }
        public static string GetTimeLeftDigital(this long endTs)
        {
            static string FixNumToDigital(int value)
            {
                return value < 10 ? $"0{value}" : $"{value}";
            }
            int daysLeft = GetDaysLeft(endTs);
            int hoursLeft = GetHoursLeft(endTs);
            int minLeft = GetMinLeft(endTs);
            int secLeft = GetSecsLeft(endTs);

            int valToSubHrs = (daysLeft * 24);
            int valToSubMins = (hoursLeft * 60);
            int valToSubSecs = (minLeft * 60);

            int hrsToDisplay = hoursLeft - valToSubHrs;
            int minsToDisplay = minLeft - valToSubMins;
            int secsToDisplay = secLeft - valToSubSecs;

            if (secsToDisplay < 0) return "00:00";
            else if (daysLeft > 0) return $"{FixNumToDigital(daysLeft)}:{FixNumToDigital(hrsToDisplay)}:{FixNumToDigital(minsToDisplay)}:{FixNumToDigital(secsToDisplay)}";
            else if (hrsToDisplay > 0) return $"{FixNumToDigital(hrsToDisplay)}:{FixNumToDigital(minsToDisplay)}:{FixNumToDigital(secsToDisplay)}";
            else return $"{FixNumToDigital(minsToDisplay)}:{FixNumToDigital(secsToDisplay)}";
        }
        #endregion

        public static int MinutesToSeconds(this int minutes) { return minutes * 60; }
        public static int HoursToSeconds(this int hours) { return hours * 60 * 60; }
        public static int DaysToSeconds(this int days) { return days * 24 * 60 * 60; }

        public static int AsDays(this int seconds)
        {
            return ((seconds / 60) / 60) / 24;
        }
        public static int AsHours(this int seconds)
        {
            return (seconds / 60) / 60;
        }
        public static int AsMins(this int seconds)
        {
            return seconds / 60;
        }

        public static int AsDigitalHours(this int seconds)
        {
            return seconds.AsHours() % 24;
        }
        public static int AsDigitalMins(this int seconds)
        {
            return seconds.AsMins() % 60;
        }

        public static int AsDigitalSeconds(this int seconds)
        {
            return seconds % 60;
        }

        public static string ToTime(this int seconds)
        {
            int hours = seconds.AsDigitalHours(), min = seconds.AsDigitalMins(), sec = seconds.AsDigitalSeconds();
            return $"{(hours < 10 ? ("0" + hours.ToString()) : hours.ToString())}:{(min < 10 ? ("0" + min.ToString()) : min.ToString())}:{(sec < 10 ? ("0" + sec.ToString()) : sec.ToString())}";
        }
        public static string ToTimeHourMin(this int seconds)
        {
            int hours = seconds.AsDigitalHours(), min = seconds.AsDigitalMins();
            return $"{(hours < 10 ? ("0" + hours.ToString()) : hours.ToString())}:{(min < 10 ? ("0" + min.ToString()) : min.ToString())}";
        }
        public static string ToTimeMinSec(this int seconds)
        {
            int min = seconds.AsDigitalMins(), sec = seconds.AsDigitalSeconds();
            return $"{(min < 10 ? ("0" + min.ToString()) : min.ToString())}:{(sec < 10 ? ("0" + sec.ToString()) : sec.ToString())}";
        }
        public static string ToClassicTime(this int seconds)
        {
            int hours = seconds.AsDigitalHours(), min = seconds.AsDigitalMins(), sec = seconds.AsDigitalSeconds();
            bool isPM = hours >= 12;
            hours = hours % 12;
            if (hours == 0) hours = 12;
            return $"{(hours < 10 ? ("0" + hours.ToString()) : hours.ToString())}:{(min < 10 ? ("0" + min.ToString()) : min.ToString())}:{(sec < 10 ? ("0" + sec.ToString()) : sec.ToString())} {(isPM ? "PM" : "AM")}";
        }
        public static bool TimeToSeconds(this string time, out int result)
        {
            string[] hoursMin = time.Split(':');

            result = 0;
            if (hoursMin.Length != 3) return false;

            string[] secIsPM = hoursMin[2].Split(' ');


            if (int.TryParse(hoursMin[0], out int hours) && int.TryParse(hoursMin[1], out int min) && int.TryParse(secIsPM[0], out int sec))
            {
                if (secIsPM.Length > 1)
                {

                    if ("AM" == secIsPM[1])
                    {
                        if (hours == 12) hours = 0;
                    }
                    else
                    {
                        if (hours < 12) hours += 12;
                    }
                }
                sec += HoursToSeconds(hours);
                sec += MinutesToSeconds(min);
                result = sec;
                return true;
            }
            return false;
        }
        public static int[] ToTimeArr(this int seconds)
        {
            int hours = seconds.AsDigitalHours(), min = seconds.AsDigitalMins(), sec = seconds.AsDigitalSeconds();
            return new int[]
            {
                hours,
                min,
                sec
            };
        }

        public static string IndexToDay(this int index)
        {
            switch (index)
            {
                case 0:
                    return "Sunday";
                case 1:
                    return "Monday";
                case 2:
                    return "Tuesday";
                case 3:
                    return "Wednesday";
                case 4:
                    return "Thursday";
                case 5:
                    return "Friday";
                case 6:
                    return "Saturday";
                default:
                    break;
            }
            return $"Index out of range (0-6) {index}";
        }
        public static int DayToIndex(this string day)
        {
            switch (day)
            {
                case "Sunday":
                    return 0;
                case "Monday":
                    return 1;
                case "Tuesday":
                    return 2;
                case "Wednesday":
                    return 3;
                case "Thursday":
                    return 4;
                case "Friday":
                    return 5;
                case "Saturday":
                    return 6;
                default:
                    break;
            }
            return -1;
        }
        public static bool IsPM(this int seconds) { int _val = seconds % 86400; return (_val < 0 ? 86400 - Mathf.Abs(_val) : _val) > 43199; }
        public static int ModToDay(this int seconds) { int _val = seconds % 86400; return _val < 0 ? 86400 - Mathf.Abs(_val) : _val; }
        public static int ModToMidDay(this int seconds) { int _val = seconds % 43200; return _val < 0 ? 43200 - Mathf.Abs(_val) : _val; }
    }
}