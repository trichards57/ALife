using ALife.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ALife.Tests.Model
{
    public class SystemVariablesTests
    {
        [Fact]
        public void NormaliseAddressHandlesLargeAddress()
        {
            var testAddress = 1;

            var actualAddress = SystemVariables.NormaliseAddress(testAddress + SystemVariables.MemoryLength * 20);

            Assert.Equal(testAddress, actualAddress);
        }

        [Fact]
        public void NormaliseAddressHandlesLargestValue()
        {
            var testAddress = SystemVariables.MemoryLength;

            var actualAddress = SystemVariables.NormaliseAddress(testAddress);

            Assert.Equal(testAddress, actualAddress);
        }

        [Fact]
        public void NormaliseAddressHandlesSmallAddress()
        {
            var testAddress = 2;

            var actualAddress = SystemVariables.NormaliseAddress(testAddress);

            Assert.Equal(testAddress, actualAddress);
        }

        [Fact]
        public void NormaliseAddressHandlesZero()
        {
            var testAddress = 0;

            var actualAddress = SystemVariables.NormaliseAddress(testAddress);

            Assert.Equal(testAddress, actualAddress);
        }
    }
}
