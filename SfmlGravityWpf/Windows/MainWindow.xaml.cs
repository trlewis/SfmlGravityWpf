namespace SfmlGravityWpf.Windows
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using SFML.Graphics;
    using SFML.System;
    using SFML.Window;
    using System.Windows.Threading;
    using GameControllers;
    using GameModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private RenderWindow _renderWindow;
        private float _circleMass = 1000;

        //for creating new shapes
        private Vector2f _mouseStartPos;
        private Vector2f _mouseEndPos;
        private bool _creatingCicle;

        private readonly GravityShapeController _shapeController = new GravityShapeController();

        public MainWindow()
        {
            InitializeComponent();

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
            this._renderWindow.MouseButtonPressed += RenderWindow_MouseButtonPressed;
            this._renderWindow.MouseButtonReleased += RenderWindow_MouseButtonReleased;
            this._renderWindow.MouseMoved += RenderWindow_MouseMoved;
            this._renderWindow.SetActive(true);
        }

        void RenderWindow_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            if (this._creatingCicle)
                this._mouseEndPos = new Vector2f(e.X, e.Y);
        }

        void RenderWindow_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            var endPos = new Vector2f(e.X, e.Y);
            var vel = endPos - this._mouseStartPos;
            var newShape = new CircleShape(5) { FillColor = Color.Cyan, Position = this._mouseStartPos };
            var gs = new GravityShape(newShape, this._circleMass) { Velocity = vel };
            this._shapeController.AddGravityShape(gs);

            this._creatingCicle = false;
        }

        private void RenderWindow_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            this._mouseStartPos = new Vector2f(e.X, e.Y);
            this._mouseEndPos = new Vector2f(e.X, e.Y);
            this._creatingCicle = true;
        }

        private void DrawSurface_SizeChanged(object sender, EventArgs e)
        {
            this.CreateRenderWindow();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //System.Windows.Forms.Application.DoEvents(); //doesn't seem to help.
            this._renderWindow.DispatchEvents();

            this._renderWindow.Clear(Color.Black);

            this._shapeController.Update();
            this._shapeController.Draw(this._renderWindow);

            if(this._creatingCicle)
            {
                var startVertex = new Vertex(this._mouseStartPos, Color.Magenta);
                var endVertex = new Vertex(this._mouseEndPos, Color.Green);
                var line = new[] { startVertex, endVertex };
                this._renderWindow.Draw(line, PrimitiveType.Lines);
            }

            this._renderWindow.Display();
        }

        private void DrawForce_Checked(object sender, RoutedEventArgs e)
        {
            this._shapeController.DrawForceLines = true;
        }

        private void DrawForce_Unchecked(object sender, RoutedEventArgs e)
        {
            this._shapeController.DrawForceLines = false;
        }

        private void DeleteCircles_Click(object sender, RoutedEventArgs e)
        {
            this._shapeController.DeleteShapes();
        }

        private void PauseUnpause_Click(object sender, RoutedEventArgs e)
        {
            if (this._shapeController.IsRunning)
                this._shapeController.Pause();
            else
                this._shapeController.Run();

            var button = sender as Button;
            if(button != null)
                button.Content = this._shapeController.IsRunning ? "Pause" : "Unpause";
        }

        private void MassTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            try
            {
                var newMass = float.Parse(textBox.Text);
                this._circleMass = newMass;
            }
            catch (Exception) { }
        }

        private void DrawVelocity_Checked(object sender, RoutedEventArgs e)
        {
            this._shapeController.DrawVelocityLines = true;
        }

        private void DrawVelocity_Unchecked(object sender, RoutedEventArgs e)
        {
            this._shapeController.DrawVelocityLines = false;
        }
    }
}
