using DotNetOpenAuth.OpenId.RelyingParty;
using System.Linq;
using System.Xml.Linq;
using System;
using System.Web;

namespace SteamGamesPlayed.src.Utils
{
    public static class SteamConnection
    {

        #region Properties

        private static string steamUrl;
        public static string SteamUrl
        {
            get { return steamUrl; }
            set { steamUrl = value; }
        }

        private static string username;
        public static string Username
        {
            get { return username; }
            set { username = value; }
        }

        private static bool isConnected;
        public static bool IsConnected
        {
            get
            {
                return isConnected;
            }
            set
            {
                if (!value)
                    RemoveCookie();

                isConnected = value;
            }
        }

        private static bool isPostBack;
        public static bool IsPostBack
        {
            get { return isPostBack; }
            set { isPostBack = value; }
        }

        #endregion

        public static void Connection()
        {
            var openid = new OpenIdRelyingParty();
            var response = openid.GetResponse();

            if (response != null)
            {
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        var responseURI = response.ClaimedIdentifier.ToString();

                        if (responseURI != null)
                            ExtractIdFromUri(responseURI);
                        break;

                    case AuthenticationStatus.Canceled:
                        break;
                    case AuthenticationStatus.Failed:
                        break;
                }

                HttpContext.Current.Response.Redirect("http://local.steamgamesplayed.com/");
            }
            else
            {
                using (OpenIdRelyingParty openidd = new OpenIdRelyingParty())
                {
                    isPostBack = true;
                    IAuthenticationRequest request = openidd.CreateRequest(Constants.STEAM_OPEN_ID);
                    request.RedirectToProvider();
                }
            }
        }

        public static string GetXmlUrl()
        {
            return Constants.LOCAL_XML_FILE_ROOT + username + ".xml";
        }

        private static void ExtractIdFromUri(string uri)
        {
            string steamId = uri.Substring(uri.LastIndexOf('/') + 1);

            if (steamId != null)
            {
                isConnected = true;
                isPostBack = false;
                SetUrl(steamId);
            } 
        }

        private static void SetUrl(string steamId)
        {
            steamUrl = "http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=" +
                Constants.STEAM_KEY + "&steamid=" + steamId + "&format=xml&include_appinfo=1";

            GetUsername(steamId);
        }

        private static void GetUsername(string steamId)
        {
            var xmlDoc = XDocument.Load("http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=" +
                Constants.STEAM_KEY + "&steamids=" + steamId + "&format=xml&include_appinfo=1");

            username = (from c in xmlDoc.Root.Descendants(Constants.STEAM_PLAYER)
                         select c.Element(Constants.STEAM_USERNAME).Value).FirstOrDefault();

            SetCookie();
        }

        private static void RemoveCookie()
        {
            if (HttpContext.Current.Request.Cookies["steamConnexion"] != null)
                HttpContext.Current.Response.Cookies["steamConnexion"].Expires = DateTime.Now.AddDays(-1);
        }

        public static void SetCookie()
        {
            if (username != null && steamUrl != null)
            {
                HttpCookie cookie = new HttpCookie("steamConnexion");
                cookie.Values.Add("username", username);
                cookie.Values.Add("url", HttpUtility.UrlEncode(steamUrl));
                cookie.Expires = DateTime.Now.AddDays(7);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static void GetCookie()
        {
            var cookie = HttpContext.Current.Request.Cookies["steamConnexion"];

            if (cookie != null && cookie["username"] != "" && cookie["url"] != "")
            {
                Username = HttpContext.Current.Request.Cookies["steamConnexion"]["username"];
                SteamUrl = HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies["steamConnexion"]["url"]);
                isConnected = true;
            }
        }
    }
}