﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfmlGravityWpf.GameCode
{
    using SfmlGravityWpf.GameModels;
    using SfmlGravityWpf.Code.Helpers;
    using SfmlGravityWpf.Code.Extensions;

    using SFML.System;

    public class CollisionChecker
    {

        private readonly ICollidableGravityObject _first;
        private readonly ICollidableGravityObject _second;

        public CollisionChecker(ICollidableGravityObject first, ICollidableGravityObject second)
        {
            this._first = first;
            this._second = second;
        }

        public Vector2f CollisionPoint { get; private set; }

        public bool AreColliding()
        {
            var firstRect = this._first.GetBoundingRectangle();
            var secondRect = this._second.GetBoundingRectangle();
            if (!firstRect.IsIntersect(secondRect))
                return false;

            //check specific types
            if (this._first is ICollidableGravityPoint)
            {
                if (this._second is ICollidableGravityPoint)
                    return false;
                if (this._second is ICollidableGravityConvexPolygon)
                {
                    var point = (ICollidableGravityPoint)this._first;
                    var poly = (ICollidableGravityConvexPolygon)this._second;
                    return CheckPointToPolygonCollision(point, poly);
                }
            }

            if(this._first is ICollidableGravityConvexPolygon)
            {
                if (this._second is ICollidableGravityPoint)
                {
                    var point = (ICollidableGravityPoint)this._second;
                    var poly = (ICollidableGravityConvexPolygon)this._first;
                    return CheckPointToPolygonCollision(point, poly);
                }
                if (this._second is ICollidableGravityConvexPolygon)
                    return false; //TODO
            }

            return false;
        }

        private bool CheckPointToPolygonCollision(ICollidableGravityPoint point, ICollidableGravityConvexPolygon polygon)
        {
            //return true;
            var pointLoc = point.GetPoint();
            var polygonPoints = polygon.GetPoints();

            for (int i = 0; i < polygonPoints.Length; i++)
            {
                //var prev = points[IntHelper.WrapMod(i - 1, points.Length)];
                var current = polygonPoints[i];
                var next = polygonPoints[IntHelper.WrapMod(i + 1, polygonPoints.Length)];
                var offsetToNext = next - current;
                var offsetToPoint = pointLoc - current;

                var side = offsetToNext.Cross(offsetToPoint);
                if (side < 0)
                    return false;
            }

            this.CollisionPoint = pointLoc;

            return true;
        }
    }
}