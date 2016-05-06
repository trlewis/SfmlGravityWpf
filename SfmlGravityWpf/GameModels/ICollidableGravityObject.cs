namespace SfmlGravityWpf.GameModels
{
    using SFML.System;

    public interface ICollidableGravityObject
    {
        //CollidableType CollidableType { get; }

        Rectangle GetBoundingRectangle();

        void HandleCollision(Vector2f collisionPoint);
    }

    public interface ICollidableGravityPoint : ICollidableGravityObject
    {
        Vector2f GetPoint();
    }

    public interface ICollidableGravityConvexPolygon : ICollidableGravityObject
    {
        Vector2f[] GetPoints();
    }
}
