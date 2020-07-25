using ALife.Model;
using FluentAssertions;
using System;
using System.Numerics;
using Xunit;

namespace ALife.Tests.Model
{
    public class BotTests
    {
        [Fact]
        public void CombinedRelativeSpeedIsCalculatedCorrectly()
        {
            const float Bot1X = 3;
            const float Bot1Y = 4;
            const float Bot2X = 5;
            const float Bot2Y = 6;

            var bot = new Bot
            {
                Orientation = 0,
                Position = new Vector2(0, 0),
                Speed = new Vector2(Bot1X, Bot1Y),
                FocussedBot = new Bot
                {
                    Orientation = (float)Math.PI,
                    Position = new Vector2(10, 0),
                    Speed = new Vector2(-Bot2X, -Bot2Y)
                }
            };

            bot.FocussedBotRelativeSpeed.X.Should().Be(-(Bot1X + Bot2X));
            bot.FocussedBotRelativeSpeed.Y.Should().Be(-(Bot1Y + Bot2Y));
        }

        [Fact]
        public void MovingDiagonalyAwayRelativeSpeedIsCalculatedCorrectly()
        {
            const float TestSpeed = 5;
            var RootDoubleSquareSpeed = (float)Math.Sqrt(2 * TestSpeed * TestSpeed);

            var bot = new Bot
            {
                Orientation = (float)Math.PI / 4,
                Position = new Vector2(0, 0),
                Speed = new Vector2(0, 0),
                FocussedBot = new Bot
                {
                    Orientation = 0,
                    Position = new Vector2(10, 10),
                    Speed = new Vector2(TestSpeed, TestSpeed)
                }
            };

            bot.FocussedBotRelativeSpeed.X.Should().Be(RootDoubleSquareSpeed);
            bot.FocussedBotRelativeSpeed.Y.Should().Be(0);
        }

        [Fact]
        public void MovingForwardAwayRelativeSpeedIsCalculatedCorrectly()
        {
            const float TestSpeed = 5;

            var bot = new Bot
            {
                Orientation = 0,
                Position = new Vector2(0, 0),
                Speed = new Vector2(0, 0),
                FocussedBot = new Bot
                {
                    Orientation = 0,
                    Position = new Vector2(10, 0),
                    Speed = new Vector2(TestSpeed, 0)
                }
            };

            bot.FocussedBotRelativeSpeed.X.Should().Be(TestSpeed);
            bot.FocussedBotRelativeSpeed.Y.Should().Be(0);
        }

        [Fact]
        public void MovingRightAwayRelativeSpeedIsCalculatedCorrectly()
        {
            const float TestSpeed = 5;

            var bot = new Bot
            {
                Orientation = -(float)Math.PI / 2,
                Position = new Vector2(0, 0),
                Speed = new Vector2(0, 0),
                FocussedBot = new Bot
                {
                    Orientation = 0,
                    Position = new Vector2(0, 10),
                    Speed = new Vector2(TestSpeed, 0)
                }
            };

            bot.FocussedBotRelativeSpeed.X.Should().Be(0);
            bot.FocussedBotRelativeSpeed.Y.Should().Be(TestSpeed);
        }
    }
}