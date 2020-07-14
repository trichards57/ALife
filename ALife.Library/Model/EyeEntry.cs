using ALife.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALife.Library.Model
{
    public class EyeEntry
    {
        public EyeEntry(Bot otherBot, float distance)
        {
            OtherBot = otherBot;
            Distance = distance;
        }

        public float Distance { get; }
        public Bot OtherBot { get; }
    }
}
