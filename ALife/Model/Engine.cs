using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ALife.Model
{
    internal class Engine : INotifyPropertyChanged
    {
        private const int InitialRobotCount = 10;
        private const float WallCollisionLoss = 0.02f;
        private readonly Timer cyclePerSecondTimer;
        private readonly Random random = new Random();
        private int cyclesLastSecond = 0;
        private int cyclesPerSecond;
        private bool shouldStop = false;

        public Engine()
        {
            cyclePerSecondTimer = new Timer(CyclePerSecondTick, null, -1, 1000);

            Field.Size = new Vector2(1000, 1000);

            for (var i = 0; i < InitialRobotCount; i++)
            {
                Bots.Add(new Bot
                {
                    Position = new Vector2(
                        (float)random.NextDouble() * Field.Size.X,
                        (float)random.NextDouble() * Field.Size.Y),
                    Speed = new Vector2(
                        (float)random.NextDouble() * 10,
                        (float)random.NextDouble() * 10)
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Bot> Bots { get; } = new List<Bot>();

        public Func<List<Bot>, Task> CycleCallback { get; set; }

        public int CyclesPerSecond { get => cyclesPerSecond; private set { cyclesPerSecond = value; RaiseOnPropertyChanged(); } }

        public Field Field { get; } = new Field();

        public bool IsStopped { get; set; }

        public async Task RunCycle()
        {
            shouldStop = false;
            IsStopped = false;
            cyclePerSecondTimer.Change(0, 1000);

            while (!shouldStop)
            {
                Parallel.ForEach(Bots, b =>
                {
                    UpdateSpeed(b);
                    UpdatePosition(b);
                });

                cyclesLastSecond++;

                await CycleCallback(Bots);
            }

            cyclePerSecondTimer.Change(-1, 1000);
            IsStopped = true;
        }

        public void StopCycle()
        {
            shouldStop = true;
        }

        private void CyclePerSecondTick(object state)
        {
            CyclesPerSecond = cyclesLastSecond;
            cyclesLastSecond = 0;
        }

        private void RaiseOnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
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