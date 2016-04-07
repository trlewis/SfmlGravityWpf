namespace SfmlGravityWpf.GameModels
{
    using SFML.Graphics;
    using SFML.System;
    using Code.Extensions;

    public class GravityShape : GravityObject
    {
        public GravityShape(Shape shape, float mass)
        {
            this.MotionTrail = new MotionTrail(MotionTrailType.FadingLine);
            
            this.Mass = mass;
            this.Velocity = new Vector2f();
            this.Shape = shape;

            var circle = shape as CircleShape;
            if(circle != null && circle.Origin.X == 0 && circle.Origin.Y == 0)
                this.RelativeCenterOfMass = new Vector2f(circle.Radius, circle.Radius);
            else
                this.RelativeCenterOfMass = new Vector2f();
        }

        public Shape Shape { get; set; }

        public MotionTrail MotionTrail { get; private set; }

        public override Vector2f GlobalCenterOfMass
        {
            get { return this.Shape.Position + this.RelativeCenterOfMass; }
        }

        public void Move(float dTime)
        {
            var offset = this.Velocity * dTime;
            this.Shape.Move(offset);
        }

    }
}
