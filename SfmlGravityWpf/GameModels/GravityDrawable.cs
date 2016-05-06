namespace SfmlGravityWpf.GameModels
{
    using SFML.Graphics;

    public abstract class GravityDrawable : GravityObject, IDrawable
    {
        protected GravityDrawable(float mass)
        {
            this.MotionTrail = new MotionTrail(MotionTrailType.FadingLine);
            this.Mass = mass;
        }

        public MotionTrail MotionTrail { get; protected set; }

        public abstract void Draw(RenderTarget target);

        public override void Move(float dTime)
        {
            base.Move(dTime);
            //TODO? check if MotionTrail is null first?
            this.MotionTrail.AddLocation(this.GlobalCenterOfMass);
        }
    }
}
