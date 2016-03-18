using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfmlGravityWpf.GameModels
{
    using SFML.Graphics;
    using SFML.System;
    using SfmlGravityWpf.Code.Extensions;

    public class GravityShape : GravityObject
    {
        public GravityShape(Shape shape, float mass)
        {
            this.Mass = mass;
            this.Velocity = new Vector2f();
            this.Force = new Vector2f();
            this.Shape = shape;

            if(shape is CircleShape)
            {
                var circle = (CircleShape)shape;
                if (circle.Origin.X == 0 && circle.Origin.Y == 0)
                    this.RelativeCenterOfMass = new Vector2f(circle.Radius, circle.Radius);
            }
            else
            {
                this.RelativeCenterOfMass = new Vector2f();
            }            
        }

        public Shape Shape { get; set; }

        public override Vector2f GlobalCenterOfMass
        {
            get { return this.Shape.Position + this.RelativeCenterOfMass; }
        }

        public override void Move(float dTime)
        {
            var offset = this.Velocity * dTime;
            this.Shape.Move(offset);
        }

    }
}
