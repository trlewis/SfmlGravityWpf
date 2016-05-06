namespace SfmlGravityWpf.GameModels
{
    using System;
    using System.Collections.Generic;
    using SFML.System;
    using Code.Extensions;

    public abstract class GravityObject
    {
        public const float GravitationalConstant = 0.5f;//0.05f;
        public const float Epsilon = 2f;

        private bool _initialVelocitySet;

        public float Mass { get; set; }

        public Vector2f Velocity { get; set; }

        private Vector2f VelocityHalfStep { get; set; }

        private Vector2f Force { get; set; }

        /// <summary>
        /// Center of mass relative to the location of the object
        /// </summary>
        protected Vector2f RelativeCenterOfMass { get; set; }

        /// <summary>
        /// The position in absolute space used for calculating phsyics
        /// </summary>
        public abstract Vector2f GlobalCenterOfMass { get; }

        protected abstract Vector2f Position { get; set; }

        public virtual void Move(float dTime)
        {
            this.Position += dTime*this.VelocityHalfStep;
        }

        public void UpdateVelocity(float dTime)
        {
            var acceleration = this.Force / this.Mass;
            if (this._initialVelocitySet)
            {
                this.VelocityHalfStep += dTime*acceleration;
            }
            else
            {
                this._initialVelocitySet = true;

                //if the particle was given an initial velocity, just use that.
                if (this.Velocity != new Vector2f(0, 0))
                    this.VelocityHalfStep = this.Velocity;
                else
                    this.VelocityHalfStep = 0.5f*dTime*acceleration;
            }

            this.Velocity = this.VelocityHalfStep + dTime*acceleration;
        }

        /// <summary>
        /// Calculates the net force experienced by the GravityObject from the other
        /// GravityObjects passed in.
        /// </summary>
        /// <param name="objects">The other objecs that will exert force on this GravityObject</param>
        public void CalculateForce(IEnumerable<GravityObject> objects)
        {
            //float fx = 0;
            //float fy = 0;
            var totalForce = new Vector2f();

            foreach(var obj in objects)
            {
                if(obj == this)
                    continue;

                var distSquared = this.GlobalCenterOfMass.DistanceSquared(obj.GlobalCenterOfMass);
                //calculate as though the objects are closer so the simulation moves faster.
                distSquared /= 10;
                var offsetVec = obj.GlobalCenterOfMass - this.GlobalCenterOfMass;

                //found this equation that is seems to help with keeping objects from shooting away from
                //each other when the distance between them is very small. Keeps the denominator to a minimum
                //value, but I'm not entirely sure how it works. Found it on a StackExchange post:
                //http://physics.stackexchange.com/questions/120183/why-does-my-gravity-simulation-do-this
                var force = GravitationalConstant * (this.Mass * obj.Mass) * offsetVec /
                            (float)Math.Sqrt(Math.Pow((distSquared + Epsilon * Epsilon), 3));

                totalForce += force;
            }

            this.Force = totalForce;
        }
    }
}
