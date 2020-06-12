using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace ALife.Model
{
    public class Bot
    {
        public Ellipse Ellipse { get; set; }
        public Vector2 Force { get; set; }
        public float Mass { get; set; } = 10;
        public Vector2 Position { get; set; }
        public float Radius { get; set; } = 10;
        public Vector2 Speed { get; set; }
    }
}
