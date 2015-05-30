using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteamGamesPlayed.src.Utils
{
    public class Utils
    {
        public static string FormatPlayTime(string number)
        {
            if (number != "0")
            {
                int time = Convert.ToInt32(number);
                int hour = time / 60;
                int min = time % 60;
                string strMin;

                if (min < 10)
                    strMin = "0" + Convert.ToString(min);
                else
                    strMin = Convert.ToString(min);

                return String.Format("{0}h{1}", hour, strMin);
            }
            else
                return "0h00";
        }

        public static string FormatPlayTime(int number)
        {
            return FormatPlayTime(Convert.ToString(number));
        }

        public static bool IsNumeric(object value)
        {
            try
            {
                int i = Convert.ToInt32(value.ToString());
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}