using System;

namespace ALife.Helpers
{
    public static class AngleHelper
    {
        public static double RadianPerInt = (Math.PI * 2 / 1000);

        public static int AngleToInt(float angle)
        {
            return (int)(angle / RadianPerInt);
        }

        public static float IntToAngle(int angle)
        {
            return (float)(angle * RadianPerInt);
        }

        public static float NormaliseAngle(float angle)
        {
            while (angle < 0)
                angle += (float)Math.PI * 2;
            while (angle > Math.PI * 2)
                angle -= (float)Math.PI * 2;

            return angle;
        }
    }
}