using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace DragDropPhoneApp
{
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Threading.Tasks;

    using DragDropPhoneApp.Context;
    using DragDropPhoneApp.Model;

    using Newtonsoft.Json;

    public partial class WinnersRatingPage : PhoneApplicationPage
    {
        const string uri = "http://localhost:4232/api/values/";
        GameContext dataContext = new GameContext();
        public WinnersRatingPage()
        {
            InitializeComponent();
            var winners = dataContext.RankTableEntries.OrderByDescending(v => v.TimePassed).ToList();
            foreach (var winner in winners)
            {
                continue;
                ListBoxItem attrItem = new ListBoxItem();
                attrItem.Content = string.Format("{0} : {1} seconds", winner.UserName, winner.TimePassed);
                WinnersItemsControl.Items.Add(attrItem);
            }


        }
        private void btnSaveBill_Click(object sender, RoutedEventArgs e)
        {
            //  try
            // {
            //Call the POST on the Item wise Quote and then on the StockiestBillMaster


            string quote = "";
            //S1: Generate the JSON Serializer Data
            DataContractJsonSerializer jsonData =
                new DataContractJsonSerializer(typeof(RankTableEntry));
            MemoryStream memStream = new MemoryStream();
            //S2 : Write data into Memory Stream
            jsonData.WriteObject(memStream, quote);
            //S3 : Read the bytes from Stream do that it can then beconverted to JSON String 
            byte[] jsonDataToPost = memStream.ToArray();
            memStream.Close();
            //S4: Ehencode the stream into string format
            var data = Encoding.UTF8.GetString(jsonDataToPost, 0, jsonDataToPost.Length);
            //S5: Save Data in the QuoteMaster Table
            WebClient webClientQuote = new WebClient();
            webClientQuote.Headers["content-type"] = "application/json";
            webClientQuote.UploadStringAsync(new Uri("http://localhost:51006/api/Quote"), "POST", data);

            /*
                            Random rnd = new Random();
                            StockiestBillMaster finalStockBill = new StockiestBillMaster()
                            {
                                StockiestBillId = rnd.Next(),
                                StockiestID = Convert.ToInt32(lstStockiest.SelectedValue),
                                TotalBill = TotalBill
                            };

                            //S1: Generate the JSON Serializer Data
                            DataContractJsonSerializer jsonStockiestData =
                                new DataContractJsonSerializer(typeof(StockiestBillMaster));
                            MemoryStream memStockiestStream = new MemoryStream();
                            //S2 : Write data into Memory Stream
                            jsonStockiestData.WriteObject(memStockiestStream, finalStockBill);
                            //S3 : Read the bytes from Stream do that it can then beconverted to JSON String 
                            byte[] jsonStockiestDataToPost = memStockiestStream.ToArray();
                            memStockiestStream.Close();
                            var stockiestData =
                                Encoding.UTF8.GetString(jsonStockiestDataToPost, 0, jsonStockiestDataToPost.Length);

                            //S5 : Save the Total Bill
                            WebClient webClientTotalBill = new WebClient();
                            webClientTotalBill.UploadStringCompleted +=
                                new UploadStringCompletedEventHandler(webClientTotalBill_UploadStringCompleted);
                            webClientTotalBill.Headers["content-type"] = "application/json";
                            webClientTotalBill.UploadStringAsync(new Uri("http://localhost:51006/api/Bill"), "POST", stockiestData);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }*/
        }
        public void SendPost(RankTableEntry gizmo)
        {

            var serializedString = JsonConvert.SerializeObject(gizmo);
            //   client1.UploadStringAsync(d, serializedString);
            //  SendPost(d, serializedString);
            string quote = serializedString;
            //S1: Generate the JSON Serializer Data
            DataContractJsonSerializer jsonData =
                new DataContractJsonSerializer(typeof(RankTableEntry));
            MemoryStream memStream = new MemoryStream();
            //S2 : Write data into Memory Stream
            jsonData.WriteObject(memStream, quote);

            //S3 : Read the bytes from Stream do that it can then beconverted to JSON String 
            byte[] jsonDataToPost = memStream.ToArray();
            memStream.Close();



            //S4: Ehencode the stream into string format
            var data1 = Encoding.UTF8.GetString(jsonDataToPost, 0, jsonDataToPost.Length);
            //S5: Save Data in the QuoteMaster Table
            WebClient webClientQuote = new WebClient();
            webClientQuote.Headers["content-type"] = "application/json";
            webClientQuote.UploadStringAsync(new Uri(uri), "POST", data1);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            Uri d = new Uri(uri);
            WebClient client = new WebClient();





            client.Headers["Accept"] = "application/json";
            client.DownloadStringAsync(new Uri(uri));
            client.DownloadStringCompleted += (s1, e1) =>
            {
                try
                {
                    var data = JsonConvert.DeserializeObject<RankTableEntry[]>(e1.Result.ToString()).OrderBy(v => v.TimePassed);
                    foreach (var rankTableEntry in data)
                    {
                        ListBoxItem attrItem = new ListBoxItem();
                        attrItem.Content = string.Format("{0} : {1} seconds", rankTableEntry.UserName, rankTableEntry.TimePassed);
                        WinnersItemsControl.Items.Add(attrItem);
                    }
                }
                catch (Exception)
                {
                    
                    throw;
                }
                
            };
        }
    }
}