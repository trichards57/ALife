using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ALife.Model
{
    internal class Engine
    {
        private const int InitialRobotCount = 10;
        private const float WallCollisionLoss = 0.02f;
        private readonly Random _random = new Random();
        private bool shouldStop = false;

        public Engine()
        {
            Field.Size = new Vector2(1000, 1000);

            for (var i = 0; i < InitialRobotCount; i++)
            {
                Bots.Add(new Bot
                {
                    Position = new Vector2(
                        (float)_random.NextDouble() * Field.Size.X,
                        (float)_random.NextDouble() * Field.Size.Y),
                    Speed = new Vector2(
                        (float)_random.NextDouble() * 10,
                        (float)_random.NextDouble() * 10)
                });
            }
        }

        public List<Bot> Bots { get; } = new List<Bot>();
        public Func<List<Bot>, Task> CycleCallback { get; set; }
        public Field Field { get; } = new Field();
        public bool IsStopped { get; set; }

        public async Task RunCycle()
        {
            shouldStop = false;
            IsStopped = false;

            while (!shouldStop)
            {
                Parallel.ForEach(Bots, b =>
                {
                    UpdateSpeed(b);
                    UpdatePosition(b);
                });

                await CycleCallback(Bots);
            }

            IsStopped = true;
        }

        public void StopCycle()
        {
            shouldStop = true;
        }

        private void UpdatePosition(Bot bot)
        {
            var radiusVector = new Vector2(bot.Radius / 2, bot.Radius / 2);
            bot.Position = bot.Speed + bot.Position;

            var wallForceX = 0.0f;
            var wallForceY = 0.0f;

            if (bot.Position.X <= bot.Radius / 2 || bot.Position.X >= Field.Size.X - bot.Radius / 2)
                wallForceX = -(2 - WallCollisionLoss) * bot.Speed.X * bot.Mass;
            if (bot.Position.Y <= bot.Radius / 2 || bot.Position.Y >= Field.Size.Y - bot.Radius / 2)
                wallForceY = -(2 - WallCollisionLoss) * bot.Speed.Y * bot.Mass;

            bot.Force += new Vector2(wallForceX, wallForceY);

            bot.Position = Vector2.Clamp(bot.Position, radiusVector, Field.Size - radiusVector);
        }

        private void UpdateSpeed(Bot bot)
        {
            bot.Speed += bot.Force / bot.Mass;

            bot.Force = Vector2.Zero;
        }
    }
}
