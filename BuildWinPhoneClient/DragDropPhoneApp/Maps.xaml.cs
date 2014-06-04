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
    using System.Device.Location;
    using System.Diagnostics;
    using System.IO.IsolatedStorage;
    using System.Reflection;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    using Windows.Devices.Geolocation;

    using DragDropPhoneApp.ViewModel;

    using Microsoft.Phone.Maps.Controls;
    using Microsoft.Phone.Maps.Services;

    public partial class MapPage : PhoneApplicationPage
    {
        #region Fields

        private MapOverlay DestinationMarker;

        private bool DestinationRevGeoNow;

        private MapOverlay OriginMarker;

        private RouteOptimization optimization = RouteOptimization.MinimizeTime;

        private TravelMode travelMode = TravelMode.Driving;

        private bool draggingNow;

        private RouteQuery geoQ;

        private ReverseGeocodeQuery geoRev;

        private MapRoute lastRoute;

        private MapLayer markerLayer;

        private MapOverlay selectedMarker;
        private MainViewModel dataContext; 
        #endregion

        // Constructor

        #region Constructors and Destructors

        public MapPage()
        {
            this.InitializeComponent();
            dataContext = App.DataContext;
            DataContext = App.DataContext;
            Touch.FrameReported += this.Touch_FrameReported;

            this.map1.ZoomLevelChanged += this.map1_ZoomLevelChanged;

            this.AddResultToMap(
                new GeoCoordinate(dataContext.CurrentRealty.MapPosX, dataContext.CurrentRealty.MapPosY),
                new GeoCoordinate(dataContext.CurrentRealty.MapPosX + 1, dataContext.CurrentRealty.MapPosY+1));

            this.geoRev = new ReverseGeocodeQuery();
            this.geoRev.QueryCompleted += this.geoRev_QueryCompleted;

            this.geoQ = new RouteQuery();
            this.geoQ.QueryCompleted += this.geoQ_QueryCompleted;
            StartGeoLoc();
            if (dataContext.isInRealtyCreating)
            {
                this.Save.IsEnabled = false;
                this.GetRouteBtn.Visibility = Visibility.Collapsed;
                this.Submit.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region Methods
        private async void StartGeoLoc()
        {

          
GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            watcher.MovementThreshold = 20;
             watcher.Start();
             watcher.PositionChanged += (o, args) =>
                 {
                     this.DestinationMarker.GeoCoordinate = watcher.Position.Location;
                     Start_ReverceGeoCoding(this.DestinationMarker);
                     StartGeoQ();
                 };
      
        }
        private void AddResultToMap(GeoCoordinate origin, GeoCoordinate destination)
        {
            if (this.markerLayer != null)
            {
                this.map1.Layers.Remove(this.markerLayer);
                this.markerLayer = null;
            }

            this.OriginMarker = this.MakeDotMarker(origin, false);
           this.DestinationMarker = this.MakeDotMarker(destination, true);
            this.map1.SetView(origin, 14);
            this.markerLayer = new MapLayer();
            this.map1.Layers.Add(this.markerLayer);
            this.markerLayer.Add(this.OriginMarker);
            this.markerLayer.Add(this.DestinationMarker);
            
        }

        private void StartGeoQ()
        {
            if (this.geoQ.IsBusy)
            {
                this.geoQ.CancelAsync();
            }

            this.geoQ.InitialHeadingInDegrees = this.map1.Heading;

            this.geoQ.RouteOptimization = this.optimization;
            this.geoQ.TravelMode = this.travelMode;

            List<GeoCoordinate> MyWayPoints = new List<GeoCoordinate>();
            MyWayPoints.Add(this.OriginMarker.GeoCoordinate);
            MyWayPoints.Add(this.DestinationMarker.GeoCoordinate);

            this.geoQ.Waypoints = MyWayPoints;
            this.geoQ.QueryAsync();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
            if (sender == this.GetRouteBtn)
            {
                StartGeoQ();
            }
        }

        private MapOverlay MakeDotMarker(GeoCoordinate location, bool isDestination)
        {
            MapOverlay Marker = new MapOverlay();
            
            Marker.GeoCoordinate = location;

            Ellipse circle = new Ellipse();
            if (isDestination)
            {
                circle.Fill = new SolidColorBrush(Colors.Green);
                circle.Stroke = new SolidColorBrush(Colors.Orange);
            }
            else
            {
                circle.Fill = new SolidColorBrush(Colors.Yellow);
                circle.Stroke = new SolidColorBrush(Colors.Red);
            }

            circle.StrokeThickness = 20;
            circle.Opacity = 0.8;
            circle.Height = 50;
            circle.Width = 50;
            if (isDestination && dataContext.isInRealtyCreating)
            {
              //  circle.Visibility = Visibility.Collapsed;
            }
            Marker.Content = circle;
            Marker.PositionOrigin = new Point(0.5, 0.5);
            circle.MouseLeftButtonDown += this.textt_MouseLeftButtonDown;

            return Marker;
        }

        private void Start_ReverceGeoCoding(MapOverlay Marker)
        {
            if (this.geoRev.IsBusy != true && (Marker != null))
            {
                if (Marker == this.DestinationMarker)
                {
                    this.DestinationRevGeoNow = true;
                    this.DestinationTitle.Text = string.Empty;
                }
                else
                {
                    this.DestinationRevGeoNow = false;
                    this.OriginTitle.Text = string.Empty;
                }

                this.geoRev.GeoCoordinate = Marker.GeoCoordinate;
                this.geoRev.QueryAsync();
            }
        }

        private void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {return;
            
            if (this.draggingNow)
            {
                TouchPoint tPoint = e.GetPrimaryTouchPoint(this.map1);

                if (tPoint.Action == TouchAction.Move && (this.selectedMarker != null))
                {
                    this.selectedMarker.GeoCoordinate = this.map1.ConvertViewportPointToGeoCoordinate(tPoint.Position);
                    this.Start_ReverceGeoCoding(this.selectedMarker);
                }
                else if (tPoint.Action == TouchAction.Up)
                {
                    this.selectedMarker = null;
                    this.draggingNow = false;
                    this.map1.IsEnabled = true;
                }
            }
        }

        private void geoQ_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
        {

            if (this.lastRoute != null)
            {
                this.map1.RemoveRoute(this.lastRoute);
                this.lastRoute = null;
            }

            try
            {
                Route myRoute = e.Result;

                this.lastRoute = new MapRoute(myRoute);

                this.map1.AddRoute(this.lastRoute);
                this.map1.SetView(e.Result.BoundingBox);

                MessageBox.Show(
                    "Distance: " + (myRoute.LengthInMeters / 1000) + " km, Estimated traveltime: "
                    + myRoute.EstimatedDuration);
            }
            catch (TargetInvocationException)
            {
                
               Debug.WriteLine("wrong data to query");
            }
       
        }

        private void geoRev_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {


            string GeoStuff = string.Empty;
            return;
            if (e.Result.Count() > 0)
            {
                if (e.Result[0].Information.Address.Street.Length > 0)
                {
                    GeoStuff = GeoStuff + e.Result[0].Information.Address.Street;

                    if (e.Result[0].Information.Address.HouseNumber.Length > 0)
                    {
                        GeoStuff = GeoStuff + " " + e.Result[0].Information.Address.HouseNumber;
                    }
                }

                if (e.Result[0].Information.Address.City.Length > 0)
                {
                    if (GeoStuff.Length > 0)
                    {
                        GeoStuff = GeoStuff + ",";
                    }

                    GeoStuff = GeoStuff + " " + e.Result[0].Information.Address.City;

                    if (e.Result[0].Information.Address.Country.Length > 0)
                    {
                        GeoStuff = GeoStuff + " " + e.Result[0].Information.Address.Country;
                    }
                }
                else if (e.Result[0].Information.Address.Country.Length > 0)
                {
                    if (GeoStuff.Length > 0)
                    {
                        GeoStuff = GeoStuff + ",";
                    }
                    GeoStuff = GeoStuff + " " + e.Result[0].Information.Address.Country;
                }
            }
            if (this.DestinationRevGeoNow)
            {
                this.DestinationTitle.Text = GeoStuff;
            }
            else
            {
                this.OriginTitle.Text = GeoStuff;
            }
        }

        private void map1_ZoomLevelChanged(object sender, MapZoomLevelChangedEventArgs e)
        {
         
        
        }
        
        private void textt_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse clickedOne = sender as Ellipse;
            if (clickedOne != null && this.OriginMarker != null && this.DestinationMarker != null)
            {
                 if (this.DestinationMarker.Content == clickedOne)
                {
                    this.selectedMarker = this.DestinationMarker;
                    this.draggingNow = true;
                    this.map1.IsEnabled = false;
                }else if (this.OriginMarker.Content == clickedOne && dataContext.isInRealtyCreating)
                {
                    this.selectedMarker = this.OriginMarker;
                    this.draggingNow = true;
                    this.map1.IsEnabled = false;
                }
            }
        }

     
        #endregion

        private void Submit_Tap(object sender, GestureEventArgs e)
        {
            dataContext.CurrentRealty.MapPosX = this.OriginMarker.GeoCoordinate.Latitude;
            dataContext.CurrentRealty.MapPosY = this.OriginMarker.GeoCoordinate.Longitude;
            MessageBox.Show("accepted");

            this.NavigationService.Navigate(new Uri("/RealtyDetailsPage.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            this.map1.Heading = this.map1.Heading + 12;
        }

        private void MinHeading_Click(object sender, EventArgs e)
        {
            this.map1.Heading = this.map1.Heading - 12;
        }

        private void map1_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "87555bfe-031d-45a2-94ba-ec960fd90426";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "AqA8uwlJ0rHF34MD6sXxAgRhmZTuQwGtw-jR0ZN82R2-b4p3m8i-W8aDv-zjP4bo";
    
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
        
            this.NavigationService.Navigate(new Uri("/RealtyDetailsPage.xaml", UriKind.Relative));

        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (dataContext.isInRealtyCreating)
            {
                dataContext.CurrentRealty.MapPosX = this.OriginMarker.GeoCoordinate.Latitude;
                dataContext.CurrentRealty.MapPosY = this.OriginMarker.GeoCoordinate.Longitude;
                MessageBox.Show("accepted");
            }
          
            this.NavigationService.Navigate(new Uri("/RealtyDetailsPage.xaml", UriKind.Relative));
        }
    }
}