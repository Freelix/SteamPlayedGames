using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml;

namespace SteamGamesPlayed
{
    public class OnlineXMLOperation
    {
        #region Attributes

        private XDocument xmlDoc;
        private string url;
        private List<string> listNames;
        private List<string> listlogoUrls;
        private List<string> listIds;
        private List<string> listPlayTime;
        private int numberOfGames;

        #endregion

        #region Constructors

        public OnlineXMLOperation(string url)
        {
            xmlDoc = new XDocument();
            this.url = url;
            numberOfGames = 0;
        }

        #endregion

        #region Methods

        public void retrieveDataFromXML()
        {
            List<GameItem> listGameFromXml = new List<GameItem>();

            xmlDoc = XDocument.Load(this.url);

            listNames = (from c in xmlDoc.Root.Descendants(Constants.GAME_INFO)
                        select c.Element(Constants.NAME).Value).ToList();

            listlogoUrls = (from c in xmlDoc.Root.Descendants(Constants.GAME_INFO)
                        select c.Element(Constants.LOGO).Value).ToList();

            listIds = (from c in xmlDoc.Root.Descendants(Constants.GAME_INFO)
                       select c.Element(Constants.GAME_ID).Value).ToList();

            listPlayTime = (from c in xmlDoc.Root.Descendants(Constants.GAME_INFO)
                       select c.Element(Constants.PLAY_TIME).Value).ToList();

            if (listNames != null && listlogoUrls != null && listIds != null)
            {
                numberOfGames = listNames.Count;

                // Load local xml
                LocalXMLOperation localXml = new LocalXMLOperation(HttpContext.Current.Server.MapPath(Constants.LOCAL_XML_FILE));
                List<GameItem> lgi = localXml.RetrieveDataFromXml();
                GameItem gi;
                int lgiIndex = 0;

                for (int i = 0; i < numberOfGames; i++)
                {
                    // If the game doesn't exist in localXML then we add it
                    if (lgi.Count == 0 || !lgi.Any(str => str.IsGameExist(lgi, listNames[i])))
                        gi = new GameItem(listIds[i], listNames[i], listlogoUrls[i], listPlayTime[i]);
                    else // else we take the one in localXML (but we always update the time of playing
                    {
                        lgi[lgiIndex].PlayTime = listPlayTime[i]; // Find the right element, modify the method IsGameExist to return a game?
                        gi = lgi[lgiIndex];
                        lgiIndex++;
                    }

                    GameItemCollection.AddGame(gi);
                }
            }
        }
        #endregion
    }
}