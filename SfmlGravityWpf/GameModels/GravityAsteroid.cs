namespace SfmlGravityWpf.GameModels
{
    using System;
    using SFML.Graphics;
    using SFML.System;

    public class GravityAsteroid : GravityShape
    {
        private readonly ConvexShape _convexShape = new ConvexShape();

        public GravityAsteroid(Vector2f position, float mass, float radius = 20, uint pointCount = 6)
        {
            this.Mass = mass;
            this._convexShape.SetPointCount(pointCount);
            this._convexShape.FillColor = Color.Red;

            float rx = 0;
            float ry = 0;

            var rand = new Random();

            for (uint i = 0; i < pointCount; i++)
            {
                var angle = (Math.PI * 2 * (i / (double)pointCount));
                var x = Math.Sin(angle);
                var y = Math.Cos(angle);

                var scale = (float)rand.Next(65, 110)/100;
                x *= scale;
                y *= scale;

                var pos = new Vector2f((float)x, (float)y);
                pos *= radius;
                this._convexShape.SetPoint(i, pos);
                rx += pos.X;
                ry += pos.Y;
            }

            rx /= pointCount;
            ry /= pointCount;
            this.RelativeCenterOfMass = new Vector2f(rx, ry);

            this._convexShape.Position = position;
            this.Shape = this._convexShape;
        }

    }
}
