namespace SfmlGravityWpf.GameCode
{
    using System;
    using System.Collections.Generic;
    using Code.Extensions;
    using GameModels;
    using SFML.System;

    public static class CollisionParticleFactory
    {
        /// <summary>
        /// Creates particles going "outward" with some base velocity added
        /// </summary>
        /// <param name="totalMomentum">The total momentum of the two colliding objects</param>
        /// <param name="baseVelocity">The base velocity of each particle</param>
        /// <returns>A collection of particles</returns>
        public static IEnumerable<CollisionParticle> CreateParticles(Vector2f location, Vector2f baseVelocity)
        {
            var particles = new List<CollisionParticle>();
            var rand = new Random();
            //var momentMag = totalMomentum.Magnitude();
            
            //use totalMomentum to choose max additional speed to set particles
            //var speedScale = momentMag/2000;

            //use the magnitude of the momentum to determine how many particles to spawn. 
            //int numParticles = (int)momentMag/100;
            var numParticles = 75 + rand.Next(150);
            for (int i = 0; i < numParticles; i++)
            {
                var vx = (float) rand.NextDouble()*2 - 1;
                var vy = (float) rand.NextDouble()*2 - 1;
                //give direction, then set magnitude of speed, then randomly scale it, then add base velocity
                var velocity = new Vector2f(vx, vy).Normalize();
                //velocity *= speedScale;
                velocity *= (float)Math.Max(0.1, rand.NextDouble() * 40);
                velocity += baseVelocity;

                var particle = new CollisionParticle(location, velocity, 1, 0.1f);
                particles.Add(particle);
            }

            return particles;
        }
    }
}
