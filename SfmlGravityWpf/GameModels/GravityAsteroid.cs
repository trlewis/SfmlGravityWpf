namespace SfmlGravityWpf.GameModels
{
    using System;
    using System.Collections.Generic;
    using Code.Extensions;
    using GameCode;
    using SFML.Graphics;
    using SFML.System;
    using Code.Helpers;

    public class GravityAsteroid : GravityDrawable, ICollidableGravityConvexPolygon
    {
        private readonly ConvexShape _convexShape = new ConvexShape();
        private readonly float _radius;

        public GravityAsteroid(Vector2f position, float mass, float radius = 20, uint pointCount = 6)
            : base(mass)
        {
            this._radius = radius;
            this._convexShape = ConvexShapeHelper.GetRandomConvexShape(pointCount, radius);
            this._convexShape.FillColor = Color.Red;
            this.RelativeCenterOfMass = this._convexShape.GetRelativeCenter();
            this._convexShape.Position = position;
        }

        #region ICollidableGravityX members

        public bool RemoveOnCollide
        {
            get { return true; }
        }

        public Rectangle GetBoundingRectangle()
        {
            return this._convexShape.GetBoundingRectangle() + this.GlobalCenterOfMass;
        }

        public void HandleCollision(Vector2f collisionPoint, GravityObject other, Action<IList<GravityObject>> spawnedGravityAction)
        {
            this._convexShape.FillColor = ColorHelper.GetRandomColor();
            var center = this._convexShape.Position;
            const float explosionSpeed = 28; //TODO: a better system than this

            //create fragments
            var fragments = new List<GravityObject>();
            var globalPoints = this.GetGlobalPoints();
            var fragmentMass = this.Mass/globalPoints.Length;

            var otherMomentumToAdd = other.GetMomentum()/globalPoints.Length;

            for (int i = 0; i < globalPoints.Length; i++)
            {
                var thisPoint = globalPoints[i];
                var nextPoint = globalPoints[IntHelper.WrapMod(i + 1, globalPoints.Length)];
                var fragCenterX = (thisPoint.X + nextPoint.X + center.X)/3;
                var fragCenterY = (thisPoint.Y + nextPoint.Y + center.Y)/3;
                var fragCenter = new Vector2f(fragCenterX, fragCenterY);

                var vertices = new[] { thisPoint - fragCenter, nextPoint - fragCenter, center - fragCenter};
                var fragment = new GravityAsteroidFragment(new Vector2f(fragCenterX, fragCenterY), vertices, fragmentMass);

                var angle = Math.PI*2/(globalPoints.Length/(float) i);
                var vx = (float) Math.Cos(angle) * explosionSpeed; //extra "outward explosion" movement
                var vy = (float) -Math.Sin(angle) * explosionSpeed;

                fragment.Velocity = this.Velocity + new Vector2f(vx, vy);
                fragment.AddMomentum(otherMomentumToAdd);
                fragments.Add(fragment);
            }

            if (spawnedGravityAction != null)
                spawnedGravityAction(fragments);
        }

        /// <summary>
        /// Gets the positions of points of the vertices of the asteroid in absolute space. Not relative to the shape.
        /// </summary>
        public Vector2f[] GetGlobalPoints()
        {
            var points = this._convexShape.GetPoints();
            for (int i = 0; i < points.Length; i++)
                points[i] = points[i] + this._convexShape.Position;

            return points;
        }

        #endregion ICollidableGravityX members

        #region GravityDrawable members

        public override void Draw(RenderTarget target)
        {
            target.Draw(this._convexShape);
        }

        public override Vector2f GlobalCenterOfMass
        {
            get { return this._convexShape.Position + this.RelativeCenterOfMass; }
        }

        protected override Vector2f Position
        {
            get { return this._convexShape.Position; }
            set { this._convexShape.Position = value; }
        }

        #endregion GravityDrawable members
    }
}
