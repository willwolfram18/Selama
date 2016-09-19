using MarkdownDeep;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Selama.Common.Utility
{
    public static class Util
    {
        public const string WOW_CHARACTER_THUMBNAIL_URL = "https://render-api-us.worldofwarcraft.com/static-render/us/{0}-avatar.jpg";
        public const string WOW_CHARCTER_MODEL_URL = "https://render-api-us.worldofwarcraft.com/static-render/us/{0}-profilemain.jpg";

        public static string WowRealmName
        {
            get
            {
                return ConfigurationManager.AppSettings["WowRealmName"].ToString();
            }
        }

        public static string WowGuildName
        {
            get
            {
                return ConfigurationManager.AppSettings["WowGuildName"].ToString();
            }
        }

        public static string BattleNetApiClientId
        {
            get
            {
                return ConfigurationManager.AppSettings["BattleNetApiClientId"];
            }
        }

        public static string BattleNetApiClientSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["BattleNetApiClientSecret"];
            }
        }

        public static Markdown Markdown = new Markdown
        {
            SafeMode = true,
            ExtraMode = true,
            MarkdownInHtml = true,
        };
    }
}