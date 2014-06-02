
using System.Globalization;
using System.Threading;
using System.Web.SessionState;

namespace BuildSeller.Helpers
{

    public class SessionManager
    {

        protected HttpSessionState session;

        public SessionManager(HttpSessionState httpSessionState)
        {
            this.session = httpSessionState;
        }

        public static int CurrentCulture
        {
            get
            {
                if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                {
                    return 0;
                }

                if (Thread.CurrentThread.CurrentUICulture.Name == "ru-RU")
                {
                    return 1;
                }

                return 0;
            }

            set
            {
                if (value == 0)
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                }
                else if (value == 1)
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
                }
                else
                {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                }

                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            }
        }
    }
}
