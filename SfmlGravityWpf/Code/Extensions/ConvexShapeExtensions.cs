namespace SfmlGravityWpf.Code.Extensions
{
    using GameModels;
    using SFML.Graphics;
    using SFML.System;

    public static class ConvexShapeExtensions
    {
        /// <summary>
        /// This bounding rectangle values are relative to the center of the ConvexShape,
        /// not relative to the origin of the screen.
        /// </summary>
        /// <param name="shape">The ConvexShape to get the bounding rectangle of</param>
        public static Rectangle GetBoundingRectangle(this ConvexShape shape)
        {
            var first = shape.GetPoint(0);
            var leftx = first.X;
            var rightx = first.X;
            var topy = first.Y;
            var bottomy = first.Y;
            
            for(uint i = 1 ; i < shape.GetPointCount(); i++)
            {
                var point = shape.GetPoint(i);
                if (point.X < leftx)
                    leftx = point.X;
                else if (point.X > rightx)
                    rightx = point.X;

                if (point.Y < topy)
                    topy = point.Y;
                else if (point.Y > bottomy)
                    bottomy = point.Y;
            }

            return new Rectangle(leftx, topy, rightx, bottomy);
        }

        /// <summary>
        /// Gets the location of the points relative to the center of the polygon.
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        public static Vector2f[] GetPoints(this ConvexShape shape)
        {
            var points = new Vector2f[shape.GetPointCount()];
            for (uint i = 0; i < shape.GetPointCount(); i++)
            {
                points[i] = shape.GetPoint(i);
            }
            return points;
        }

        /// <summary>
        /// Gets the "center point" of the polygon.
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        public static Vector2f GetRelativeCenter(this ConvexShape shape)
        {
            var points = shape.GetPoints();

            float centerX = 0f;
            float centerY = 0f;

            for (int i = 0; i < points.Length; i++)
            {
                centerX += points[i].X;
                centerY += points[i].Y;
            }

            return new Vector2f(centerX / points.Length, centerY / points.Length);
        }
    }
}
