namespace DragDropPhoneApp
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using System.Windows.Threading;

    using Microsoft.Phone.Controls;

    #endregion

    public partial class MainPage : PhoneApplicationPage
    {
        #region Constants

        private const double SPEED_FACTOR = 60;

        #endregion

        #region Static Fields

        private static bool FirstTimeLoad = true;
        static Random rnd = new Random();
        #endregion

        #region Fields

        private FrameworkElement ElemToMove = null;

        private double ElemVelX;

        private double ElemVelY;

        private int blueNum;
        private int greenNum;
        private int redNum;

        private List<FrameworkElement> elementsToDrop = new List<FrameworkElement>();

        private DispatcherTimer timer;

        private static List<SolidColorBrush> colors = new List<SolidColorBrush>
                                                          {
                                                              new SolidColorBrush(Colors.Red),
                                                                new SolidColorBrush(Colors.Blue),
                                                                 
                                                               new SolidColorBrush(Colors.Green)
                                                          };
        #endregion

        #region Constructors and Destructors

        public MainPage()
        {
            this.InitializeComponent();
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromMilliseconds(35);
            this.timer.Tick += this.OnTimerTick;
            this.RedNum.DataContext = redNum;
            this.GreenNum.DataContext = greenNum;
        }

        #endregion

        #region Methods

        private void AddCircleOnCanvas(GestureEventArgs e)
        {
            int rndColorNum = rnd.Next(colors.Count);
            var color = colors.ElementAt(rndColorNum);
            if (color.Color == Colors.Green)
            {
                greenNum++;
                GreenNum.Text = greenNum.ToString();
            }
            else if (color.Color == Colors.Red)
            {
                redNum++;
                RedNum.Text = redNum.ToString();
            }
            else if (color.Color == Colors.Blue)
            {
                blueNum++;
                BlueNum.Text = blueNum.ToString();
            }




            var el = new Ellipse { Width = 70, Height = 70, Fill = color };

            el.ManipulationDelta += OnManipulationDelta;
            el.ManipulationCompleted += OnManipulationCompleted;
            Canvas.SetLeft(el, e.GetPosition(this.MainCanvas).X - 35);
            Canvas.SetTop(el, e.GetPosition(this.MainCanvas).Y - 35);
            this.MainCanvas.Children.Add(el);
        }

        private void AddDragableItemOnCanvas()
        {
            Image appleImage = new Image { Source = new BitmapImage(new Uri("/images/Apple.png", UriKind.Relative)) };

            Canvas.SetTop(appleImage, 0);
            Canvas.SetLeft(appleImage, 240);

            this.MainCanvas.Children.Add(appleImage);

            appleImage.ManipulationDelta += OnManipulationDelta;
            appleImage.ManipulationCompleted += OnManipulationCompleted;
        }

        private void MainCanvas_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void MainCanvas_Tap(object sender, GestureEventArgs e)
        {
            this.AddCircleOnCanvas(e);
        }

        private void OnManipulationCompleted(object sender, ManipulationCompletedEventArgs args)
        {
            FrameworkElement Elem = sender as FrameworkElement;
            this.elementsToDrop.Add(Elem);

            this.timer.Start();
        }

        private void OnManipulationDelta(object sender, ManipulationDeltaEventArgs args)
        {
            FrameworkElement Elem = sender as FrameworkElement;

            double Left = Canvas.GetLeft(Elem);
            double Top = Canvas.GetTop(Elem);

            Left += args.DeltaManipulation.Translation.X;
            Top += args.DeltaManipulation.Translation.Y;

            if (Left < 0)
            {
                Left = 0;
            }
            else if (Left > (this.LayoutRoot.ActualWidth - Elem.ActualWidth))
            {
                Left = this.LayoutRoot.ActualWidth - Elem.ActualWidth;
            }

            if (Top < 0)
            {
                Top = 0;
            }
            else if (Top > (this.LayoutRoot.ActualHeight - Elem.ActualHeight))
            {
                Top = this.LayoutRoot.ActualHeight - Elem.ActualHeight;
            }

            Canvas.SetLeft(Elem, Left);
            Canvas.SetTop(Elem, Top);
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            double Top;
            List<FrameworkElement> elemsToRemove = new List<FrameworkElement>();
            foreach (var elem in this.elementsToDrop)
            {
                Top = Canvas.GetTop(elem);
                double delta = 1000 / Top;
                if (Top > this.MainCanvas.ActualHeight - 75)
                {
                    elemsToRemove.Add(elem);
                    continue;
                }

                if (delta == double.PositiveInfinity)
                {
                    continue;
                }

                Top += delta;
                Canvas.SetTop(elem, Top);
            }

            foreach (var elem in elemsToRemove)
            {
                this.elementsToDrop.Remove(elem);
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (FirstTimeLoad)
            {
                FirstTimeLoad = false;
                this.NavigationService.Navigate(new Uri("/Maps.xaml", UriKind.Relative));
            }
        }

        private void PhoneApplicationPage_Tap(object sender, GestureEventArgs e)
        {
        }

        #endregion
    }
}