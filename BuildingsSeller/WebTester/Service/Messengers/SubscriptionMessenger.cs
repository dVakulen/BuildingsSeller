
using System;
using System.Collections.Generic;
using System.Linq;
using BuildSeller.Core.Model;
using BuildSeller.Infra;

namespace BuildSeller.Service
{

    public class SubscriptionMessenger
    {

        private static SubscriptionMessenger instance;

        private SubscriptionMessenger()
        {
        }

        private ISubscribeService SubscribeServic;

        public static SubscriptionMessenger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SubscriptionMessenger();
                    instance.SubscribeServic = IoC.Resolve<ISubscribeService>();
                }
                return instance;
            }
        }

        public void SendRealtyCreated(Realty real)
        {
            var args = new RealtyCreatedEventArgs();
            args.Reallty = real;
            args.TimeReached = DateTime.Now;
            realtyAdded(null, args);

        }

        protected virtual void OnRealtyCreated(RealtyCreatedEventArgs e)
        {
            EventHandler<RealtyCreatedEventArgs> handler = this.RealtyAdded;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        private void realtyAdded(object sender, RealtyCreatedEventArgs e)
        {
            IEnumerable<Subscribe> subscrList = SubscribeServic.GetAllIncluding(x => x.Subscriber).Where(x => x.Name == e.Reallty.BuildCategory.CatName);
            var usList = new List<Users>();
            if (subscrList.Count() == 0)
            {
                return;
            }
            foreach (Subscribe subscribe in subscrList)
            {
                usList.Add(subscribe.Subscriber);
            }

            string message = "New realty" + e.Reallty.Named + " were added to our assortiment \n" +
            "You can see details by clicking the following link : /Realty/Details/" +
            e.Reallty.Id;

            GlobalMessenger.Send(message, "New realty for you", string.Empty, usList);
        }

        public event EventHandler<RealtyCreatedEventArgs> RealtyAdded;
    }

    public class RealtyCreatedEventArgs : EventArgs
    {

        public Realty Reallty { get; set; }

        public DateTime TimeReached { get; set; }
    }
}
