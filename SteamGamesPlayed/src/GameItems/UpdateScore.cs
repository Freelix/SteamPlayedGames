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
    public class UpdateScore
    {
        private List<GameItem> games = null;
        private HttpContext context;

        private readonly MainPage.ProgressUpdateDelegate reportProgressToUI;

        public UpdateScore(MainPage.ProgressUpdateDelegate reportProgressToUI)
        {
            games = GameItemCollection.Games;
            this.reportProgressToUI = reportProgressToUI;
            context = HttpContext.Current;
        }

        public void RetrieveScores()
        {
            HttpContext.Current = context;

            List<GameItem> lgi = new List<GameItem>();
            LocalXMLOperation localXml = new LocalXMLOperation();
            localXml.XmlDoc.Load(HttpContext.Current.Server.MapPath(Constants.LOCAL_XML_FILE));
            Metacritic mc;

            Debug.WriteLine("Start...........");

            foreach (GameItem gi in games)
            {
                mc = new Metacritic();
                gi.Meta = mc.GetDataFromMetacritic(gi.Name);

                // If not null, save it to xml
                if (gi.Meta != null && gi.Meta.Score != null)
                    localXml.SaveScoreToXml(gi.Id, gi.Meta.Score);

                Debug.WriteLine("A game has been modified: " + gi.Name + " with " + gi.Score);
                reportProgressToUI(gi.Name);

                lgi.Add(gi);
            }

            Debug.WriteLine("Finished...........");
        }
    }
}