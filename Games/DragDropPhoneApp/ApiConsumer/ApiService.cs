using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragDropPhoneApp.ApiConsumer
{
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization.Json;
    using System.Threading;
    using System.Windows;
    using System.Windows.Navigation;
    using System.Windows.Threading;

    using DragDropPhoneApp.Model;
    using DragDropPhoneApp.ViewModel;

    using Microsoft.Phone.Controls;

    using Newtonsoft.Json;

    static class ApiService<T> where T : class
    {
        private static void StartWebRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.BeginGetResponse(new AsyncCallback(FinishWebRequest), request);
        }

        private static void FinishWebRequest(IAsyncResult result)
        {
            try
            {
                Thread.Sleep(1000);
                HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
  Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("aa");

                    if (((PhoneApplicationFrame)Application.Current.RootVisual).DataContext is MainViewModel)
                    (((PhoneApplicationFrame)Application.Current.RootVisual).DataContext as MainViewModel).IsLoading =
                        false;
                    ((PhoneApplicationFrame)Application.Current.RootVisual).Navigate(new Uri("/Menu.xaml", UriKind.Relative));
                }); 
                var z = response.Headers;
                var b = z;
            }
            catch (WebException)
            {
                
            }
          
        }

        public static void Login(string login, string pass)
        {

            WebClient client = new WebClient();


            HttpWebRequest myReq =
(HttpWebRequest)WebRequest.Create(uri.OriginalString + string.Format("?login={0}&pass={1}", login, pass));

  //       Uri uri = new Uri(http://host.ru/forum/cont/add.php?parm=p);
HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
            StartWebRequest(uri.OriginalString + string.Format("?login={0}&pass={1}", login, pass));
            object s = new object();
          //  IAsyncResult httpWebResponse = httpWebRequest.BeginGetResponse(ar => { }, s);
          //  while (!httpWebResponse.IsCompleted)
            {
             //  Thread.Sleep(100);
            }
           
//StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream());

            return;
            
            client.Headers["Accept"] = "application/json";
            client.DownloadStringAsync(new Uri(uri.OriginalString + string.Format("?login={0}&pass={1}", login, pass)));
            var z = client.ResponseHeaders;
            var b = z;
           
            client.DownloadStringCompleted += (s1, e1) =>
            {

                try
                {
                //    var data = JsonConvert.DeserializeObject<RankTableEntry[]>(e1.Result.ToString()).OrderBy(v => v.TimePassed);

                }
                catch (Exception)
                {

                    throw;
                }

            };
        }
        public static void Get()
        {
            WebClient client = new WebClient();





            client.Headers["Accept"] = "application/json";
            client.DownloadStringAsync(uri);
            client.DownloadStringCompleted += (s1, e1) =>
            {
                try
                {
                    var data = JsonConvert.DeserializeObject<RankTableEntry[]>(e1.Result.ToString()).OrderBy(v => v.TimePassed);

                }
                catch (Exception)
                {

                    throw;
                }

            };
        }
        static Uri uri = new Uri("http://localhost:61251/api/buildapi/");
        public static void SendPost(T gizmo)
        {



            var serializedString = JsonConvert.SerializeObject(gizmo);
            //   client1.UploadStringAsync(d, serializedString);
            //  SendPost(d, serializedString);
            string quote = serializedString;
            //S1: Generate the JSON Serializer Data
            DataContractJsonSerializer jsonData =
                new DataContractJsonSerializer(typeof(T));
            MemoryStream memStream = new MemoryStream();
            //S2 : Write data into Memory Stream
            jsonData.WriteObject(memStream, quote);

            //S3 : Read the bytes from Stream do that it can then beconverted to JSON String 
            byte[] jsonDataToPost = memStream.ToArray();
            memStream.Close();



            //S4: Ehencode the stream into string format
            var data1 = Encoding.UTF8.GetString(jsonDataToPost, 0, jsonDataToPost.Length);


            try
            {
                T f1d = JsonConvert.DeserializeObject<T>(serializedString);
                var gb1 = f1d;
                //    T fd = JsonConvert.DeserializeObject<T>(data1);
                //   var gb = fd;
            }
            catch (Exception)
            {

            }

            //S5: Save Data in the QuoteMaster Table
            WebClient webClientQuote = new WebClient();

            webClientQuote.Headers["content-type"] = "application/json";
            webClientQuote.UploadStringAsync((uri), "POST", data1);
        }
    }
}
