using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Threading;

namespace SteamGamesPlayed
{
    public partial class MainPage : System.Web.UI.Page
    {
        private OnlineXMLOperation onlineXml;
        private static LocalXMLOperation localXml;

        private static HttpContext context;
        private static string currentGameLabel = null;
        private static Boolean requestFinished;

        public delegate void ProgressUpdateDelegate(string progress);
        private ProgressUpdateDelegate mProgressDelegate;

        public MainPage()
        {
            context = HttpContext.Current;
            // Create the delegate for the progress update.
            this.mProgressDelegate = new ProgressUpdateDelegate(this.SetCurrentGameLabel);
        }

        /// <summary>
        /// This is the function you would be updating the text.
        /// </summary>        
        private void SetCurrentGameLabel(string currentGame)
        {
            currentGameLabel = currentGame;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            // Buttons links (must be on each load)
            BtnOrderByRelevance.Click += new EventHandler(this.OrderByRelevance);
            BtnOrderByAlpha.Click += new EventHandler(this.OrderByAlphabetical);
            BtnOrderByCompleted.Click += new EventHandler(this.OrderByStatusCompleted);
            BtnOrderByTimePlayed.Click += new EventHandler(this.OrderByTimePlayed);
            BtnRetrieveScores.Click += new EventHandler(this.RetrieveScores);

            if (!Page.IsPostBack)
            {
                onlineXml = new OnlineXMLOperation(Constants.SITE_URL);
                localXml = new LocalXMLOperation(HttpContext.Current.Server.MapPath(Constants.LOCAL_XML_FILE));

                // Delete the items for make sure it doesn't already exist
                GameItemCollection.DeleteAll();

                // Retrieve games and add it to a collection
                onlineXml.retrieveDataFromXML();

                // Save the changes in a XML file.
                localXml.SaveAllDataToXml();

                //localXml.LoadDataFromXml();
                AddHeader(Constants.RELEVANCE, GameItemCollection.Games);
            }
        }

        private void ConstructDivs(List<GameItem> listGames)
        {
            int index = 0;

            HtmlGenericControl divBoth = new HtmlGenericControl("div");
            divBoth.Attributes["class"] = "separationHeaderBottom";

            HtmlGenericControl divL = new HtmlGenericControl("divLeft");
            divL.Attributes["class"] = "two_column float_l";

            HtmlGenericControl divR = new HtmlGenericControl("divRight");
            divR.Attributes["class"] = "two_column float_r";

            foreach (GameItem gi in listGames)
            {
                HtmlGenericControl aDiv = new HtmlGenericControl("div");
                aDiv.Attributes["class"] = "game";

                if (gi.IsCompleted())
                {
                    aDiv.Controls.Add(new LiteralControl("<img id=\"green_" + gi.Id + "\" class=\"greenCheck\" style=\"opacity: 1.0;\" src=\"/Styles/Images/greenCheck.png\" alt=\"" + Constants.STATUS_COMPLETE + "\" />"));
                    aDiv.Controls.Add(new LiteralControl("<img id=\"red_" + gi.Id + "\" class=\"redX\" src=\"/Styles/Images/redCancel.png\" alt=\"" + Constants.STATUS_INCOMPLETE + "\" />"));
                }
                else
                {
                    aDiv.Controls.Add(new LiteralControl("<img id=\"green_" + gi.Id + "\" class=\"greenCheck\" src=\"/Styles/Images/greenCheck.png\" alt=\"" + Constants.STATUS_COMPLETE + "\" />"));
                    aDiv.Controls.Add(new LiteralControl("<img id=\"red_" + gi.Id + "\" class=\"redX\" style=\"opacity: 1.0;\" src=\"/Styles/Images/redCancel.png\" alt=\"" + Constants.STATUS_INCOMPLETE + "\" />"));
                }

                aDiv.Controls.Add(new LiteralControl("<label class=\"metaScore\" style=\"color: " + gi.Color + ";\">" + gi.Score + "</label>"));
                aDiv.Controls.Add(new LiteralControl("<label class=\"playTime\">" + gi.PlayTime + "</label>"));
                aDiv.Controls.Add(new LiteralControl("<img class=\"gameImage\" src=\"" + gi.Image + "\" alt=\"" + gi.Name + "\"/>"));
                aDiv.Controls.Add(new LiteralControl("<h3 class=\"gameTitle\">" + gi.Name + "</h3>"));

                if (index % 2 == 0)
                    divL.Controls.Add(aDiv);
                else
                    divR.Controls.Add(aDiv);

                index++;
            }

            divBoth.Controls.Add(divL);
            divBoth.Controls.Add(divR);
            PageColumns.Controls.Add(divBoth);
        }

        private void RemoveControls()
        {
            PageColumns.Controls.Clear();
        }

        private void AddHeader(string headerName, List<GameItem> lgi)
        {
            PageColumns.Controls.Add(new LiteralControl(string.Format("<h2 class=\"separationHeader\">{0} - {1} games</h2>", headerName, lgi.Count)));
            PageColumns.Controls.Add(new LiteralControl("<hr class=\"separation\">"));
            ConstructDivs(lgi);
        }

        #region Buttons

        protected void OrderByRelevance(Object sender, EventArgs e)
        {
            RemoveControls();
            List<GameItem> listGames = GameItemCollection.OrderByRelevance();
            AddHeader(Constants.RELEVANCE, listGames);
        }

        protected void OrderByAlphabetical(Object sender, EventArgs e)
        {
            RemoveControls();
            List<GameItem> listGames = GameItemCollection.OrderByAlphabetical();
            AddHeader(Constants.ALPHABETICAL, listGames);
        }

        protected void OrderByStatusCompleted(Object sender, EventArgs e)
        {
            RemoveControls();
            List<List<GameItem>> listGamesByStatus = GameItemCollection.OrderByStatusCompleted();

            AddHeader(Constants.COMPLETED_LIST, listGamesByStatus[0]);
            AddHeader(Constants.NOT_COMPLETED_LIST, listGamesByStatus[1]);
        }

        protected void OrderByTimePlayed(Object sender, EventArgs e)
        {
            RemoveControls();
            List<GameItem> listGames = GameItemCollection.OrderByPlayTime();
            AddHeader(Constants.ORDER_PT, listGames);
        }

        private void UpdateProgress()
        {
            HttpContext.Current = context;
            UpdateScore us = new UpdateScore(SetCurrentGameLabel);
            us.RetrieveScores();

            requestFinished = true;
        }

        protected void RetrieveScores(Object sender, EventArgs e)
        {
            requestFinished = false;
            ProgressLabel.Visible = true;
            TimerScoreUpdate.Enabled = true;

            // lambda expression
            Thread t = new Thread(() => UpdateProgress());
            t.Start();
        }

        protected void TimerScoreUpdate_Tick(object sender, EventArgs e)
        {
            ProgressLabel.Text = String.Format("Updating: {0}", currentGameLabel);
            if (requestFinished)
            {
                TimerScoreUpdate.Enabled = false;
                ProgressLabel.Text = "Done !";
            }
        }

        #endregion

        #region WebMethods

        [WebMethod]
        public static string SaveStatus(string strId)
        {
            strId = strId.Substring(strId.IndexOf('_') + 1);

            localXml.SaveStatusToXml(strId);

            // Update the status of the specified game
            GameItemCollection.UpdateAGame(strId);

            return strId;
        }

        #endregion
    }
}
