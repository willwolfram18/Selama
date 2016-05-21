using System.Web.Mvc;

namespace Selama.Areas.Forums
{
    public class ForumsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Forums";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Forums_default",
                "Forums/{action}/{id}",
                new { controller = "Forum", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Selama.Areas.Forums.Controllers" }
            );
        }
    }
}