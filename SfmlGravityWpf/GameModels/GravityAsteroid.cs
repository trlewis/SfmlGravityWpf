namespace SfmlGravityWpf.GameModels
{
    using System;
    using SfmlGravityWpf.Code.Extensions;
    using SFML.Graphics;
    using SFML.System;
    using Code.Helpers;

    public class GravityAsteroid : GravityDrawable, ICollidableGravityConvexPolygon
    {
        private readonly ConvexShape _convexShape = new ConvexShape();

        public GravityAsteroid(Vector2f position, float mass, float radius = 20, uint pointCount = 6)
        {
            this.MotionTrail = new MotionTrail(MotionTrailType.FadingLine);

            this.Mass = mass;
            this._convexShape.SetPointCount(pointCount);
            this._convexShape.FillColor = Color.Red;

            float rx = 0;
            float ry = 0;

            var rand = new Random();

            for (uint i = 0; i < pointCount; i++)
            {
                var angle = (Math.PI * 2 * (i / (double)pointCount));
                var x = -Math.Sin(angle);//so we're winding it CCW
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
        }

        public override Vector2f GlobalCenterOfMass
        {
            get { return this._convexShape.Position + this.RelativeCenterOfMass; }
        }

        protected override Vector2f Position
        {
            get { return this._convexShape.Position; }
            set { this._convexShape.Position = value; }
        }

        public override void Draw(RenderTarget target)
        {
            target.Draw(this._convexShape);
        }

        public Vector2f[] GetPoints()
        {
            var points = this._convexShape.GetPoints();
            for (int i = 0; i < points.Length; i++)
                points[i] = points[i] + this.GlobalCenterOfMass;

            return points;
        }
        
        public Rectangle GetBoundingRectangle()
        {
            return this._convexShape.GetBoundingRectangle() + this.GlobalCenterOfMass;
        }


        public void HandleCollision(Vector2f collisionPoint)
        {
            //TODO: actually handle collision.
            this._convexShape.FillColor = ColorHelper.GetRandomColor();
        }
    }
}
