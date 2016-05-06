namespace SfmlGravityWpf.GameModels
{
    using SFML.Graphics;
    using SFML.System;

    public class GravityPoint : GravityDrawable, IDrawable
    {
        protected GravityPoint()
        {
            this.MotionTrail = new MotionTrail(MotionTrailType.FadingLine);
            this.Velocity = new Vector2f();
        }

        public GravityPoint(CircleShape shape, float mass)
            : this()
        {
            this.Mass = mass;
            this.PointShape = shape;

            //TODO: remove this constructor, make gravitycircles their own object
            var circle = shape as CircleShape;
            if(circle != null && circle.Origin.X == 0 && circle.Origin.Y == 0)
                this.RelativeCenterOfMass = new Vector2f(circle.Radius, circle.Radius);
        }

        public CircleShape PointShape { get; set; }

        //public MotionTrail MotionTrail { get; private set; }

        public override Vector2f GlobalCenterOfMass
        {
            get { return this.PointShape.Position + this.RelativeCenterOfMass; }
        }

        protected override Vector2f Position
        {
            get { return this.PointShape.Position; }
            set { this.PointShape.Position = value; }
        }

        public override void Draw(RenderTarget target)
        {
            target.Draw(this.PointShape);
        }
    }
}
