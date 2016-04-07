namespace SfmlGravityWpf.GameControllers
{
    using System.Collections.Generic;
    using GameModels;
    using SFML.System;
    using SFML.Graphics;

    public class GravityShapeController
    {
        private readonly Clock _timer = new Clock();
        private const float LineMass = 10;
        private float _lastTick;
        private bool _addingShape;
        private Vector2f _startPoint; //new shape's spawn point, start of initial vector line
        private Vector2f _endPoint; //used to calculate new shape's initial velocity, end of init velocity line

        public GravityShapeController()
        {
            this.IsRunning = true;
            this.GravityShapes = new List<GravityShape>();
        }

        public void StartNewShape(Vector2f startPoint)
        {
            if (this._addingShape)
                return;
            this._addingShape = true;
            this._startPoint = startPoint;
            this._endPoint = startPoint; //so line is 0 length at start
        }

        public void UpdateEndPoint(Vector2f endPoint)
        {
            if (!this._addingShape)
                return;
            this._endPoint = endPoint;
        }

        public void FinishAddingShape(float mass, float radius)
        {
            var vel = this._endPoint - this._startPoint;
            var circle = new CircleShape(radius) { FillColor = Color.Cyan, Position = this._startPoint};
            var gs = new GravityShape(circle, mass) { Velocity = vel };
            this.GravityShapes.Add(gs);
            this._addingShape = false;
        }

        public bool DrawGravityField { get; set; }

        public bool DrawGravityFieldAsLines { get; set; }

        public bool DrawGravityFieldAsGradient { get; set; }

        public bool DrawMotionTrails { get; set; }

        public bool DrawVelocityLines { get; set; }

        private IList<GravityShape> GravityShapes { get; set; }

        public bool IsRunning { get; private set; }

        public int ShapeCount
        {
            get { return this.GravityShapes.Count; }
        }

        public void DeleteShapes()
        {
            this.GravityShapes.Clear();
        }

        public void Draw(RenderTarget target)
        {
            if (this.DrawGravityField)
                this.DrawForceField(target);

            if (this.DrawMotionTrails)
            {
                foreach (var gs in this.GravityShapes)
                    gs.MotionTrail.Draw(target);               
            }

            foreach (var gs in this.GravityShapes)
                target.Draw(gs.Shape);

            if (this.DrawVelocityLines)
                this.DrawVelocity(target);

            if (this._addingShape)
            {
                var startVertex = new Vertex(this._startPoint, Color.Magenta);
                var endVertex = new Vertex(this._endPoint, Color.Green);
                var line = new[] { startVertex, endVertex };
                target.Draw(line, PrimitiveType.Lines);
            }
        }

        private void DrawForceField(RenderTarget target)
        {
            if (this.DrawGravityFieldAsLines)
            {
                const int spacing = 20;
                for (int y = 0; y <= target.Size.Y / spacing; y++)
                {
                    for (int x = 0; x <= target.Size.X / spacing; x++)
                    {
                        var start = new Vector2f(x * spacing, y * spacing);
                        var line = new ForceLine(start, LineMass, spacing);
                        line.CalculateLine(this.GravityShapes);
                        target.Draw(line.Line, PrimitiveType.Lines);
                    }
                }
            }

            if (this.DrawGravityFieldAsGradient)
            {
                const int spacing = 10;
                int width = (int)target.Size.X / spacing;
                int height = (int)target.Size.Y / spacing;
                var field = new GravityGradientField(spacing, width, height);
                field.Calculate(this.GravityShapes);
                field.Draw(target);
            }
        }

        private void DrawVelocity(RenderTarget target)
        {
            var purple = new Color(153, 0, 255);
            foreach(var gs in this.GravityShapes)
            {
                var start = new Vertex(gs.GlobalCenterOfMass, Color.White);
                var end = new Vertex(gs.GlobalCenterOfMass + gs.Velocity, purple);
                var line = new[] { start, end };
                target.Draw(line, PrimitiveType.Lines);
            }
        }
 
        public void Pause()
        {
            this.IsRunning = false;
        }

        public void Run()
        {
            this.IsRunning = true;
        }

        /// <summary>
        /// Updates the position/velocity of the GravityShapes since the last time Update() was called.
        /// </summary>
        public void Update()
        {
            var seconds = this._timer.ElapsedTime.AsSeconds();
            var dSeconds = seconds - this._lastTick;
            this._lastTick = seconds;
            
            if (!this.IsRunning)
                return;

            //we want to know the force on all shapes before we move any of them.
            foreach (var gs in this.GravityShapes)
                gs.CalculateForce(this.GravityShapes);

            foreach (var gs in this.GravityShapes)
            {
                gs.ApplyForce(dSeconds);
                gs.Move(dSeconds);
                gs.MotionTrail.AddLocation(gs.GlobalCenterOfMass);
            }

        }
    }
}
