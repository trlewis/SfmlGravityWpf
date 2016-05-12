namespace SfmlGravityWpf.GameModels
{
    using System;
    using SFML.Graphics;
    using SFML.System;

    public class CollisionParticle
    {
        private readonly float _coolingSpeed;
        //private float _brightness;
        //private Vector2f _location;
        private readonly Vector2f _velocity;
        private readonly Vertex[] _particle;

        /// <summary>
        /// Creates a particle that goes from white/bright yellow and fades to red and eventually black.
        /// </summary>
        /// <param name="location">The starting location of the particle.</param>
        /// <param name="velocity">The velocity of the particle</param>
        /// <param name="startBrightness">the starting brightness of the particle</param>
        /// <param name="coolingSpeed">How much the particles brightness should change every update</param>
        public CollisionParticle(Vector2f location, Vector2f velocity, float startBrightness, float coolingSpeed)
        {
            this._velocity = velocity;
            this._coolingSpeed = coolingSpeed;
            
            //TODO: set initial color based on brightness

            var point = new Vertex(location) {Color = Color.White};
            this._particle = new [] { point };
        }

        public bool IsDead
        {
            get
            {
                var color = this._particle[0].Color;
                return color.R <= 0 && color.G <= 0 && color.B <= 0;
            }
        }

        /// <summary>
        /// Moves the particle and updates its color.
        /// </summary>
        public void Update(float dSeconds)
        {
            this._particle[0].Position += this._velocity * dSeconds;

            //TODO: update color based on cooling speed

            var color = this._particle[0].Color;
            color.R = (byte)Math.Max(0, color.R - 1);
            color.G = (byte) Math.Max(0, color.G - 3);
            color.B = (byte) Math.Max(0, color.B - 10);
            this._particle[0].Color = color;
        }

        public void Draw(RenderTarget target)
        {
            target.Draw(this._particle, PrimitiveType.Points);
        }
    }
}
