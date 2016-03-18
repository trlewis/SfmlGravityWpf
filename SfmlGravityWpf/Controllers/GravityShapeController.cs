using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfmlGravityWpf.Controllers
{
    using SfmlGravityWpf.GameModels;
    using SFML.System;
    using SFML.Graphics;

    public class GravityShapeController
    {
        private Clock _timer = new Clock();
        private const float LineMass = 10;
        private float _lastTick = 0;

        public GravityShapeController()
        {
            this.IsRunning = true;
            this.GravityShapes = new List<GravityShape>();
            var timer = new SFML.System.Clock();
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

            this.DrawShapes(target);

            if (this.DrawVelocityLines)
                this.DrawVelocity(target);
        }

        private void DrawForceField(RenderTarget target)
        {
            int spacing = 30;
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
                var line = new Vertex[] { start, end };
                target.Draw(line, PrimitiveType.Lines);
            }
        }

        private void DrawShapes(RenderTarget target)
        {
            foreach(var gs in this.GravityShapes)
            {
                target.Draw(gs.Shape);
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

            this.CalculateForce();
            this.ApplyForce(dSeconds);
            this.Move(dSeconds);
        }

        private void CalculateForce()
        {
            var list = new List<GravityShape>(this.GravityShapes);
            foreach (var gs in this.GravityShapes)
            {
                gs.CalculateForce(list);
            }
        }

        private void ApplyForce(float dTime)
        {
            foreach (var gs in this.GravityShapes)
            {
                gs.ApplyForce(dTime);
            }
        }

        private void Move(float dTime)
        {
            foreach (var gs in this.GravityShapes)
            {
                gs.Move(dTime);
            }
        }
    }
}
