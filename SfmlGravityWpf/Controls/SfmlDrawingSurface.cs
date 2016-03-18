using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfmlGravityWpf.Controls
{
    public class SfmlDrawingSurface : System.Windows.Forms.Panel
    {
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            //commented out so the SFML window will draw
            //base.OnPaint(e);
        }

        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
        {
            //commented out so the SFML window will draw
            //base.OnPaintBackground(e);
        }
    }
}
