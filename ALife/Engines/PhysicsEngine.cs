using ALife.Model;
using System.Numerics;

namespace ALife.Engines
{
    public class PhysicsEngine
    {
        private const float WallCollisionLoss = 0.02f;
        private readonly Field field;

        public PhysicsEngine(Field field)
        {
            this.field = field;
        }

        public void UpdateBot(Bot bot)
        {
            UpdateSpeed(bot);
            UpdatePosition(bot);
        }

        private void UpdatePosition(Bot bot)
        {
            var radiusVector = new Vector2(bot.Radius / 2, bot.Radius / 2);
            bot.Position = bot.Speed + bot.Position;

            var wallForceX = 0.0f;
            var wallForceY = 0.0f;

            if (bot.Position.X <= bot.Radius / 2 || bot.Position.X >= field.Size.X - bot.Radius / 2)
                wallForceX = -(2 - WallCollisionLoss) * bot.Speed.X * bot.Mass;
            if (bot.Position.Y <= bot.Radius / 2 || bot.Position.Y >= field.Size.Y - bot.Radius / 2)
                wallForceY = -(2 - WallCollisionLoss) * bot.Speed.Y * bot.Mass;

            bot.Force += new Vector2(wallForceX, wallForceY);

            bot.Position = Vector2.Clamp(bot.Position, radiusVector, field.Size - radiusVector);
        }

        private void UpdateSpeed(Bot bot)
        {
            bot.Speed += bot.Force / bot.Mass;

            bot.Force = Vector2.Zero;
        }
    }
}