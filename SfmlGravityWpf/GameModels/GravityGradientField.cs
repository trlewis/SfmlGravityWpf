namespace SfmlGravityWpf.GameModels
{
    using System;
    using System.Collections.Generic;
    using Code.Extensions;
    using SFML.Graphics;
    using SFML.System;

    public class GravityGradientField
    {
        private const float Mass = 10f;

        private readonly float _cellSize;
        private readonly int _width;
        private readonly int _height;
        //[x][y]
        private readonly float[,] _forces;

        public GravityGradientField(float cellSize, int width, int height)
        {
            this._cellSize = cellSize;
            this._width = width + 2;
            this._height = height + 2;
            this._forces = new float[this._width, this._height];
        }

        public void Calculate(IEnumerable<GravityObject> objects)
        {
            float foundMax = 0;
            for (int x = 0; x < this._width; x++)
            {
                for (int y = 0; y < this._height; y++)
                {
                    var point = new Vector2f(x * this._cellSize, y * this._cellSize);
                    var force = CalculateForce(objects, point);
                    this._forces[x, y] = force;
                    if (force > foundMax)
                        foundMax = force;
                }
            }
            
            //scale values so that some point is always at max
            if (foundMax >= 255)
                return;

            var scale = 255f/foundMax;
            for (int x = 0; x < this._width; x++)
            {
                for (int y = 0; y < this._height; y++)
                {
                    this._forces[x,y] *= scale;
                }
            }
        }

        public void Draw(RenderTarget target)
        {
            //create vertices
            var vertices = new Vertex[this._width, this._height];
            for (int x = 0; x < this._width; x++)
            {
                for (int y = 0; y < this._height; y++)
                {
                    var red = (byte) Math.Min(this._forces[x, y], 255);
                    var color = new Color(red, 0, 0);
                    var position = new Vector2f(x * this._cellSize, y * this._cellSize);
                    var vertex = new Vertex(position, color);
                    vertices[x, y] = vertex;
                }
            }

            //draw horizontal triangle strips
            for (uint y = 0; y < this._height - 1; y++)
            {
                var array = new VertexArray(PrimitiveType.TrianglesStrip, (uint) (this._width*2));
                for (uint x = 0; x < this._width; x++)
                {
                    array[2*x] = vertices[x, y];
                    array[2*x + 1] = vertices[x, y + 1];
                }
                target.Draw(array);
            }
        }

        private static float CalculateForce(IEnumerable<GravityObject> objects, Vector2f point)
        {
            var totalForce = new Vector2f();

            foreach (var go in objects)
            {
                var distSquared = point.DistanceSquared(go.GlobalCenterOfMass);
                distSquared /= 10;
                var offsetVec = go.GlobalCenterOfMass - point;
                var force = GravityObject.GravitationalConstant*(Mass*go.Mass)*offsetVec.Normalize()/
                            distSquared;

                totalForce += force;
            }

            return totalForce.Magnitude();
        }
    }
}
