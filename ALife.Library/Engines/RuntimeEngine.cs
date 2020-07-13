using ALife.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ALife.Engines
{
    public class RuntimeEngine
    {
        public const float EyeAngle = 10 * (float)Math.PI / 360;
        public const int VisionLimit = 25;
        public const int VisionLimitSquared = VisionLimit * VisionLimit;

        public void UpdateBot(Bot bot)
        {
            bot.Orientation = Helpers.NormaliseAngle(bot.Orientation + Helpers.IntToAngle(bot.Memory[(int)MemoryAddresses.TurnRight] - bot.Memory[(int)MemoryAddresses.TurnLeft]));

            // Right and down are positive x and y respectively
            var netForce = new Vector2(
                bot.GetFromMemory(MemoryAddresses.MoveUp) - bot.GetFromMemory(MemoryAddresses.MoveDown),
                bot.GetFromMemory(MemoryAddresses.MoveRight) - bot.GetFromMemory(MemoryAddresses.MoveLeft));

            var rotation = Matrix4x4.CreateRotationZ(bot.Orientation);

            bot.Force += Vector2.Transform(netForce, rotation);
        }

        public void UpdateMemory(Bot bot)
        {
            // Clear the instruction memory
            bot.SetMemory(MemoryAddresses.MoveUp, 0);
            bot.SetMemory(MemoryAddresses.MoveDown, 0);
            bot.SetMemory(MemoryAddresses.MoveLeft, 0);
            bot.SetMemory(MemoryAddresses.MoveRight, 0);
            bot.SetMemory(MemoryAddresses.TurnLeft, 0);
            bot.SetMemory(MemoryAddresses.TurnRight, 0);

            // Populate the state memory
            var rotation = Matrix4x4.CreateRotationZ(-bot.Orientation);
            var speed = Vector2.Transform(bot.Speed, rotation);

            bot.SetMemory(MemoryAddresses.SpeedForward, speed.X);
            bot.SetMemory(MemoryAddresses.SpeedRight, speed.Y);
        }

        public void UpdateVision(Bot bot, IEnumerable<Bot> allBots)
        {
            foreach (var otherBot in allBots.Where(b => b != bot))
            {
                var distanceVector = bot.Position - otherBot.Position;
                var distanceSquared = distanceVector.LengthSquared();
                if (distanceSquared < VisionLimitSquared)
                {
                    var angleToOtherBot = (float)Math.Atan2(distanceVector.Y, distanceVector.X);
                    var relativeAngle = Helpers.NormaliseAngle(angleToOtherBot - bot.Orientation);
                    if (relativeAngle > Math.PI)
                        relativeAngle -= 2 * (float)Math.PI;

                    var distance = Math.Sqrt(distanceSquared);
                    var radiusAngle = Math.Atan2(otherBot.Radius, distance);
                    var visibleAngleStart = relativeAngle - radiusAngle;
                    var visibleAngleStop = relativeAngle + radiusAngle;

                    if ()
                }
            }
        }
    }
}