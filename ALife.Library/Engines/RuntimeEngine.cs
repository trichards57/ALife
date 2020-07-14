﻿using ALife.Library.Model;
using ALife.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ALife.Engines
{
    public class RuntimeEngine
    {
        private const int VisionLimitSquared = Bot.VisionLimit * Bot.VisionLimit;

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
            foreach (var e in bot.Eyes)
                e.Clear();

            foreach (var otherBot in allBots.Where(b => b != bot))
            {
                var distanceVector = otherBot.Position - bot.Position;
                var distanceSquared = distanceVector.LengthSquared();
                if (distanceSquared < VisionLimitSquared)
                {
                    // Take the bearing to the centre of the other bot
                    var angleToOtherBot = (float)Math.Atan2(distanceVector.Y, distanceVector.X);
                    var relativeAngle = Helpers.NormaliseAngle(angleToOtherBot - bot.Orientation);
                    // Translate to within -pi to +pi (move the discontinuity to outside the edges of vision)
                    if (relativeAngle > Math.PI)
                        relativeAngle -= 2 * (float)Math.PI;

                    // Calculate what arc the other bot should fill
                    var distance = Math.Sqrt(distanceSquared);
                    var radiusAngle = Math.Atan2(otherBot.Radius, distance);
                    var visibleAngleStart = relativeAngle - radiusAngle;
                    var visibleAngleStop = relativeAngle + radiusAngle;

                    for (var i = 0; i < Bot.EyeCount; i++)
                    {
                        var eyePosition = i - Bot.EyeCount / 2;
                        var eyeStart = (eyePosition * Bot.EyeAngle * 2) - Bot.EyeAngle;
                        var eyeStop = (eyePosition * Bot.EyeAngle * 2) + Bot.EyeAngle;

                        if (
                            visibleAngleStart > eyeStart && visibleAngleStart < eyeStop // Other bot start edge is in the eye
                            || visibleAngleStop > eyeStart && visibleAngleStop < eyeStop // Other bot stop edge is in the eye
                            || visibleAngleStart < eyeStart && visibleAngleStop > eyeStop // Other bot completely covers the eye
                            )
                        {
                            bot.Eyes[i].Add(new EyeEntry(otherBot, (float)distance));
                        }
                    }
                }
            }

            bot.EyeDistances = bot.Eyes.Select(e => e.Any() ? Bot.VisionLimit - e.Min(n => n.Distance) : 0).ToArray();
        }
    }
}
