using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteamGamesPlayed.src.Utils
{
    public class NumericComparator : IComparer<string>
    {
        public int Compare(string s1, string s2)
        {
            if (Utils.IsNumeric(s1) && Utils.IsNumeric(s2))
            {
                if (Convert.ToInt32(s1) < Convert.ToInt32(s2)) return 1;
                if (Convert.ToInt32(s1) > Convert.ToInt32(s2)) return -1;
                if (Convert.ToInt32(s1) == Convert.ToInt32(s2)) return 0;
            }

            if (Utils.IsNumeric(s1) && !Utils.IsNumeric(s2))
                return 1;

            if (!Utils.IsNumeric(s1) && Utils.IsNumeric(s2))
                return -1;

            return string.Compare(s1, s2, true);
        }
    }
}