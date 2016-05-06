namespace SfmlGravityWpf.GameModels
{
    using SFML.Graphics;
    using SFML.System;

    public abstract class GravityDrawable : GravityObject, IDrawable
    {
        protected GravityDrawable(float mass)
        {
            this.Velocity = new Vector2f(0,0);
            this.MotionTrail = new MotionTrail(MotionTrailType.FadingLine);
            this.Mass = mass;
        }

        public MotionTrail MotionTrail { get; private set; }

        public abstract void Draw(RenderTarget target);

        public override void Move(float dTime)
        {
            base.Move(dTime);
            //TODO? check if MotionTrail is null first?
            this.MotionTrail.AddLocation(this.GlobalCenterOfMass);
        }
    }
}
