using ALife.Model;
using System.Numerics;

namespace ALife.Engines
{
    internal class RuntimeEngine
    {
        public void UpdateBot(Bot bot)
        {
            bot.Orientation = Helpers.NormaliseAngle(bot.Orientation + Helpers.IntToAngle(bot.Memory[(int)MemoryAddresses.TurnRight] - bot.Memory[(int)MemoryAddresses.TurnLeft]));

            var netForce = new Vector2(
                bot.Memory[(int)MemoryAddresses.MoveUp] - bot.Memory[(int)MemoryAddresses.MoveDown],
                bot.Memory[(int)MemoryAddresses.MoveLeft] - bot.Memory[(int)MemoryAddresses.MoveRight]);

            var rotation = Quaternion.Normalize(new Quaternion(0, 0, 1, bot.Orientation));

            bot.Force += Vector2.Transform(netForce, rotation);
        }
    }
}
