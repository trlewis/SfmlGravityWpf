namespace SfmlGravityWpf.GameModels
{
    using System;
    using System.Collections.Generic;
    using Code.Extensions;
    using SFML.Graphics;
    using SFML.System;

    public class ForceLine
    {
        private readonly Vector2f _startPos;
        private readonly float _mass;
        private readonly float _maxLength;

        public ForceLine(Vector2f start, float mass, float maxLength)
        {
            this._startPos = start;
            this._mass = mass;
            this._maxLength = maxLength;
            this.Line = new[] { new Vertex(start, Color.White), new Vertex(start, Color.Red) };
        }

        public Vertex[] Line { get; private set; }

        public void CalculateLine(IEnumerable<GravityObject> objects)
        {
            //just going to use 1 second as the time.
            var totalForce = new Vector2f();

            foreach(var go in objects)
            {
                var distSquared = this._startPos.DistanceSquared(go.GlobalCenterOfMass);
                distSquared /= 10;
                var offsetVec = go.GlobalCenterOfMass - this._startPos;
                var force = GravityObject.GravitationalConstant * (this._mass * go.Mass) * offsetVec /
                            (float)Math.Sqrt(Math.Pow((distSquared + GravityObject.Epsilon * GravityObject.Epsilon), 3));

                totalForce += force;
            }
            
            totalForce = totalForce.Clamp(this._maxLength);
            var endPoint = this._startPos + totalForce;
            this.Line = new[] { new Vertex(this._startPos, Color.White), new Vertex(endPoint, Color.Red) };
        }
    }
}
