namespace SfmlGravityWpf.GameCode
{
    using System;
    using SFML.Graphics;
    using SFML.System;

    public static class ConvexShapeHelper
    {
        public static ConvexShape GetRandomConvexShape(uint vertices, float radius)
        {
            var shape = new ConvexShape();
            shape.SetPointCount(vertices);
            shape.FillColor = Color.Red;

            var rand = new Random();

            for (uint i = 0; i < vertices; i++)
            {
                var angle = (Math.PI * 2 * (i / (double)vertices));
                var x = -Math.Sin(angle);//so we're winding it CCW
                var y = Math.Cos(angle);

                var scale = (float)rand.Next(65, 110) / 100;
                x *= scale;
                y *= scale;

                var pos = new Vector2f((float)x, (float)y);
                pos *= radius;
                shape.SetPoint(i, pos);
            }

            return shape;
        }
    }
}
