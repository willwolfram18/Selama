using MarkdownDeep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Selama.Common.Utility
{
    public static class Util
    {
        public const string WOW_CHARACTER_THUMBNAIL_URL = "https://render-api-us.worldofwarcraft.com/static-render/us/{0}-avatar.jpg";
        public const string WOW_CHARCTER_MODEL_URL = "https://render-api-us.worldofwarcraft.com/static-render/us/{0}-profilemain.jpg";

        public static Markdown Markdown = new Markdown
        {
            SafeMode = true,
            ExtraMode = true,
            MarkdownInHtml = true,
        };
    }
}