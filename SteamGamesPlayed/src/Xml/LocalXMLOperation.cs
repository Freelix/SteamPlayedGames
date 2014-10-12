using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml;

namespace SteamGamesPlayed
{
    public class LocalXMLOperation
    {
        #region Attributes

        private XmlDocument xmlDoc;
        private string path;

        #endregion

        #region Constructors

        public LocalXMLOperation(string path)
        {
            xmlDoc = new XmlDocument();
            this.path = path;
        }

        public LocalXMLOperation()
        {
            xmlDoc = new XmlDocument();
            path = string.Empty;
        }

        #endregion

        public XmlDocument XmlDoc
        {
            get { return xmlDoc; }
        }

        #region Methods

        public void SaveAllDataToXml()
        {
            XmlNode rootNode, gNode;

            // Create root
            rootNode = xmlDoc.CreateElement("Games");
            xmlDoc.AppendChild(rootNode);

            // Create Declaration
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.InsertBefore(xmlDeclaration, rootNode);

            // Create a node for each ListItem
            XmlNode id, name, status, score, image, playTime;

            foreach (GameItem gi in GameItemCollection.Games)
            {
                // Create gNode
                gNode = xmlDoc.CreateElement("Game");

                id = xmlDoc.CreateElement("Id");
                name = xmlDoc.CreateElement("Name");
                status = xmlDoc.CreateElement("Status");
                score = xmlDoc.CreateElement("Score");
                image = xmlDoc.CreateElement("Image");
                playTime = xmlDoc.CreateElement("PlayTime");

                id.InnerText = gi.Id;
                name.InnerText = gi.Name;
                status.InnerText = Convert.ToString(Convert.ToInt32(gi.Option));
                score.InnerText = gi.Score != null ? gi.Score.ToString() : "0";
                image.InnerText = gi.Image;
                playTime.InnerText = gi.PlayTime;

                gNode.AppendChild(id);
                gNode.AppendChild(name);
                gNode.AppendChild(status);
                gNode.AppendChild(score);
                gNode.AppendChild(image);
                gNode.AppendChild(playTime);

                rootNode.AppendChild(gNode);
            }

            // Save to File
            xmlDoc.Save(this.path);
        }

        /*public void LoadDataFromXml()
        {
            // Retrieve a list of all specified nodes
            XElement file = XElement.Load(HttpContext.Current.Server.MapPath(Constants.LOCAL_XML_FILE));
            IEnumerable<XElement> elements = file.Descendants("Game");

            string id, name, image, playTime;
            int score;
            GameItem.Status status;

            foreach (XElement el in elements)
            {
                id = ((XElement)(((XContainer)(el)).FirstNode)).Value;
                name = ((XElement)(((XContainer)(el)).FirstNode.NextNode)).Value;
                status = (GameItem.Status) Enum.Parse(typeof(GameItem.Status), ((XElement)(((XContainer)(el)).FirstNode.NextNode.NextNode)).Value);
                score = Convert.ToInt32(((XElement)(((XContainer)(el)).LastNode.PreviousNode.PreviousNode)).Value);
                image = ((XElement)(((XContainer)(el)).LastNode.PreviousNode)).Value;
                playTime = ((XElement)(((XContainer)(el)).LastNode)).Value;

                new GameItem(id, name, image, score, status, playTime);
            }
        }*/

        public List<GameItem> RetrieveDataFromXml()
        {
            List<GameItem> lgi = new List<GameItem>();

            // Retrieve a list of all specified nodes
            XElement file = XElement.Load(HttpContext.Current.Server.MapPath(Constants.LOCAL_XML_FILE));
            IEnumerable<XElement> elements = file.Descendants("Game");

            string id, name, image, playTime;
            int score;
            GameItem.Status status;

            foreach (XElement el in elements)
            {
                id = ((XElement)(((XContainer)(el)).FirstNode)).Value;
                name = ((XElement)(((XContainer)(el)).FirstNode.NextNode)).Value;
                status = (GameItem.Status)Enum.Parse(typeof(GameItem.Status), ((XElement)(((XContainer)(el)).FirstNode.NextNode.NextNode)).Value);
                score = Convert.ToInt32(((XElement)(((XContainer)(el)).LastNode.PreviousNode.PreviousNode)).Value);
                image = ((XElement)(((XContainer)(el)).LastNode.PreviousNode)).Value;
                playTime = ((XElement)(((XContainer)(el)).LastNode)).Value;

                //Metacritic mc = new Metacritic();
                lgi.Add(new GameItem(id, name, image, score, status, playTime));               
            }

            return lgi;
        }

        #region Update Xml File

        public void SaveStatusToXml(string id)
        {
            xmlDoc.Load(HttpContext.Current.Server.MapPath(Constants.LOCAL_XML_FILE));

            if (UpdateStatusById(id))
                xmlDoc.Save(HttpContext.Current.Server.MapPath(Constants.LOCAL_XML_FILE));    
        }

        public void SaveScoreToXml(string id, Nullable<int> score)
        {
            xmlDoc.Load(HttpContext.Current.Server.MapPath(Constants.LOCAL_XML_FILE));

            if (UpdateScoreById(id, score))
                xmlDoc.Save(HttpContext.Current.Server.MapPath(Constants.LOCAL_XML_FILE));

        }

        public bool UpdateScoreById(string id, Nullable<int> score)
        {
            XmlNode node;
            node = xmlDoc.DocumentElement;

            foreach (XmlNode node1 in node.ChildNodes)
            {
                foreach (XmlNode node2 in node1.ChildNodes)
                    if (node2.Name == "Id" && node2.InnerText == id)
                    {
                        node2.NextSibling.NextSibling.NextSibling.InnerText = score.ToString(); // Element : Score
                        return true;
                    }
            }

            return false;
        }

        private bool UpdateStatusById(string id)
        {
            XmlNode node;
            node = xmlDoc.DocumentElement;

            foreach (XmlNode node1 in node.ChildNodes)
            {
                foreach (XmlNode node2 in node1.ChildNodes)
                    if (node2.Name == "Id" && node2.InnerText == id)
                    {
                        node2.NextSibling.NextSibling.InnerText = node2.NextSibling.NextSibling.InnerText == "0" ? "1" : "0"; // Element : Status
                        return true;
                    }
            }

            return false;
        }

        #endregion

        #endregion
    }
}