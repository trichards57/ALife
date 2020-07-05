using ALife.Engines;
using ALife.Model;
using System;
using System.Numerics;
using Xunit;

namespace ALife.Tests.Engines
{
    public class RuntimeEngineTests
    {
        [Fact]
        public void UpdateBotAppliesNetForce()
        {
            const int testUp = 5;
            const int testDown = 6;
            const int testLeft = 9;
            const int testRight = 12;

            var bot = new Bot
            {
                Orientation = 0,
                Speed = Vector2.Zero
            };

            bot.SetMemory(MemoryAddresses.MoveRight, testRight);
            bot.SetMemory(MemoryAddresses.MoveLeft, testLeft);
            bot.SetMemory(MemoryAddresses.MoveDown, testDown);
            bot.SetMemory(MemoryAddresses.MoveUp, testUp);

            var engine = new RuntimeEngine();
            engine.UpdateBot(bot);

            Assert.Equal(testUp - testDown, bot.Force.X);
            Assert.Equal(testRight - testLeft, bot.Force.Y);
        }

        [Fact]
        public void UpdateBotNormalisesOrientation()
        {
            const int turnLeft = 100;
            const int turnRight = 350;

            var bot = new Bot
            {
                Orientation = 0
            };

            bot.SetMemory(MemoryAddresses.TurnLeft, turnLeft);
            bot.SetMemory(MemoryAddresses.TurnRight, turnRight + 3000);

            var engine = new RuntimeEngine();
            engine.UpdateBot(bot);

            Assert.Equal(Helpers.IntToAngle(turnRight - turnLeft), bot.Orientation, 3);
        }

        [Fact]
        public void UpdateBotRotatesNetForce()
        {
            const int testX = 10;
            const float testAngle = (float)Math.PI / 3;

            var bot = new Bot
            {
                Orientation = testAngle,
                Speed = Vector2.Zero
            };

            bot.SetMemory(MemoryAddresses.MoveUp, testX);

            var engine = new RuntimeEngine();
            engine.UpdateBot(bot);

            Assert.Equal(testX * Math.Cos(testAngle), bot.Force.X, 3);
            Assert.Equal(testX * Math.Sin(testAngle), bot.Force.Y, 3);
        }

        [Fact]
        public void UpdateBotSetsRotation()
        {
            const int turnLeft = 100;
            const int turnRight = 350;

            var bot = new Bot
            {
                Orientation = 0
            };

            bot.SetMemory(MemoryAddresses.TurnLeft, turnLeft);
            bot.SetMemory(MemoryAddresses.TurnRight, turnRight);

            var engine = new RuntimeEngine();
            engine.UpdateBot(bot);

            Assert.Equal(Helpers.IntToAngle(turnRight - turnLeft), bot.Orientation, 3);
        }

        [Fact]
        public void UpdateMemoryClearsUserMemory()
        {
            var bot = new Bot();
            bot.SetMemory(MemoryAddresses.MoveDown, 1);
            bot.SetMemory(MemoryAddresses.MoveUp, 2);
            bot.SetMemory(MemoryAddresses.MoveLeft, 3);
            bot.SetMemory(MemoryAddresses.MoveRight, 4);
            bot.SetMemory(MemoryAddresses.TurnLeft, 5);
            bot.SetMemory(MemoryAddresses.TurnRight, 6);

            var engine = new RuntimeEngine();
            engine.UpdateMemory(bot);

            Assert.Equal(0, bot.GetFromMemory(MemoryAddresses.MoveDown));
            Assert.Equal(0, bot.GetFromMemory(MemoryAddresses.MoveUp));
            Assert.Equal(0, bot.GetFromMemory(MemoryAddresses.MoveLeft));
            Assert.Equal(0, bot.GetFromMemory(MemoryAddresses.MoveRight));
            Assert.Equal(0, bot.GetFromMemory(MemoryAddresses.TurnLeft));
            Assert.Equal(0, bot.GetFromMemory(MemoryAddresses.TurnRight));
        }

        [Fact]
        public void UpdateMemoryTransformsSpeed()
        {
            const int testX = 10;
            const float testAngle = (float)Math.PI / 3;

            var bot = new Bot()
            {
                Orientation = testAngle,
                Speed = new Vector2(testX, 0)
            };

            var engine = new RuntimeEngine();
            engine.UpdateMemory(bot);

            Assert.Equal(testX * Math.Cos(testAngle), bot.GetFromMemory(MemoryAddresses.SpeedForward), 0);
            Assert.Equal(-testX * Math.Sin(testAngle), bot.GetFromMemory(MemoryAddresses.SpeedRight), 0);
        }

        [Fact]
        public void UpdateMemoryUpdatesStateMemory()
        {
            var bot = new Bot()
            {
                Orientation = 0,
                Speed = new Vector2(1, 2)
            };

            var engine = new RuntimeEngine();
            engine.UpdateMemory(bot);

            Assert.Equal(1, bot.GetFromMemory(MemoryAddresses.SpeedForward));
            Assert.Equal(2, bot.GetFromMemory(MemoryAddresses.SpeedRight));
        }
    }
}