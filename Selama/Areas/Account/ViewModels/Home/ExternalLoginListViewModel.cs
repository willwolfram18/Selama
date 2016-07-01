namespace Selama.Areas.Account.ViewModels.Home
{
    public class ExternalLoginListViewModel
    {
        public ExternalLoginListViewModel()
        {
            ButtonMsg = "Log in";
        }
        public string ReturnUrl { get; set; }

        public string ButtonMsg { get; set; }
    }
}