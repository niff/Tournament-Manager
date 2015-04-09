using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using SendGrid;

namespace IglaClub.Web.Infrastructure
{
    public static class EmailSender
    {
        public static void SendEmail(string receiverEmail, string receiverName, EmailTemplate template, string link = null)
        {
                var email = CreateEmail(receiverEmail, receiverName, template, link);

                var credentials = GetCredentials();

                var transportWeb = new SendGrid.Web(credentials);

                if (ConfigurationManager.AppSettings["EnvName"].Equals("PROD"))
                {
                    transportWeb.Deliver(email);
                }
                else
                {
                    var file = new System.IO.FileStream("C:/Temp/mail/"+DateTime.Now.Ticks+".txt", FileMode.OpenOrCreate);
                    file.Write(Encoding.UTF8.GetBytes(email.Html), 0, email.Html.Length);

                }
        }

        private static SendGridMessage CreateEmail(string receiverEmail, string receiverName, EmailTemplate template, string link = null)
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
            if (link != null)
            {
                email.Text = email.Text.Replace("{{link}}", link);
                email.Html = email.Html.Replace("{{link}}", link);
            }
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

        public static EmailTemplate ResetPassword
        {
            get
            {
                return new EmailTemplate
                {
                    Subject = "Password recovery for your IGLAclub account",
                    Text = "Hello {{name}}, you asked for password recovery in IGLAclub, click the link below to set up new password\n\r{{link}}\n\rBest regards,\n\rIGLAclub Team",
                    Html = "Hello <b>{{name}}</b>, <br/><br/>you asked for password recovery in <b>IGLAclub</b>, click the link below to set up new password<br/>{{link}}<br/><br/>Best regards,<br/>IGLAclub Team"
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