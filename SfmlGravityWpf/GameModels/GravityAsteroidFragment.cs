namespace SfmlGravityWpf.GameModels
{
    using System;
    using System.Collections.Generic;
    using Code.Extensions;
    using Code.Helpers;
    using GameCode;
    using SFML.Graphics;
    using SFML.System;

    /// <summary>
    /// A triangle that "used to" make up the polygon that was a GravityAsteroid. These objects are
    /// still collidable but no longer break apart
    /// </summary>
    public class GravityAsteroidFragment : GravityDrawable, ICollidableGravityConvexPolygon
    {
        private readonly ConvexShape _triangle;

        public GravityAsteroidFragment(Vector2f position, float radius, float mass)
            : base(mass)
        {
            this._triangle = ConvexShapeHelper.GetRandomConvexShape(3, radius);
            this._triangle.FillColor = Color.Red;
            this.RelativeCenterOfMass = this._triangle.GetRelativeCenter();
            this._triangle.Position = position;
            
            var rand = new Random((int)position.X + (int)position.Y);
            var angle = rand.Next(360);
            this._triangle.Rotation = angle;
        }

        #region ICollidableGravityX members

        public bool RemoveOnCollide
        {
            get { return false; }
        }

        public Rectangle GetBoundingRectangle()
        {
            var rekt = this._triangle.GetBoundingRectangle();
            return rekt + this.Position;
        }

        public void HandleCollision(Vector2f collisionPoint, GravityObject other, Action<IList<GravityObject>> spawnedGravityAction)
        {
            this._triangle.FillColor = ColorHelper.GetRandomColor();
        }

        public Vector2f[] GetGlobalPoints()
        {
            var points = this._triangle.GetPoints();
            for (int i = 0; i < points.Length; i++)
                points[i] = points[i] + this._triangle.Position;

            return points;
        }

        #endregion ICollidableGravityX members

        #region GravityDrawable members

        public override Vector2f GlobalCenterOfMass
        {
            get { return this._triangle.Position + this.RelativeCenterOfMass; }
        }

        protected override Vector2f Position
        {
            get { return this._triangle.Position; }
            set { this._triangle.Position = value; }
        }

        public override void Draw(RenderTarget target)
        {
            target.Draw(this._triangle);
        }

        #endregion GravityDrawable members
    }
}
