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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ImageBrush backgroundImage = new ImageBrush(); //placeholder for the background
        ImageBrush ghostSprite = new ImageBrush(); //placeholder for the ghost enemy
        ImageBrush aimImage = new ImageBrush(); //placeholder for the scope

        DispatcherTimer DummyMoveTimer = new DispatcherTimer(); //Timer to move dummy images
        DispatcherTimer showGhostTimer = new DispatcherTimer(); //Timer to move ghost images

        int topCount = 0; //top location counter
        int bottomCount = 0; //bottom location counter

        int score; //keep score
        int miss; //keep misses

        List<int> topLocations;
        List<int> bottomLocations;

        List<Rectangle> remover = new List<Rectangle>(); //garbage collector for the game

        Random rand = new Random(); //random number generator

        public MainWindow()
        {
            InitializeComponent();

            this.Cursor = Cursors.None; //hides cursor

            //setting up background
            backgroundImage.ImageSource = new BitmapImage(new Uri("/images/background.png"));
            MyCanvas.Background = backgroundImage;

            //setting up scope
            scopeImage.Source = new BitmapImage(new Uri("/images/sniper-aim.png"));

            //setting up ghost image
            ghostSprite.ImageSource = new BitmapImage(new Uri("/images/ghost.png"));

            //setting up dummy moving timer
            DummyMoveTimer.Tick += DummyMoveTimer_Tick;
            DummyMoveTimer.Interval = TimeSpan.FromMilliseconds(rand.Next(800, 2000));
            DummyMoveTimer.Start();

            //setting up ghost moving timer
            showGhostTimer.Tick += ShowGhostTimer_Tick;
            showGhostTimer.Interval = TimeSpan.FromMilliseconds(20);
            showGhostTimer.Start();

            //add locations to the list in pixels
            topLocations = new List<int> { 23, 270, 540, 23, 270, 540 };
            bottomLocations = new List<int> { 138, 128, 678, 138, 128, 678 };
        }

        private void ShowGhostTimer_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DummyMoveTimer_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ShowDummies(int x, int y, int skin, string tag)
        {

        }

        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void ShootDummy(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
