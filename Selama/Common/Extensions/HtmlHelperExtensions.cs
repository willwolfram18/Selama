using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Selama.Common.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString OpenGraphProtocolMetaTag(this HtmlHelper helper, string propertyName, string content)
        {
            return new MvcHtmlString(string.Format("<meta property='og:{0}' content='{1}' />", propertyName, content));
        }

        public static MvcHtmlString FontAwesomeIconTextActionLink(this HtmlHelper Html, string fontAwesomeIconName, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            string htmlGeneratedLink = Html.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes).ToString();
            int endOfOpeningTag = htmlGeneratedLink.IndexOf(">") + 1;
            htmlGeneratedLink = htmlGeneratedLink.Insert(endOfOpeningTag, string.Format("<span class='fa fa-{0} fa-icontext'></span>", fontAwesomeIconName));
            return new MvcHtmlString(htmlGeneratedLink);
        }
    }
}