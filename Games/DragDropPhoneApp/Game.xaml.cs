namespace DragDropPhoneApp
{
    #region Using Directives

    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using System.Windows.Threading;

    using Coding4Fun.Toolkit.Controls;

    using DragDropPhoneApp.Context;
    using DragDropPhoneApp.Model;

    using Microsoft.Phone.Controls;

    using Newtonsoft.Json;

    #endregion

    public partial class Game : PhoneApplicationPage
    {
        #region Fields

        private double cWidth = 480;

        private double cellWidth = 100;

        private Cell[][] cells = new Cell[4][];

        private int secondsElapsed;
        private int[] usedNumbers = new int[16];
        private DispatcherTimer timer;
        private int usedNumbersCount;
        private GameContext dataContext = new GameContext();
        #endregion

        #region Constructors and Destructors

        public Game()
        {
            this.InitializeComponent();
            {
                // this.MainCanvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                // this.MainCanvas.Arrange(new Rect(0, 0, this.MainCanvas.ActualHeight, this.MainCanvas.ActualWidth));
                this.Initialize();
                this.timer = new DispatcherTimer();
                this.timer.Interval = TimeSpan.FromMilliseconds(1000);
                this.timer.Tick += this.OnTimerTick;
                this.timer.Start();
            }
        }

        #endregion

        #region Public Methods and Operators
        public void SendPost(RankTableEntry gizmo)
        {



            return; //Nah!
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


            string uri = "http://localhost:4232/api/values/";

            //S4: Ehencode the stream into string format
            var data1 = Encoding.UTF8.GetString(jsonDataToPost, 0, jsonDataToPost.Length);
            //S5: Save Data in the QuoteMaster Table
            WebClient webClientQuote = new WebClient();
            webClientQuote.Headers["content-type"] = "application/json";
            webClientQuote.UploadStringAsync(new Uri(uri), "POST", data1);
        }
        private void OnTimerTick(object sender, EventArgs e)
        {
            secondsElapsed++;
            TimerTextBox.Text = "Timer : " + secondsElapsed + " seconds";
        }

        public void CreateButtonsWithNumbers(Cell cell)
        {
            Button btn = new Button
                             {
                                 Width = this.cellWidth,
                                 DataContext = cell,
                                 Height = this.cellWidth,
                                 Content = this.GetNextRandomNumber()
                             };
            cell.Number = (int)btn.Content;
            btn.Tap += this.cellClick;
            Canvas.SetLeft(btn, cell.PositionOnCanvas.X);
            Canvas.SetTop(btn, cell.PositionOnCanvas.Y);

            cell.Element = btn;
            this.MainCanvas.Children.Add(btn);
        }

        #endregion

        #region Methods
        private Storyboard CreateStoryboard(UIElement element, Point pTo)
        {
            double to = pTo.Y;
            double toLeft = pTo.X;
            Storyboard result = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation();
            animation.To = to;
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Canvas.Top)"));
            Storyboard.SetTarget(animation, element);
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(800));
            DoubleAnimation animationLeft = new DoubleAnimation();
            animationLeft.To = toLeft;
            Storyboard.SetTargetProperty(animationLeft, new PropertyPath("(Canvas.Left)"));
            Storyboard.SetTarget(animationLeft, element);

            result.Children.Add(animation);
            result.Children.Add(animationLeft);

            return result;
        }

        private int fake = 0;
        private int GetNextRandomNumber()
        {
            fake++;
            return fake;
            Random r = new Random();
            int num = 0;
            bool unique;
            do
            {
                num = r.Next(0, 16);

                unique = this.usedNumbers.Contains(num);
                if (this.usedNumbersCount == 15)
                {
                    break;
                }
            }
            while (unique);
            this.usedNumbers[this.usedNumbersCount] = num;
            this.usedNumbersCount++;
            return num;
        }

        private void Initialize()
        {
            this.cellWidth = this.cWidth / 4;
            for (int i1 = 0; i1 < 4; i1++)
            {
                this.cells[i1] = new Cell[4];
            }

            int j = 0;
            int k = 0;
            for (double i = 0; i < this.cWidth; i += this.cellWidth)
            {
                for (double w = 0; w < this.cWidth; w += this.cellWidth)
                {
                    this.cells[j][k] = new Cell
                                           {
                                               PositionOnCanvas = new Point(i, w),
                                               PositionInMatrix = new Point(j, k)
                                           };
                    k++;
                }

                k = 0;
                j++;
            }
            for (int i = 0; i < 4; i++)
            {
                for (int j1 = 0; j1 < 4; j1++)
                {
                    this.CreateButtonsWithNumbers(this.cells[j1][i]);
                }
            }

            this.MainCanvas.Children.Remove(this.cells[3][3].Element);
            this.cells[3][3].IsEmptySpace = true;
        }

        private bool CheckWin()
        {
            int sum = 0;
            bool win = true;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (this.cells[j][i].Number != sum + 1)
                    {
                        win = false;
                        return win;

                    }
                    sum++;
                }
            }
            return true;
        }
        private bool Move(Cell to, Cell from)
        {
            bool moved = false;
            if (to.IsEmptySpace)
            {
                var story = this.CreateStoryboard(from.Element, to.PositionOnCanvas);

                var temp = to.Element;
                to.Element = from.Element;
                from.Element = temp;
                from.IsEmptySpace = true;
                to.IsEmptySpace = false;
                var tempNo = to.Number;
                to.Number = from.Number;
                from.Number = tempNo;
                story.Begin();
                moved = true;
            }

            return moved;
        }
       
        private void congratulateWinner()
        {
            MessageBox.Show("Winner, elapsed time = " + secondsElapsed);
            InputPrompt s = new InputPrompt();
            s.Show();
            s.Completed += (o, args) =>
            {
                this.SendPost(new RankTableEntry
                {
                    UserName = args.Result,
                    TimePassed = secondsElapsed.ToString()
                });
               
                MessageBox.Show("Saved");
            };

            this.secondsElapsed = 0;
            this.NavigationService.Navigate(new Uri("/Game.xaml?Refresh=true", UriKind.Relative));
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void cellClick(object sender, GestureEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null)
            {
                return;
            }

            var cell = btn.DataContext as Cell;
            if (cell == null)
            {
                return;
            }

            var cellPosY = (int)cell.PositionInMatrix.Y;
            var cellPosX = (int)cell.PositionInMatrix.X;

            var left = (int)cell.PositionInMatrix.X - 1;
            var right = (int)cell.PositionInMatrix.X + 1;
            var top = (int)cell.PositionInMatrix.Y + 1;
            var bottom = (int)cell.PositionInMatrix.Y - 1;

            if (left >= 0)
            {
                var CellToMove = this.cells[left][cellPosY];

                if (this.Move(CellToMove, cell))
                {
                    btn.DataContext = CellToMove;
                    if (CheckWin())
                    {
                        congratulateWinner();

                    }
                    return;
                }
            }

            if (right < 4)
            {
                var CellToMove = this.cells[right][cellPosY];
                if (this.Move(CellToMove, cell))
                {
                    btn.DataContext = CellToMove;
                    if (CheckWin())
                    {

                        congratulateWinner();
                    }
                    return;
                }
            }

            if (bottom >= 0)
            {
                var CellToMove = this.cells[cellPosX][bottom];
                if (this.Move(CellToMove, cell))
                {
                    btn.DataContext = CellToMove;
                    if (CheckWin())
                    {

                        congratulateWinner();
                    }
                    return;
                }
            }

            if (top < 4)
            {
                var CellToMove = this.cells[cellPosX][top];
                if (this.Move(CellToMove, cell))
                {
                    btn.DataContext = CellToMove;
                }
                if (CheckWin())
                {

                    congratulateWinner();
                }
            }

        }

        #endregion
    }

    public class Cell
    {
        #region Fields

        public UIElement Element;

        public int Number;
        public bool IsEmptySpace;

        public Point PositionInMatrix;

        public Point PositionOnCanvas;

        #endregion
    }
}