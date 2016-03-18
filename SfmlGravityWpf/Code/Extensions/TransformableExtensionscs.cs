using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfmlGravityWpf.Code.Extensions
{
    using SFML.System;
    using SFML.Graphics;

    public static class TransformableExtensionscs
    {
        public static void Move(this Transformable transformable, Vector2f offset)
        {
            var newPos = new Vector2f(transformable.Position.X + offset.X, transformable.Position.Y + offset.Y);
            transformable.Position = newPos;
        }
    }
}
