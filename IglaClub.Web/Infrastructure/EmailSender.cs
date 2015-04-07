using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using SendGrid;

namespace IglaClub.Web.Infrastructure
{
    public static class EmailSender
    {
        public static void SendEmail(string receiverEmail, string receiverName, EmailTemplate template)
        {
            if (ConfigurationManager.AppSettings["EnvName"].Equals("PROD"))
            {
                var email = CreateEmail(receiverEmail, receiverName, template);

                var credentials = GetCredentials();

                var transportWeb = new SendGrid.Web(credentials);

                transportWeb.Deliver(email);
            }
        }

        private static SendGridMessage CreateEmail(string receiverEmail, string receiverName, EmailTemplate template)
        {
            string senderEmail = ConfigurationManager.AppSettings["SenderEmailAddress"];
            string senderName = ConfigurationManager.AppSettings["SenderName"];
            var email = new SendGridMessage
                {
                    From = new MailAddress(senderEmail, senderName),
                    Subject = template.Subject,
                    Text = template.Text.Replace("{{name}}", receiverName),
                    Html = template.Html.Replace("{{name}}", receiverName)
                };
            email.AddTo(receiverEmail);
            return email;
        }

        private static NetworkCredential GetCredentials()
        {
            var username = Environment.GetEnvironmentVariable("SENDGRID_USER"); //from azure portal
            var password = Environment.GetEnvironmentVariable("SENDGRID_PASS");
            var credentials = new NetworkCredential(username, password);
            return credentials;
        }
    }

    public static class EmailTemplatesDict
    {
        public static EmailTemplate NewAccount
        {
            get
            {
                return new EmailTemplate
                {
                    Subject = "Welcome to IGLAclub!",
                    Text = "Hello {{name}}, welcome to IGLAclub, you have successfully created an account. You can now log in.\n\rBest regards,\n\rIGLAclub Team",
                    Html = "Hello <b>{{name}}</b>, <br/><br/>welcome to <b>IGLAclub</b>, you have successfully created an account.<br/>You can now log in.<br/><br/>Best regards,<br/>IGLAclub Team"
                };
            }
        }
    }

    public class EmailTemplate
    {
        public string Subject { get; set; }

        public string Text { get; set; }

        public string Html { get; set; }
    }
}