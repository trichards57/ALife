using System;
using System.Collections.Generic;
using System.Linq;

namespace ALife.Model
{
    public static class SystemVariables
    {
        public static readonly int MemoryLength = Enum.GetValues(typeof(MemoryAddresses)).Cast<int>().Max() + 1;

        public static Dictionary<string, int> Variables { get; }
            = new Dictionary<string, int>
            {
                {"up", (int)MemoryAddresses.MoveUp },
                {"down", (int)MemoryAddresses.MoveDown },
                {"left", (int)MemoryAddresses.MoveLeft },
                {"right", (int)MemoryAddresses.MoveRight },
                {"turnleft", (int)MemoryAddresses.TurnLeft },
                {"turnright", (int)MemoryAddresses.TurnRight },
                {"speed", (int)MemoryAddresses.Speed },
                {"speedfwd", (int)MemoryAddresses.SpeedForward },
                {"speedright", (int)MemoryAddresses.SpeedRight },
                {"eye1", (int)MemoryAddresses.Eye1 },
                {"eye2", (int)MemoryAddresses.Eye2 },
                {"eye3", (int)MemoryAddresses.Eye3 },
                {"eye4", (int)MemoryAddresses.Eye4 },
                {"eye5", (int)MemoryAddresses.Eye5 },
                {"eye6", (int)MemoryAddresses.Eye6 },
                {"eye7", (int)MemoryAddresses.Eye7 },
                {"fbotdist", (int)MemoryAddresses.FocusBotDistance },
                {"fboteyes", (int)MemoryAddresses.FocusBotEyeRefCount },
                {"myeyes", (int)MemoryAddresses.MyEyeRefCount }
            };

        public static bool IsAddressWritable(int address)
        {
            switch ((MemoryAddresses)address)
            {
                case MemoryAddresses.MoveUp:
                case MemoryAddresses.MoveDown:
                case MemoryAddresses.MoveLeft:
                case MemoryAddresses.MoveRight:
                case MemoryAddresses.TurnLeft:
                case MemoryAddresses.TurnRight:
                    return true;

                default:
                    return false;
            }
        }

        public static int NormaliseAddress(int address)
        {
            if (address == 0)
                return 0;

            var actual = Math.Abs(address) % MemoryLength;

            if (actual == 0)
                return MemoryLength;
            return actual;
        }
    }
}