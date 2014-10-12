using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteamGamesPlayed
{
    public class Constants
    {
        public const string GAME_ID = "appid";
        public const string NAME = "name";
        public const string GAME_INFO = "message";
        public const string PLAY_TIME = "playtime_forever";
        public const string ICON = "img_icon_url";
        public const string LOGO = "img_logo_url";
        public const string SITE_URL = "http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=F1854E7F4338467B6BDEDF574669D483&steamid=76561198001807062&format=xml&include_appinfo=1";
        public const string STATUS_COMPLETE = "I finished this game";
        public const string STATUS_INCOMPLETE = "I have not finished this game yet";
        
        // Headers names
        public const string COMPLETED_LIST = "Completed Games";
        public const string NOT_COMPLETED_LIST = "Not Completed Games";
        public const string RELEVANCE = "By Relevance";
        public const string ALPHABETICAL = "By Alphabetical Order";
        public const string ORDER_PT = "By Playing Time";
        
        public const string LOCAL_XML_FILE = "../Xml/SavedItems.xml";
    }
}