﻿namespace SfmlGravityWpf.GameModels
{
    using System.Collections.Generic;
    using Code.Extensions;
    using SFML.Graphics;
    using SFML.System;

    public class ForceLine
    {
        private readonly Vector2f _startPos;
        private readonly float _mass;

        public ForceLine(Vector2f start, float mass)
        {
            this._startPos = start;
            this._mass = mass;
            this.Line = new[] { new Vertex(start, Color.White), new Vertex(start, Color.Red) };
        }

        public Vertex[] Line { get; private set; }

        public void CalculateLine(IEnumerable<GravityObject> objects)
        {
            //just going to use 1 second as the time.
            float fx = 0;
            float fy = 0;

            foreach(var go in objects)
            {
                var distSquared = this._startPos.DistanceSquared(go.GlobalCenterOfMass);
                var force = ((this._mass * go.Mass) / distSquared);

                var offsetVec = go.GlobalCenterOfMass - this._startPos;
                offsetVec = offsetVec.Normalize();

                fx += force * offsetVec.X;
                fy += force * offsetVec.Y;
            }

            var forceVec = new Vector2f(fx, fy);
            forceVec = forceVec.Clamp(20);

            var endPoint = this._startPos + forceVec;

            this.Line = new[] { new Vertex(this._startPos, Color.White), new Vertex(endPoint, Color.Red) };
        }
    }
}
