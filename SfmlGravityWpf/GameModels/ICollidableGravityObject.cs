namespace SfmlGravityWpf.GameModels
{
    using System;
    using System.Collections.Generic;
    using SFML.System;

    public interface ICollidableGravityObject
    {
        bool RemoveOnCollide { get; }

        Rectangle GetBoundingRectangle();

        void HandleCollision(Vector2f collisionPoint, GravityObject other, Action<IList<GravityObject>> spawnedObjectAction);
    }

    public interface ICollidableGravityPoint : ICollidableGravityObject
    {
        Vector2f GetPoint();
    }

    public interface ICollidableGravityConvexPolygon : ICollidableGravityObject
    {
        Vector2f[] GetGlobalPoints();
    }
}
