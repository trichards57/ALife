using ALife.Model;
using ALife.Serializer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace ALife.Engines
{
    public class SimulationEngine
    {
        private const int InitialRobotCount = 10;

        [ThreadStatic]
        private static DNAEngine dnaEngine;

        [ThreadStatic]
        private static PhysicsEngine physicsEngine;

        [ThreadStatic]
        private static RuntimeEngine runtimeEngine;

        private readonly List<Bot> botsToAdvertise = new List<Bot>();
        private readonly Timer cyclePerSecondTimer;
        private readonly Random random = new Random();
        private int cyclesLastSecond = 0;
        private int cyclesPerSecond;
        private bool shouldStop = false;

        public SimulationEngine()
        {
            cyclePerSecondTimer = new Timer(CyclePerSecondTick, null, -1, 1000);

            Field.Size = new Vector2(1000, 1000);

            for (var i = 0; i < InitialRobotCount; i++)
            {
                var bot = new Bot
                {
                    Position = new Vector2(
                                        (float)random.NextDouble() * Field.Size.X,
                                        (float)random.NextDouble() * Field.Size.Y),
                    Speed = new Vector2(0, 0),
                    DNA = DNASerializer.DeserializeDNA("test-dna.txt"),
                    Orientation = (float)(random.NextDouble() * Math.PI * 2),
                    Color = Color.Red
                };
                botsToAdvertise.Add(bot);
                Bots.Add(bot);
            }

            for (var i = 0; i < InitialRobotCount; i++)
            {
                var bot = new Bot
                {
                    Position = new Vector2(
                                    (float)random.NextDouble() * Field.Size.X,
                                    (float)random.NextDouble() * Field.Size.Y),
                    Speed = new Vector2(0, 0),
                    DNA = DNASerializer.DeserializeDNA("test-dna-2.txt"),
                    Orientation = (float)(random.NextDouble() * Math.PI * 2),
                    Color = Color.Green
                };
                botsToAdvertise.Add(bot);
                Bots.Add(bot);
            }
        }

        public event EventHandler<int> CyclePerSecondChanged;

        public Func<Bot, Task> AddBotCallback { get; set; }
        public List<Bot> Bots { get; } = new List<Bot>();

        public Func<List<Bot>, Task> CycleCallback { get; set; }

        public int CyclesPerSecond
        {
            get => cyclesPerSecond; private set
            {
                cyclesPerSecond = value;
                CyclePerSecondChanged?.Invoke(this, cyclesPerSecond);
            }
        }

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
                    if (dnaEngine == null)
                        dnaEngine = new DNAEngine();

                    dnaEngine.ExecuteDNA(b);
                });

                Parallel.ForEach(Bots, b =>
                {
                    if (runtimeEngine == null)
                        runtimeEngine = new RuntimeEngine();

                    runtimeEngine.UpdateBot(b);
                });

                Parallel.ForEach(Bots, b =>
                {
                    if (physicsEngine == null)
                        physicsEngine = new PhysicsEngine(Field);

                    physicsEngine.UpdateBot(b);
                });

                Parallel.ForEach(Bots, b =>
                {
                    if (runtimeEngine == null)
                        runtimeEngine = new RuntimeEngine();

                    runtimeEngine.UpdateMemory(b);
                });

                cyclesLastSecond++;

                if (AddBotCallback != null)
                {
                    foreach (var b in botsToAdvertise)
                        await AddBotCallback(b);
                }
                botsToAdvertise.Clear();
                if (CycleCallback != null)
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
    }
}