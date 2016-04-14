namespace SfmlGravityWpf.WpfModels
{
    using Code;
    using SFML.Graphics;
    using SFML.System;
    using SFML.Window;
    using GameControllers;

    /// <summary>
    /// Use this class for interacting with a <see cref="GravityShapeController"/> to get
    /// the benefits of XAML binding and whatnot.
    /// </summary>
    public class ObservableGravityShapeController : ObservableModel
    {
        private readonly GravityShapeController _gsController;
        private float _newRadius;//radius of new shapes
        private float _newShapeMass;

        public ObservableGravityShapeController(GravityShapeController gsController)
        {
            this._gsController = gsController;
            this.NewShapeMass = 1000;
            this.NewRadius = 3;
        }

        public float NewRadius
        {
            get { return this._newRadius; }
            set
            {
                if (this._newRadius == value)
                    return;
                this._newRadius = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The mass of a new shape
        /// </summary>
        public float NewShapeMass
        {
            get { return this._newShapeMass; }
            set
            {
                if (this._newShapeMass == value)
                    return;
                this._newShapeMass = value;
                this.NotifyPropertyChanged();
            }
        }

        public bool DrawGravityField
        {
            get { return this._gsController.DrawGravityField; }
            set
            {
                if (this._gsController.DrawGravityField == value)
                    return;
                this._gsController.DrawGravityField = value;
                this.NotifyPropertyChanged();

                if (!this._gsController.DrawGravityFieldAsLines && !this._gsController.DrawGravityFieldAsGradient)
                    this.DrawGravityFieldAsLines = true;
            }
        }

        public bool DrawGravityFieldAsLines
        {
            get { return this._gsController.DrawGravityFieldAsLines; }
            set
            {
                if (value == this._gsController.DrawGravityFieldAsLines)
                    return;
                this._gsController.DrawGravityFieldAsLines = value;
                this.NotifyPropertyChanged();

                if (value)
                    this.DrawGravityFieldAsGradient = false;
            }
        }

        public bool DrawGravityFieldAsGradient
        {
            get { return this._gsController.DrawGravityFieldAsGradient; }
            set
            {
                if (value == this._gsController.DrawGravityFieldAsGradient)
                    return;
                this._gsController.DrawGravityFieldAsGradient = value;
                this.NotifyPropertyChanged();

                if (value)
                    this.DrawGravityFieldAsLines = false;
            }
        }

        public bool DrawMotionTrails
        {
            get { return this._gsController.DrawMotionTrails; }
            set
            {
                if (value == this._gsController.DrawMotionTrails)
                    return;
                this._gsController.DrawMotionTrails = value;
                this.NotifyPropertyChanged();
            }
        }

        public bool DrawVelocityLines
        {
            get { return this._gsController.DrawVelocityLines; }
            set
            {
                if (this._gsController.DrawVelocityLines == value)
                    return;
                this._gsController.DrawVelocityLines = value;
                this.NotifyPropertyChanged();
            }
        }

        public bool IsRunning
        {
            get { return this._gsController.IsRunning; }
            set
            {
                if (this._gsController.IsRunning == value)
                    return;

                if (this._gsController.IsRunning)
                    this._gsController.Pause();
                else
                    this._gsController.Run();
                this.NotifyPropertyChanged();
            }
        }

        public int ShapeCount
        {
            get { return this._gsController.ShapeCount; }
        }

        public void ModifyNewShapeMass(string directions)
        {
            this.NewShapeMass = RegexValueChanger.Change(this.NewShapeMass, directions);
        }

        /// <summary>
        /// Event handler for the RenderWindow MouseButtonPressed event
        /// </summary>
        public void MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            this._gsController.StartNewShape(new Vector2f(e.X, e.Y));
        }

        /// <summary>
        /// Event handler for the RenderWindow MouseMoved event
        /// </summary>
        public void MouseMoved(object sender, MouseMoveEventArgs e)
        {
            this._gsController.UpdateEndPoint(new Vector2f(e.X, e.Y));
        }

        /// <summary>
        /// Event handler for the RenderWindow MouseButtonReleased event
        /// </summary>
        public void MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            this._gsController.FinishAddingShape(this.NewShapeMass, this.NewRadius);
            this.NotifyPropertyChanged(() => this.ShapeCount);
        }

        public void DeleteShapes()
        {
            this._gsController.DeleteShapes();
            this.NotifyPropertyChanged(() => this.ShapeCount);
        }

        public void TogglePause()
        {
            if (this._gsController.IsRunning)
                this._gsController.Pause();
            else
                this._gsController.Run();
            this.NotifyPropertyChanged(() => this.IsRunning);
        }

        public void Draw(RenderWindow target)
        {
            target.Clear(Color.Black);

            this._gsController.Update();
            this._gsController.Draw(target);

            target.Display();
        }

    }
}
