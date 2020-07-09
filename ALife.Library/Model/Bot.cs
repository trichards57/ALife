using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace ALife.Model
{
    public class Bot
    {
        public Color Color { get; set; }
        public List<BasePair> DNA { get; set; }
        public Vector2 Force { get; set; }
        public float Mass { get; set; } = 10;
        public IList<int> Memory { get; } = new int[SystemVariables.MemoryLength];
        public float Orientation { get; set; }
        public Vector2 Position { get; set; }
        public IReadOnlyList<int> PreviousMemory { get; private set; }
        public float Radius { get; set; } = 10;
        public Vector2 Speed { get; set; }

        public int GetFromMemory(MemoryAddresses address)
        {
            return Memory[(int)address];
        }

        public void SetMemory(MemoryAddresses address, int value)
        {
            Memory[(int)address] = value;
        }

        public void SetMemory(MemoryAddresses address, float value)
        {
            Memory[(int)address] = (int)Math.Round(value);
        }
    }
}