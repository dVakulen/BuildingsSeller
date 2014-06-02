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
    using System.Reflection;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

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
        }

        #endregion

        #region Methods

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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (sender == this.Hplus)
            {
                this.map1.Heading = this.map1.Heading + 12;
            }
            else if (sender == this.Hmins)
            {
                this.map1.Heading = this.map1.Heading - 12;
            }
            else if (sender == this.TrMod)
            {
                if (this.travelMode == TravelMode.Driving)
                {
                    this.travelMode = TravelMode.Walking;
                    this.TrMod.Content = "Walk";
                }
                else
                {
                    this.travelMode = TravelMode.Driving;
                    this.TrMod.Content = "Drive";
                }
            }
            else if (sender == this.GetRouteBtn)
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
        }

        private MapOverlay MakeDotMarker(GeoCoordinate location, bool isDestination)
        {
            MapOverlay Marker = new MapOverlay();

            Marker.GeoCoordinate = location;

            Ellipse Circhegraphic = new Ellipse();
            if (isDestination)
            {
                Circhegraphic.Fill = new SolidColorBrush(Colors.Green);
                Circhegraphic.Stroke = new SolidColorBrush(Colors.Orange);
            }
            else
            {
                Circhegraphic.Fill = new SolidColorBrush(Colors.Yellow);
                Circhegraphic.Stroke = new SolidColorBrush(Colors.Red);
            }

            Circhegraphic.StrokeThickness = 20;
            Circhegraphic.Opacity = 0.8;
            Circhegraphic.Height = 50;
            Circhegraphic.Width = 50;

            Marker.Content = Circhegraphic;
            Marker.PositionOrigin = new Point(0.5, 0.5);
            Circhegraphic.MouseLeftButtonDown += this.textt_MouseLeftButtonDown;

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
        {
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
            this.zoomSlider.Value = this.map1.ZoomLevel;
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
                }
            }
        }

        private void zoomSlider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.zoomSlider != null)
            {
                this.map1.ZoomLevel = this.zoomSlider.Value;
            }
        }

        #endregion
    }
}