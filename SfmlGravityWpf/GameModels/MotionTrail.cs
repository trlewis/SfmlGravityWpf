namespace SfmlGravityWpf.GameModels
{
    using System.Collections.Generic;
    using SFML.Graphics;
    using SFML.System;

    /// <summary>
    /// A visual representation of the path an object takes. Needs to be updated every time the object
    /// is repositioned.
    /// </summary>
    public class MotionTrail
    {
        private const int DefaultTrailLength = 200;

        private readonly bool _isFading;
        private readonly int _trailLength;
        private readonly Queue<Vector2f> _positions;
        private readonly PrimitiveType _primitiveType;

        public MotionTrail(MotionTrailType type, int trailLength = DefaultTrailLength)
        {
            this._isFading = type == MotionTrailType.FadingDots || type == MotionTrailType.FadingLine;
            this._trailLength = trailLength;
            this._positions = new Queue<Vector2f>(this._trailLength);
            this._primitiveType = (type == MotionTrailType.EqualDots || type == MotionTrailType.FadingDots)
                ? PrimitiveType.Points
                : PrimitiveType.LinesStrip;
        }

        /// <summary>
        /// Add the most recent position to the motion trail
        /// </summary>
        /// <param name="position">The latest position to add to the trail</param>
        public void AddLocation(Vector2f position)
        {
            if (this._positions.Count >= this._trailLength)
                this._positions.Dequeue();
            this._positions.Enqueue(position);
        }

        public void Draw(RenderTarget target)
        {
            float colorVal = this._isFading ? 0 : 255;
            float dColor = 255f / this._trailLength;

            var vertices = new Vertex[this._positions.Count];
            var positionArray = this._positions.ToArray();
            for (int i = 0; i < this._positions.Count ; i++)
            {
                var color = new Color((byte) colorVal, (byte) colorVal, (byte) colorVal);
                vertices[i] = new Vertex(positionArray[i], color);

                if (this._isFading && colorVal < 255)
                {
                    colorVal += dColor;
                    if (colorVal > 255)
                        colorVal = 255;
                }
            }

            target.Draw(vertices, this._primitiveType);
        }
    }

    /// <summary>
    /// The visual style of a <see cref="MotionTrail"/>
    /// </summary>
    public enum MotionTrailType
    {
        /// <summary>
        /// A line that's brigther where the object was most recently
        /// </summary>
        FadingLine,

        /// <summary>
        /// A line that has constant brightness
        /// </summary>
        SolidLine,

        /// <summary>
        /// Dots that are located at positions the object had occupied,
        /// fading darker as time elapses
        /// </summary>
        FadingDots,

        /// <summary>
        /// Dots that are located at positions the object has occupied,
        /// they do not fade as time elapses
        /// </summary>
        EqualDots,
    }
}
