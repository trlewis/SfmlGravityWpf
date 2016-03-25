namespace SfmlGravityWpf.GameModels
{
    using System;
    using System.Collections.Generic;
    using SFML.System;
    using Code.Extensions;

    public abstract class GravityObject
    {
        private const float GravitationalConstant = 0.05f;
        private const float Epsilon = 2f;

        public float Mass { get; set; }

        public Vector2f Velocity { get; set; }

        private Vector2f Force { get; set; }

        /// <summary>
        /// Center of mass relative to the location of the object
        /// </summary>
        protected Vector2f RelativeCenterOfMass { get; set; }

        /// <summary>
        /// The position in absolute space used for calculating phsyics
        /// </summary>
        public abstract Vector2f GlobalCenterOfMass { get; }

        /// <summary>
        /// Adjusts the object's velocity based on it's currently experience force. Does not
        /// move the object, only changes velocity.
        /// </summary>
        /// <param name="dTime">Time delta in seconds</param>
        public void ApplyForce(float dTime)
        {
            //F = ma -> a = F/m
            //v = a * dt

            if(this.Force == null || (this.Force.X == 0 && this.Force.Y == 0))
                return;

            var acceleration = this.Force / this.Mass;
            var dVelocity = acceleration * dTime;
            this.Velocity += dVelocity;
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
                {
                    continue;
                }
                
                var distSquared = this.GlobalCenterOfMass.DistanceSquared(obj.GlobalCenterOfMass);
                //calculate as though the objects are closer so the simulation moves faster.
                distSquared /= 10;

                var offsetVec = obj.GlobalCenterOfMass - this.GlobalCenterOfMass;
                //var force = GravitationalConstant * ((this.Mass * obj.Mass) / distSquared);

                //found this equation that is seems to help with keeping objects from shooting away from
                //each other when the distance between them is very small. Keeps the denominator to a minimum
                //value, but I'm not entirely sure how it works. Found it on a StackExchange post:
                //http://physics.stackexchange.com/questions/120183/why-does-my-gravity-simulation-do-this
                var force = GravitationalConstant * (this.Mass * obj.Mass) * offsetVec /
                            (float)Math.Sqrt(Math.Pow((distSquared + Epsilon * Epsilon), 3));

                //this is still using the Euler method for calculating gravity though. Leapfrog or Verlet
                //integration would (probably) be better. that's on the to do list though...

                totalForce += force;
            }

            this.Force = totalForce;
        }
    }
}
