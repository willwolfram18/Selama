using Selama.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using System.Web.Security;

namespace Selama.Classes.Events
{
    public class NewUserRegistrationEvent
    {
        public string UserOverviewUrl { get; set; }
        public string NewUserName { get; set; }
        public string NewUserEmail { get; set; }

        public void NotifyAdmin()
        {
            string sendGridApiKey = ConfigurationManager.AppSettings["SendGridApiKey"];
            var sendGridApi = new SendGridAPIClient(sendGridApiKey);

            var sender = new Email(ConfigurationManager.AppSettings["EmailSmtpFromAddress"]);
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (var username in Roles.GetUsersInRole("Admin"))
                {
                    var user = db.Users.Where(u => u.UserName == username).FirstOrDefault();
                    SendEmail(sendGridApi, sender, UserOverviewUrl, user);
                }

                foreach (var username in Roles.GetUsersInRole("Guild Officer"))
                {
                    var user = db.Users.Where(u => u.UserName == username).FirstOrDefault();
                    SendEmail(sendGridApi, sender, UserOverviewUrl, user);
                }
            }
        }

        private void SendEmail(SendGridAPIClient sendGridApi, Email sender, string userOverviewUrl, ApplicationUser user)
        {
            var recipient = new Email(user.Email);
            // TODO: Include user information like username and email
            Content content = new Content("text/html",
                string.Format("A new user is awaiting approval. Please verify the user <a href='{0}'>here</a>.",
                userOverviewUrl
            ));
            Mail mail = new Mail(sender, "New User Request", recipient, content);

            var response = sendGridApi.client.mail.send.post(requestBody: mail.Get());
        }
    }
}