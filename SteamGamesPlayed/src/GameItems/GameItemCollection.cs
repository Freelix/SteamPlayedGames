using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Diagnostics;
using System.Threading;

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
            List<int> listTime = new List<int>();

            foreach (GameItem gi in games)
            {
                listTime.Add(Convert.ToInt32(gi.PlayTime.Split('h')[0] + gi.PlayTime.Split('h')[1]));
            }

            return games.OrderByDescending(x => x.PlayTime).ToList();
        }

        public static void UpdateAGame(string strId)
        {
            games.Where(s => s.Id == strId).ToList().ForEach(s => s.Option = (s.Option == GameItem.Status.Finished) ? GameItem.Status.NotFinished : GameItem.Status.Finished);
        }

        #endregion
    }
}