namespace SfmlGravityWpf.Windows
{
    using System.Windows.Controls;
    using Code.Extensions;
    using GameControllers;
    using SFML.Graphics;
    using SFML.Window;
    using System;
    using System.Windows;
    using System.Windows.Threading;
    using WpfModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private RenderWindow _renderWindow;
        private readonly ObservableGravityShapeController _observableGsc;

        public MainWindow()
        {
            InitializeComponent();

            this._observableGsc = new ObservableGravityShapeController(new GravityShapeController());
            this.BindDataContext(this._observableGsc);
            this.CreateRenderWindow();

            var timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60) };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void CreateRenderWindow()
        {
            if (this._renderWindow != null)
            {
                this._renderWindow.SetActive(false);
                this._renderWindow.Dispose();
            }

            var context = new ContextSettings { DepthBits = 24 };
            this._renderWindow = new RenderWindow(this.DrawSurface.Handle, context);
            this._renderWindow.MouseButtonPressed += this._observableGsc.MouseButtonPressed;
            this._renderWindow.MouseButtonReleased += this._observableGsc.MouseButtonReleased;
            this._renderWindow.MouseMoved += this._observableGsc.MouseMoved;
            this._renderWindow.SetActive(true);
        }
        
        private void DrawSurface_SizeChanged(object sender, EventArgs e)
        {
            this.CreateRenderWindow();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //System.Windows.Forms.Application.DoEvents(); //doesn't seem to help.

            this._renderWindow.DispatchEvents();
            this._observableGsc.Draw(this._renderWindow);
        }

        private void DeleteCircles_Click(object sender, RoutedEventArgs e)
        {
            this._observableGsc.DeleteShapes();
        }

        private void PauseUnpause_Click(object sender, RoutedEventArgs e)
        {
            this._observableGsc.TogglePause();
        }

        private void QuickMassButton_OnClick(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var directions = (string)button.Content;
            this._observableGsc.ModifyNewShapeMass(directions);
        }
    }
}
