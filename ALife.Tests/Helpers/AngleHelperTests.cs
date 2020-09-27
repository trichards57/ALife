using ALife.Helpers;
using FluentAssertions;
using System;
using Xunit;

namespace ALife.Tests.Helpers
{
    public class AngleHelperTests
    {
        private const float AngleTolerance = 0.005f;

        [Fact]
        public void AngleToIntScalesCorrectly()
        {
            const float FullAngle = 2 * (float)Math.PI;
            const int FullAngleInt = 1000;

            var testAngle1 = AngleHelper.AngleToInt(FullAngle);
            var testAngle2 = AngleHelper.AngleToInt(FullAngle / 2);
            var testAngle3 = AngleHelper.AngleToInt(FullAngle / 3);

            testAngle1.Should().Be(FullAngleInt);
            testAngle2.Should().Be(FullAngleInt / 2);
            testAngle3.Should().Be(FullAngleInt / 3);
        }

        [Fact]
        public void IntToAngleScalesCorrectly()
        {
            const float FullAngle = 2 * (float)Math.PI;
            const int FullAngleInt = 1000;

            var testAngle1 = AngleHelper.IntToAngle(FullAngleInt);
            var testAngle2 = AngleHelper.IntToAngle(FullAngleInt / 2);
            var testAngle3 = AngleHelper.IntToAngle(FullAngleInt / 3);

            testAngle1.Should().BeApproximately(FullAngle, AngleTolerance);
            testAngle2.Should().BeApproximately(FullAngle / 2, AngleTolerance);
            testAngle3.Should().BeApproximately(FullAngle / 3, AngleTolerance);
        }

        [Fact]
        public void NormaliseAngleIncreasesVeryNegativeAngle()
        {
            const float TestAngle = 2.0f;

            var result = AngleHelper.NormaliseAngle(TestAngle - (float)Math.PI * 6);

            result.Should().BeApproximately(TestAngle, AngleTolerance);
        }

        [Fact]
        public void NormaliseAngleLeavesSmallAngle()
        {
            const float TestAngle = 2.0f;

            var result = AngleHelper.NormaliseAngle(TestAngle);

            result.Should().BeApproximately(TestAngle, AngleTolerance);
        }

        [Fact]
        public void NormaliseAngleReducesVeryPositiveAngle()
        {
            const float TestAngle = 2.0f;

            var result = AngleHelper.NormaliseAngle(TestAngle + (float)Math.PI * 6);

            result.Should().BeApproximately(TestAngle, AngleTolerance);
        }
    }
}