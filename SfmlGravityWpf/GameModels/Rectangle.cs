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
        private float X1 { get; set; }

        /// <summary>
        /// The x position of the right edge
        /// </summary>
        private float X2 { get; set; }

        /// <summary>
        /// The y position of the top edge
        /// </summary>
        private float Y1 { get; set; }

        /// <summary>
        /// the y position of the bottom edge
        /// </summary>
        private float Y2 { get; set; }

        private Vector2f TopLeft
        {
            get { return new Vector2f(this.X1, this.Y1); }
        }

        private Vector2f BottomRight
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
