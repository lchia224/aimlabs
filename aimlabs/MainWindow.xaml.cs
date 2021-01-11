﻿using System;
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
        }

        private void DummyMoveTimer_Tick(object sender, EventArgs e)
        {
        }

        private void ShowDummies(int x, int y, int skin, string tag)
        {
            
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
        }
    }
}
