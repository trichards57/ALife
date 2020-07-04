using System.Collections.Generic;
using System.Numerics;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ALife.Model
{
    public class Bot
    {
        public Color Color { get; set; }
        public List<BasePair> DNA { get; set; }
        public Ellipse Ellipse { get; set; }
        public Vector2 Force { get; set; }
        public float Mass { get; set; } = 10;
        public IList<int> Memory { get; } = new int[SystemVariables.MemoryLength];
        public float Orientation { get; set; }
        public Vector2 Position { get; set; }
        public IReadOnlyList<int> PreviousMemory { get; private set; }
        public float Radius { get; set; } = 10;
        public Vector2 Speed { get; set; }
    }
}