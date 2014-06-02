
using System;
using System.Collections.Generic;
using System.Timers;
using BuildSeller.Core.Model;
using BuildSeller.Core.Service;

namespace BuildSeller
{

    public class Worker
    {

        private static IUserService us;

        public Worker(IUserService u)
        {
            us = u;
        }

        public void Start()
        {
            var t = new Timer();
            t.Elapsed += this.Execute;

            t.Interval = 60 * 60 * 1000;
            t.Enabled = true;
            t.AutoReset = true;
            t.Start();
        }

        protected void Execute(object sender, ElapsedEventArgs e)
        {
            IList<Users> usersToDelete =
            us.Where(x => x.Activated == false && x.RegisterDateTime < DateTime.Now.AddDays(-1));
            foreach (Users user in usersToDelete)
            {
                us.Delete(user.Id);
            }
        }
    }
}
