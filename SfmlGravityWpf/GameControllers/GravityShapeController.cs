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

        public GravityShapeController()
        {
            this.IsRunning = true;
            this.GravityShapes = new List<GravityShape>();
        }

        public bool DrawForceLines { get; set; }

        public bool DrawVelocityLines { get; set; }

        private IList<GravityShape> GravityShapes { get; set; }

        public bool IsRunning { get; private set; }

        public void AddGravityShape(GravityShape gs)
        {
            if (!this.GravityShapes.Contains(gs))
                this.GravityShapes.Add(gs);
        }

        public void DeleteShapes()
        {
            this.GravityShapes.Clear();
        }

        public void Draw(RenderTarget target)
        {
            if (this.DrawForceLines)
                this.DrawForceField(target);

            foreach(var gs in this.GravityShapes)
                target.Draw(gs.Shape);

            if (this.DrawVelocityLines)
                this.DrawVelocity(target);
        }

        private void DrawForceField(RenderTarget target)
        {
            const int spacing = 30;
            var xcount = target.Size.X / spacing;
            var ycount = target.Size.Y / spacing;
            
            for(int y = 0 ; y <= ycount ; y++)
            {
                for(int x = 0; x <= xcount; x++)
                {
                    var start = new Vector2f(x * spacing, y * spacing);
                    var line = new ForceLine(start, LineMass);
                    line.CalculateLine(this.GravityShapes);
                    target.Draw(line.Line, PrimitiveType.Lines);
                }
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

            //calculate force, apply force, move. seems like a good order to do things. Since A and B both exert
            //an equal amount of force on each other, if we do all three for A, then B, by the time B calculates
            //its force A will have moved, thus have a different force
            this.CalculateForce();

            foreach (var gs in this.GravityShapes)
            {
                gs.ApplyForce(dSeconds);
                gs.Move(dSeconds);
            }
        }

        private void CalculateForce()
        {
            var list = new List<GravityShape>(this.GravityShapes);
            foreach (var gs in this.GravityShapes)
            {
                gs.CalculateForce(list);
            }
        }

    }
}
