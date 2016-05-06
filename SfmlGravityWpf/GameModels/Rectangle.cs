using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfmlGravityWpf.GameModels
{
    using SFML.System;

    public struct Rectangle
    {
        public Rectangle(float x1, float y1, float x2, float y2)
            : this()
        {
            this.X1 = x1;
            this.X2 = x2;
            this.Y1 = y1;
            this.Y2 = y2;
        }

        public Rectangle(Vector2f topLeft, Vector2f bottomRight)
            : this(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y)
        { }

        /// <summary>
        /// The x position of the left edge
        /// </summary>
        public float X1 { get; private set; }

        /// <summary>
        /// The x position of the right edge
        /// </summary>
        public float X2 { get; private set; }

        /// <summary>
        /// The y position of the top edge
        /// </summary>
        public float Y1 { get; private set; }

        /// <summary>
        /// the y position of the bottom edge
        /// </summary>
        public float Y2 { get; private set; }

        public Vector2f TopLeft
        {
            get { return new Vector2f(this.X1, this.Y1); }
        }

        public Vector2f BottomRight
        {
            get { return new Vector2f(this.X2, this.Y2); }
        }

        /// <summary>
        /// Checks if the given Rectangle overlaps this rectangle.
        /// </summary>
        public bool IsIntersect(Rectangle other)
        {
            return this.X1 < other.X2 && this.X2 > other.X1
                && this.Y1 < other.Y2 && this.Y2 > other.Y1;
        }
        
        public static Rectangle operator +(Rectangle rekt, Vector2f offset)
        {
            return new Rectangle(rekt.TopLeft + offset, rekt.BottomRight + offset);
        }

    }
}
