using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfmlGravityWpf.GameModels
{
    using SFML.System;
    using SfmlGravityWpf.Code.Extensions;

    public abstract class GravityObject
    {
        public const float GravitationalConstant = 9.8f;

        public float Mass { get; set; }

        public Vector2f Velocity { get; set; }

        public Vector2f Force { get; set; }

        /// <summary>
        /// Center of mass relative to the location of the object
        /// </summary>
        public virtual Vector2f RelativeCenterOfMass { get; set; }

        /// <summary>
        /// The position in absolute space used for calculating phsyics
        /// </summary>
        public abstract Vector2f GlobalCenterOfMass { get; }

        /// <summary>
        /// Moves the object based on it's current velocity and the amount
        /// of time given.
        /// </summary>
        /// <param name="dTime">Time delta in seconds</param>
        public abstract void Move(float dTime);

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
            // F = G ((m1 * m2) / r^2)

            float fx = 0;
            float fy = 0;

            foreach(var obj in objects)
            {
                if(obj == this)
                {
                    continue;
                }
                
                var distSquared = this.GlobalCenterOfMass.DistanceSquared(obj.GlobalCenterOfMass);
                var force = GravitationalConstant * ((this.Mass * obj.Mass) / distSquared);

                var offsetVec = obj.GlobalCenterOfMass - this.GlobalCenterOfMass;
                offsetVec = offsetVec.Normalize();

                fx += force * offsetVec.X;
                fy += force * offsetVec.Y;
            }

            this.Force = new Vector2f(fx, fy);
        }
    }
}
