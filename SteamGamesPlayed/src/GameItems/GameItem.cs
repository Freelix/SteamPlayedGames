using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteamGamesPlayed
{
    public class GameItem
    {
        public enum Status
        {
            NotPlayed = 0,
            Finished = 1,
            NotFinished = 2
        };

        #region Attributes

        private string id;
        private string name;
        private string image;
        private Status option;
        private Metacritic meta;
        private int score;
        private string color;
        private string playTime;

        #endregion

        #region Constructors

        // Used by OnlineXMLOperation
        public GameItem(string id, string name, string image, string playTime)
        {
            this.id = id;
            this.name = name.Replace("\"", "\\\"");
            this.image = ConstructLogoUrl(image.Replace("\"", "\\\""));
            this.option = Status.NotPlayed;
            this.meta = new Metacritic();
            this.playTime = GetTimeFromOnlineXml(playTime);
            this.color = "#FFFFFF";
            //GameItemCollection.AddGame(this);
        }

        /*public GameItem(string id, string name, string image, int metaScore, string playTime)
        {
            this.id = id;
            this.name = name.Replace("\"", "\\\""); ;
            this.image = ConstructLogoUrl(image.Replace("\"", "\\\""));
            this.option = Status.NotPlayed;
            this.metaScore = metaScore;
            this.playTime = GetTimeFromOnlineXml(playTime);
            EvaluateScore();
            GameItemCollection.AddGame(this);
        }*/

        // Used by LocalXMLOperation
        public GameItem(string id, string name, string image, int score, GameItem.Status status, string playTime)
        {
            this.id = id;
            this.name = name.Replace("\"", "\\\"");
            this.image = image.Replace("\"", "\\\"");
            this.option = status;
            this.meta = new Metacritic();
            this.score = score;
            this.playTime = playTime;
            EvaluateScore();
            //GameItemCollection.AddGame(this);
        }
        
        #endregion

        #region Properties

        public String Id
        {
            get { return id; }
            set { id = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Image
        {
            get { return image; }
            set { image = value; }
        }

        public Status Option
        {
            get { return option; }
            set { option = value; }
        }

        public Metacritic Meta
        {
            get { return meta; }
            set { meta = value; }
        }

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        public string PlayTime
        {
            get { return playTime; }
            set { playTime = value; }
        }

        #endregion

        #region Methods

        private string ConstructLogoUrl(string image)
        {
            if (this.id != string.Empty && image != string.Empty)
                return string.Format("http://media.steampowered.com/steamcommunity/public/images/apps/{0}/{1}.jpg", this.id, image);
            else
                return "Styles/Images/steam.jpg";
        }

        // The format in playTime is like a number (ex:"5464")
        private string GetTimeFromOnlineXml(string playTime) //
        {
            int hour = 0;
            string min = "00";

            if (playTime != string.Empty)
            {
                try
                {
                    // Retrieve hours
                    int minutes = Convert.ToInt32(playTime);
                    TimeSpan ts = TimeSpan.FromMinutes(minutes);

                    if (ts.Days > 0)
                        hour = ts.Days * 24;

                    hour += ts.Hours;

                    // Retrieve minutes
                    int minTemp = ts.Minutes;

                    if (minTemp < 10)
                        min = string.Format("0{0}", minTemp);
                    else
                        min = Convert.ToString(minTemp);
                }
                catch
                {
                    return "Time Error";
                }
            }

            return string.Format("{0}h{1}", hour, min);
        }

        private void EvaluateScore()
        {
            if (score < 60)
                color = "#990000";
            else if (score >= 60 && score < 80)
                color = "#A681CE";
            else if (score >= 80 && score < 95)
                color = "#008000";
            else if (score >= 95)
                color = "#FFD700";
            else
                color = "#FFFFFF";
        }

        public bool IsCompleted()
        {
            if (this.option == Status.Finished)
                return true;
            return false;
        }

        #endregion

        public bool IsGameExist(List<GameItem> list, string name)
        {
            return list.Any(c => c.Name  == name);
        }
    }
}