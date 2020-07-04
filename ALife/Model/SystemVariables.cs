using System;
using System.Collections.Generic;
using System.Linq;

namespace ALife.Model
{
    public static class SystemVariables
    {
        public static readonly int MemoryLength = Enum.GetValues(typeof(MemoryAddresses)).Cast<int>().Max();

        public static Dictionary<string, int> Variables { get; }
            = new Dictionary<string, int>
            {
                {"up", (int)MemoryAddresses.MoveUp },
                {"down", (int)MemoryAddresses.MoveDown },
                {"left", (int)MemoryAddresses.MoveLeft },
                {"right", (int)MemoryAddresses.MoveRight },
                {"turnleft", (int)MemoryAddresses.TurnLeft },
                {"turnright", (int)MemoryAddresses.TurnRight },
            };

        public static int NormaliseAddress(int address)
        {
            if (address == 0)
                return 0;

            var actual = (Math.Abs(address) + 1) % MemoryLength - 1;

            if (actual == 0)
                return MemoryLength;
            return actual;
        }
    }
}
