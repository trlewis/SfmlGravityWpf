namespace SfmlGravityWpf.GameModels
{
    using SFML.Graphics;
    using SFML.System;

    public class GravityShape : GravityObject
    {
        protected GravityShape()
        {
            this.MotionTrail = new MotionTrail(MotionTrailType.FadingLine);
            this.Velocity = new Vector2f();
        }

        public GravityShape(Shape shape, float mass)
            : this()
        {
            this.Mass = mass;
            this.Shape = shape;

            //TODO: remove this constructor, make gravitycircles their own object
            var circle = shape as CircleShape;
            if(circle != null && circle.Origin.X == 0 && circle.Origin.Y == 0)
                this.RelativeCenterOfMass = new Vector2f(circle.Radius, circle.Radius);
        }

        public Shape Shape { get; set; }

        public MotionTrail MotionTrail { get; private set; }

        public override Vector2f GlobalCenterOfMass
        {
            get { return this.Shape.Position + this.RelativeCenterOfMass; }
        }

        protected override Vector2f Position
        {
            get { return this.Shape.Position; }
            set { this.Shape.Position = value; }
        }
    }
}
