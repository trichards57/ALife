using System.Numerics;

namespace ALife.Library.Helpers
{
    public static class VectorHelpers
    {
        public static float Cross(this Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.Y + v1.Y * v2.X;
        }
    }
}