using System;

namespace ALife.Engines
{
    public class Helpers
    {
        public static float RadianPerInt = (float)(Math.PI * 2 / 1000);

        public static float AngleToInt(float angle)
        {
            return angle / RadianPerInt;
        }

        public static float IntToAngle(int angle)
        {
            return angle * RadianPerInt;
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