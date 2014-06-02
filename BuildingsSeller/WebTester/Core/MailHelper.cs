
using System.Net.Mail;

namespace BuildSeller.Core
{

    public class MailHelper
    {

        public SmtpClient smtpServer { get; set; }

        public bool SendMail(string from, string to, string subj, string body)
        {
            var mail = new MailMessage();

            mail.From = new MailAddress(from);
            mail.To.Add(to);
            mail.Subject = subj;
            mail.Body = body;
            this.smtpServer.Send(mail);
            return true;
        }
    }
}
