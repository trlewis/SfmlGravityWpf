namespace SfmlGravityWpf.GameModels
{
    using System;
    using System.Collections.Generic;
    using SFML.Graphics;
    using SFML.System;
    using Code.Helpers;

    /// <summary>
    /// A point-object that has zero-size but has mass and interacts with other GravityObjects via gravity. Points
    /// cannot collide with each-other but they can collide with objects that do have size.
    /// </summary>
    public class GravityPoint : GravityDrawable, ICollidableGravityPoint
    {
        private readonly CircleShape _pointShape;

        public GravityPoint(CircleShape shape, float mass)
            : base(mass)
        {
            this._pointShape = shape;

            //TODO: remove this constructor, make gravitycircles their own object
            if(shape != null && shape.Origin.X == 0 && shape.Origin.Y == 0)
                this.RelativeCenterOfMass = new Vector2f(shape.Radius, shape.Radius);
        }

        #region ICollidableGravityX members

        public bool RemoveOnCollide
        {
            get { return true; }
        }

        public Rectangle GetBoundingRectangle()
        {
            var gsm = this.GlobalCenterOfMass;
            return new Rectangle(gsm, gsm);
        }

        public void HandleCollision(Vector2f collisionPoint, GravityObject other, Action<IList<GravityObject>> spawnedGravityAction)
        {
            this._pointShape.FillColor = ColorHelper.GetRandomColor();
        }

        public Vector2f GetPoint()
        {
            return this.GlobalCenterOfMass;
        }

        #endregion ICollidableGravityX members

        #region GravityDrawable members

        public override Vector2f GlobalCenterOfMass
        {
            get { return this._pointShape.Position + this.RelativeCenterOfMass; }
        }

        protected override Vector2f Position
        {
            get { return this._pointShape.Position; }
            set { this._pointShape.Position = value; }
        }

        public override void Draw(RenderTarget target)
        {
            target.Draw(this._pointShape);
        }

        #endregion GravityDrawable members
    }
}
