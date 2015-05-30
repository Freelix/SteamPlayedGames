using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using unirest_net.http;
using unirest_net.request;
using System.Threading.Tasks;
using System.Diagnostics;
using FluentMetacritic.Domain;

/*
 * NuGet Package Manager - Extension
 * Unirest for .Net in "Package Manager Console"
 * ----------------
 * 
 * Install
 * --------
 * 
 * Install-Package Unirest-Net -Pre
 * 
 * See all packages
 * -----------------
 * 
 * Get-Package 
 * 
 * Uninstall
 * ----------
 * 
 * Uninstall-Package Unirest-Net
 */

namespace SteamGamesPlayed
{
    public class Metacritic
    {
        #region Attributes

        //private static string key = "E5vTpX1ZQrmshCLAocoRv8CTZtRWp1C3KRJjsnJzLMoojyxUmo";

        private Nullable<int> score;
        private DateTime releaseDate;
        private string rating;
        private string publisher;
        private string url;
        private string description;

        #endregion

        #region Properties

        public Nullable<int> Score
        {
            get { return score; }
            set { score = value; }
        }

        public string Description
        {
            get { return description; }
        }

        #endregion

        public Metacritic(Nullable<int> score)
        {

                this.score = 0;
            releaseDate = new DateTime();
            rating = string.Empty;
            publisher = string.Empty;
            url = string.Empty;
            description = string.Empty;
        }

        public Metacritic()
        {
            score = 0;
            releaseDate = new DateTime();
            rating = string.Empty;
            publisher = string.Empty;
            url = string.Empty;
            description = string.Empty;
        }

        public Metacritic GetDataFromMetacritic(string name)
        {
            string platform = "PC";

            IEnumerable<IGame> result = null;

            try
            {
                result = FluentMetacritic.Metacritic.SearchFor().Games().UsingText(name);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error occured : " + e.Message);
            }

            if (result != null)
                foreach (var el in result)
                {
                    if (el.Platform == platform)
                    {
                        description = el.Description;
                        score = el.Score;
                        releaseDate = el.ReleaseDate;
                        rating = el.MaturityRating;
                        publisher = el.Publisher;

                        return this;
                    }
                }

            return null;
        }

    }
}