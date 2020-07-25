using ALife.Engines;
using ALife.Model;
using FluentAssertions;
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

            RuntimeEngine.UpdateBot(bot);

            bot.Force.X.Should().Be(testUp - testDown);
            bot.Force.Y.Should().Be(testRight - testLeft);
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

            RuntimeEngine.UpdateBot(bot);

            bot.Orientation.Should().BeApproximately(Helpers.IntToAngle(turnRight - turnLeft), 3);
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

            RuntimeEngine.UpdateBot(bot);

            bot.Force.X.Should().BeApproximately(testX * (float)Math.Cos(testAngle), 3);
            bot.Force.Y.Should().BeApproximately(testX * (float)Math.Sin(testAngle), 3);
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

            RuntimeEngine.UpdateBot(bot);

            bot.Orientation.Should().BeApproximately(Helpers.IntToAngle(turnRight - turnLeft), 3);
        }

        [Fact]
        public void UpdateMemoryClearsFocusBotMemoryWhenNone()
        {
            var bot = new Bot
            {
                FocussedBot = null
            };

            bot.GetFromMemory(MemoryAddresses.FocusBotDistance).Should().Be(0);
            bot.GetFromMemory(MemoryAddresses.FocusBotSpeed).Should().Be(0);
            bot.GetFromMemory(MemoryAddresses.FocusBotSpeedForward).Should().Be(0);
            bot.GetFromMemory(MemoryAddresses.FocusBotSpeedRight).Should().Be(0);
            bot.GetFromMemory(MemoryAddresses.FocusBotEyeRefCount).Should().Be(0);
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

            RuntimeEngine.UpdateMemory(bot);

            bot.GetFromMemory(MemoryAddresses.MoveDown).Should().Be(0);
            bot.GetFromMemory(MemoryAddresses.MoveUp).Should().Be(0);
            bot.GetFromMemory(MemoryAddresses.MoveLeft).Should().Be(0);
            bot.GetFromMemory(MemoryAddresses.MoveRight).Should().Be(0);
            bot.GetFromMemory(MemoryAddresses.TurnLeft).Should().Be(0);
            bot.GetFromMemory(MemoryAddresses.TurnRight).Should().Be(0);
        }

        [Fact]
        public void UpdateMemorySetFocusBotMemoryWhenPresent()
        {
            const int TestForward = 3;
            const int TestRight = 4;

            var bot = new Bot
            {
                Orientation = 0,
                FocussedBot = new Bot
                {
                    Speed = new Vector2(TestForward, TestRight),
                    DNA = new[] { new BasePair(BasePairType.StarNumber, (int)MemoryAddresses.Eye1) }
                }
            };

            RuntimeEngine.UpdateMemory(bot);

            bot.GetFromMemory(MemoryAddresses.FocusBotSpeed).Should().Be((int)Math.Round(bot.FocussedBotRelativeSpeed.Length()));
            bot.GetFromMemory(MemoryAddresses.FocusBotSpeedForward).Should().Be((int)Math.Round(bot.FocussedBotRelativeSpeed.X));
            bot.GetFromMemory(MemoryAddresses.FocusBotSpeedRight).Should().Be((int)Math.Round(bot.FocussedBotRelativeSpeed.Y));
            bot.GetFromMemory(MemoryAddresses.FocusBotEyeRefCount).Should().Be(1);
        }

        [Fact]
        public void UpdateMemorySetsBotEyes()
        {
            var testArray = new float[Bot.EyeCount];

            for (var i = 0; i < Bot.EyeCount; i++)
                testArray[i] = i * 3.2f;

            var bot = new Bot
            {
                EyeDistances = testArray
            };

            RuntimeEngine.UpdateMemory(bot);

            for (var i = 0; i < Bot.EyeCount; i++)
                bot.GetFromMemory(MemoryAddresses.EyeFirst, i).Should().Be((int)Math.Round(testArray[i]));
        }

        [Fact]
        public void UpdateMemorySetsEyeRefCount()
        {
            var bot = new Bot
            {
                DNA = new[] {
                    new BasePair(BasePairType.StarNumber, (int)MemoryAddresses.Eye1),
                    new BasePair(BasePairType.StarNumber, (int)MemoryAddresses.Eye2),
                    new BasePair(BasePairType.StarNumber, (int)MemoryAddresses.Eye3),
                    new BasePair(BasePairType.StarNumber, (int)MemoryAddresses.Eye4),
                    new BasePair(BasePairType.StarNumber, (int)MemoryAddresses.Eye5),
                    new BasePair(BasePairType.StarNumber, (int)MemoryAddresses.Eye6),
                    new BasePair(BasePairType.StarNumber, (int)MemoryAddresses.Eye7),
                    new BasePair(BasePairType.Number, (int)MemoryAddresses.Eye1),
                    new BasePair(BasePairType.Basic, (int)MemoryAddresses.Eye2),
                    new BasePair(BasePairType.Boolean, (int)MemoryAddresses.Eye3),
                    new BasePair(BasePairType.Condition, (int)MemoryAddresses.Eye4),
                    new BasePair(BasePairType.Flow, (int)MemoryAddresses.Eye5),
                    new BasePair(BasePairType.Store, (int)MemoryAddresses.Eye6),
                    new BasePair(BasePairType.Unknown, (int)MemoryAddresses.Eye7),
                }
            };

            RuntimeEngine.UpdateMemory(bot);

            bot.GetFromMemory(MemoryAddresses.MyEyeRefCount).Should().Be(7);
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

            RuntimeEngine.UpdateMemory(bot);

            bot.GetFromMemory(MemoryAddresses.SpeedForward).Should().Be((int)Math.Round(testX * Math.Cos(testAngle)));
            bot.GetFromMemory(MemoryAddresses.SpeedRight).Should().Be((int)Math.Round(-testX * Math.Sin(testAngle)));
        }

        [Fact]
        public void UpdateMemoryUpdatesStateMemory()
        {
            const int TestForward = 3;
            const int TestRight = 4;
            const int TestSpeed = 5;

            var bot = new Bot()
            {
                Orientation = 0,
                Speed = new Vector2(TestForward, TestRight)
            };

            RuntimeEngine.UpdateMemory(bot);
            bot.GetFromMemory(MemoryAddresses.Speed).Should().Be(TestSpeed);
            bot.GetFromMemory(MemoryAddresses.SpeedRight).Should().Be(TestRight);
            bot.GetFromMemory(MemoryAddresses.SpeedForward).Should().Be(TestForward);
            for (var i = 0; i < Bot.EyeCount; i++)
                bot.GetFromMemory(MemoryAddresses.EyeFirst, i).Should().Be(0);
            bot.GetFromMemory(MemoryAddresses.FocusBotDistance).Should().Be(0);
            bot.GetFromMemory(MemoryAddresses.MyEyeRefCount).Should().Be(0);
        }
    }
}