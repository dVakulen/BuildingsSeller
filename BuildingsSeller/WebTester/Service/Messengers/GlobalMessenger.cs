
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using BuildSeller.Core;
using BuildSeller.Core.Model;
using BuildSeller.Core.Properties;
using BuildSeller.Core.Service;
using BuildSeller.Infra;

namespace BuildSeller.Service
{

    public class GlobalMessenger
    {

        public static MailHelper mailSender;

        private static IUserService UserServic;

        private static readonly ISubscribeService SubscribeService;

        static GlobalMessenger()
        {
            mailSender = new MailHelper();
            mailSender.smtpServer = new SmtpClient("smtp.gmail.com", 587);
            mailSender.smtpServer.UseDefaultCredentials = false;
            mailSender.smtpServer.Credentials = new NetworkCredential("dimitriu777@gmail.com",
            "710914710914" + Resources.pw);
            mailSender.smtpServer.EnableSsl = true;
            SubscriptionMessenger subscriptionMessenger = SubscriptionMessenger.Instance;
            subscriptionMessenger.RealtyAdded += (sender, args) => c_RealtyAdded(sender, args);
            UserServic = IoC.Resolve<IUserService>();
            SubscribeService = IoC.Resolve<ISubscribeService>();
        }

        public static void X()
        {
        }

        public static void Send(string message, string Title, string from, List<Users> adresatsList)
        {
            foreach (Users adresat in adresatsList)
            {
                mailSender.SendMail(@"@gmail.com", adresat.Email, Title, message);
            }
        }

        public static void SendToOne(string message, string Title, string from, Users adresat)
        {
            mailSender.SendMail(@"@gmail.com", adresat.Email, Title, message);
        }

        private static void c_RealtyAdded(object sender, RealtyCreatedEventArgs e)
        {
            IEnumerable<Subscribe> subscrList = SubscribeService.Where(x => x.Name == e.Reallty.BuildCategory.CatName);
            var usList = new List<Users>();
            foreach (Subscribe subscribe in subscrList)
            {
                usList.Add(subscribe.Subscriber);
            }

            string message = "New realty" + e.Reallty.Named + " were added to our assortiment \n" +
            "You can see details by clicking the following link : /Realty/Details/" +
            e.Reallty.Id;

            Send(message, "New realty for you", string.Empty, usList);
        }
    }
}
