using System.Web.Mvc;

namespace Selama.Areas.Account
{
    public class AccountAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Account";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Account_manage",
                "Account/Manage/{action}/{id}",
                new { controller = "Manage", action = "Index", id = UrlParameter.Optional },
                new string[] { "Selama.Areas.Account.Controllers" }
            );
            context.MapRoute(
                "Account_default",
                "Account/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "Selama.Areas.Account.Controllers" }
            );
        }
    }
}