using System;
using System.Collections.Generic;
using System.Linq;

namespace ALife.Model
{
    public enum MemoryAddresses
    {
        MoveUp = 1,
        MoveDown = 2,
        MoveLeft = 3,
        MoveRight = 4,
        TurnLeft = 5,
        TurnRight = 6,
    }

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
    }
}