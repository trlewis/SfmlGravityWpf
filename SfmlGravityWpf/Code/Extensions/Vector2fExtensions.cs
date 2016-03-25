namespace SfmlGravityWpf.Code.Extensions
{
    using System;
    using SFML.System;

    public static class Vector2fExtensions
    {
        public static float DistanceSquared(this Vector2f vec, Vector2f other)
        {
            var offset = other - vec;
            return MagnitudeSquared(offset);
        }

        public static float Distance(this Vector2f vec, Vector2f other)
        {
            var offset = other - vec;
            return Magnitude(offset);
        }

        public static float MagnitudeSquared(this Vector2f vec)
        {
            return (vec.X * vec.X) + (vec.Y * vec.Y);
        }

        public static float Magnitude(this Vector2f vec)
        {
            return (float)Math.Sqrt(MagnitudeSquared(vec));
        }

        /// <summary>
        /// Normalizes the given vector into a unit vector in the same direction.
        /// </summary>
        /// <param name="vec">The vector to normalize</param>
        public static Vector2f Normalize(this Vector2f vec)
        {
            var mag = vec.Magnitude();
            return new Vector2f(vec.X / mag, vec.Y / mag);
        }

        /// <summary>
        /// If the given vector is longer than the given length, the vector is
        /// adjusted to the given length.
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Vector2f Clamp(this Vector2f vec, float length)
        {
            if (vec.MagnitudeSquared() <= (length * length))
                return vec;

            var normal = vec.Normalize();
            return normal * length;
        }
    }
}
