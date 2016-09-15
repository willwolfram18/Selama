using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Selama.Common.ExtensionMethods
{
    public static class HtmlHelperExtensionMethods
    {
        public static MvcHtmlString OpenGraphProtocolMetaTag(this HtmlHelper helper, string propertyName, string content)
        {
            return new MvcHtmlString(string.Format("<meta property='og:{0}' content='{1}' />", propertyName, content));
        }
    }
}