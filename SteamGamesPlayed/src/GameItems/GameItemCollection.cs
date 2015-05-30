using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Diagnostics;
using System.Threading;
using SteamGamesPlayed.src.Utils;

namespace SteamGamesPlayed
{
    public static class GameItemCollection
    {
        private static List<GameItem> games = new List<GameItem>();

        public static List<GameItem> Games
        {
            get { return games; }
        }

        public static void AddGame(GameItem game)
        {
            if (game != null)
                games.Add(game);
        }

        public static void DeleteAll()
        {
            games = new List<GameItem>();
        }

        #region OrderBy

        public static List<GameItem> OrderByRelevance()
        {
            return games.OrderBy(x => x.Id).ToList();
        }

        public static List<GameItem> OrderByAlphabetical()
        {
            return games.OrderBy(x => x.Name).ToList();
        }

        public static List<List<GameItem>> OrderByStatusCompleted()
        {
            List<List<GameItem>> listGamesByStatus = new List<List<GameItem>>();
            List<GameItem> listGameCompleted = new List<GameItem>();
            List<GameItem> listGameNotCompleted = new List<GameItem>();

            foreach (GameItem gi in games)
            {
                if (gi.Option == GameItem.Status.Finished)
                    listGameCompleted.Add(gi);
                else
                    listGameNotCompleted.Add(gi);
            }

            listGamesByStatus.Add(listGameCompleted);
            listGamesByStatus.Add(listGameNotCompleted);
            return listGamesByStatus;
        }

        public static List<GameItem> OrderByPlayTime()
        {
            return games.OrderBy(x => x.PlayTime, new NumericComparator()).ToList();
        }

        public static string GetTotalPlayTime()
        {
            int totalPlayTime = 0;

            foreach (GameItem gi in games) {
                if (Utils.IsNumeric(gi.PlayTime))
                    totalPlayTime += Convert.ToInt32(gi.PlayTime);
            }

            return Utils.FormatPlayTime(totalPlayTime);
        }

        public static void UpdateAGame(string strId)
        {
            games.Where(s => s.Id == strId).ToList().ForEach(s => s.Option = (s.Option == GameItem.Status.Finished) ? GameItem.Status.NotFinished : GameItem.Status.Finished);
        }

        #endregion
    }
}