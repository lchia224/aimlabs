using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace aimlabs
{
    public partial class MainWindow : Window
    {
        ImageBrush backgroundImage = new ImageBrush(); // placeholder for the background
        ImageBrush ghostSprite = new ImageBrush(); // placeholder for the ghost enemy
        ImageBrush aimImage = new ImageBrush(); // placeholder for the scope

        DispatcherTimer DummyMoveTimer = new DispatcherTimer(); // Timer to move dummy images
        DispatcherTimer showGhostTimer = new DispatcherTimer(); // Timer to move ghost images

        int topCount = 0; // top location counter
        int bottomCount = 0; // bottom location counter

        int score; // keep score
        int miss; // keep misses

        List<int> topLocations;
        List<int> bottomLocations;

        List<Rectangle> remover = new List<Rectangle>(); // garbage collector for the game

        Random rand = new Random(); // random number generator

        Result result = new Result();

        public MainWindow()
        {
            InitializeComponent();

            this.Cursor = Cursors.None; // hides cursor

            // setting up background
            backgroundImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/background.png"));
            MyCanvas.Background = backgroundImage;

            // setting up scope
            scopeImage.Source = new BitmapImage(new Uri("pack://application:,,,/images/sniper-aim.png"));

            // setting up ghost image
            ghostSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/ghost.png"));

            // setting up dummy moving timer
            DummyMoveTimer.Tick += DummyMoveTimer_Tick;
            DummyMoveTimer.Interval = TimeSpan.FromMilliseconds(rand.Next(800, 2000));
            DummyMoveTimer.Start();

            // setting up ghost moving timer
            showGhostTimer.Tick += ShowGhostTimer_Tick;
            showGhostTimer.Interval = TimeSpan.FromMilliseconds(20);
            showGhostTimer.Start();

            // add locations to the list in pixels
            topLocations = new List<int> { 23, 270, 540, 23, 270, 540 };
            bottomLocations = new List<int> { 138, 128, 678, 138, 128, 678 };
        }

        private void ShowGhostTimer_Tick(object sender, EventArgs e)
        {
            scoreText.Content = "Scored: " + score; // links score to the score text
            missText.Content = "Missed: " + miss; // links miss to missed text

            // runs a foreach loop to check if there are any rectangles present in the canvas
            foreach(var x in MyCanvas.Children.OfType<Rectangle>())
            {
                // checks for rectangles with ghost tag
                if((string)x.Tag == "ghost")
                {
                    // animate ghost to top of the canvas
                    Canvas.SetTop(x, (Canvas.GetTop(x) - 5));

                    // if the ghost animates 180 pixels from its current position
                    // add to remover list
                    if(Canvas.GetTop(x) < -180)
                    {
                        remover.Add(x);
                    }
                }
            }

            // loops through rectangle list and removes each entity inside the list
            foreach(Rectangle y in remover)
            {
                MyCanvas.Children.Remove(y);
            }
        }

        private void DummyMoveTimer_Tick(object sender, EventArgs e)
        {
            remover.Clear(); // clears the garbage collector

            // foreach loop to check if any rectangles are present in the canvas
            // if so, remove the rectangles
            foreach(var i in MyCanvas.Children.OfType<Rectangle>())
            {
                if((string)i.Tag == "top") 
                {
                    remover.Add(i);
                    topCount--;
                    
                    miss++;
                }
                else if((string)i.Tag == "bottom")
                {
                    remover.Add(i);
                    bottomCount--;

                    miss++;
                }
            }

            // if dummy on top row is less than 3
            // add a dummy
            if(topCount < 3)
            {
                ShowDummies(topLocations[rand.Next(0, 5)], 35, rand.Next(1, 4), "top");
                topCount++;
            }
            // if dummy on bottom row is less than 3
            // add a dummy
            if(bottomCount < 3)
            {
                ShowDummies(bottomLocations[rand.Next(0, 5)], 230, rand.Next(1, 4), "bottom");
                bottomCount++;
            }
        }

        private void ShowDummies(int x, int y, int skin, string tag)
        {
            // creates a new imagebrush for the dummy skin
            ImageBrush dummyColour = new ImageBrush();

            // switch function for dictating the colour of the dummy
            // takes an int from the parameter and uses it for the switch function
            switch (skin)
            {
                case 1:
                    dummyColour.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/dummy01.png"));
                    break;
                case 2:
                    dummyColour.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/dummy02.png"));
                    break;
                case 3:
                    dummyColour.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/dummy03.png"));
                    break;
                case 4:
                    dummyColour.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/dummy04.png"));
                    break;
                default:
                    dummyColour.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/dummy01.png"));
                    break;
            }

            // creates a new rectangle for the dummy
            Rectangle newRect = new Rectangle
            {
                Tag = tag,
                Width = 80,
                Height = 155,
                Fill = dummyColour
            };

            Canvas.SetTop(newRect, y); // positions the rectangle y coord
            Canvas.SetLeft(newRect, x); // positions the rectangle x coord

            MyCanvas.Children.Add(newRect);
        }

        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            // Gets the x and y coords of the mouse cursor
            System.Windows.Point position = e.GetPosition(this);
            double pX = position.X;
            double pY = position.Y;

            // Sets the height and width of the scope to the mouse coords
            Canvas.SetLeft(scopeImage, pX - (scopeImage.Width / 2));
            Canvas.SetTop(scopeImage, pY - (scopeImage.Height / 2));
        }

        private void ShootDummy(object sender, MouseButtonEventArgs e)
        {
            // if click source is rectangle then we will create a new rectangle
            // and link it to the rectangle that will trigger the click event
            if(e.OriginalSource is Rectangle)
            {
                Rectangle activeRec = (Rectangle)e.OriginalSource; // create link between the sender rectangle

                MyCanvas.Children.Remove(activeRec); // finds rectangle and removes from canvas

                score++; // adds 1 to the score

                if((string)activeRec.Tag == "top")
                {
                    // if the rectangle tag was top
                    // deduct one from the top count integer
                    topCount--;
                }
                else if((string)activeRec.Tag == "bottom")
                {
                    // if the rectangle tag was bottom
                    // deduct one from the bottom count integer
                    bottomCount--;
                }

                createGhost();
                ShowResult();
            }
            else if(e.OriginalSource is Canvas)
            {
                remover.Clear();

                foreach(Rectangle i in MyCanvas.Children.OfType<Rectangle>())
                {                   
                    if ((string)i.Tag == "top" || (string)i.Tag == "bottom")
                    {
                        remover.Add(i);
                        topCount--;
                        bottomCount--;
                        miss++;
                    }
                }
            }
        }

        private void createGhost()
        {
            // initializing ghost rectangle
            Rectangle ghostRec = new Rectangle
            {
                Width = 60,
                Height = 100,
                Fill = ghostSprite,
                Tag = "ghost"
            };

            // setting where the ghost will appear on the mouse click location
            Canvas.SetLeft(ghostRec, Mouse.GetPosition(MyCanvas).X - 40); // sets left position of rectangle to mouse X axis
            Canvas.SetTop(ghostRec, Mouse.GetPosition(MyCanvas).Y - 60); // set top position of rectangle to mouse Y axis

            MyCanvas.Children.Add(ghostRec); // adds the new rectangle to the canvas
        }

        private void ShowResult()
        {
            Result result = new Result();

            if(score == 5)
            {
                result.ShowDialog();
            }
        }
    }
}
