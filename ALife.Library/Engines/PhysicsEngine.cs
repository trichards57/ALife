using ALife.Model;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

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

        public void UpdateBot(IEnumerable<Bot> bots)
        {
            Parallel.ForEach(bots, bot =>
            {
                UpdateSpeed(bot);
                BotCollision(bot, bots);
                UpdatePosition(bot);
            });
        }

        private void BotCollision(Bot bot, IEnumerable<Bot> allBots)
        {
            if (bot.IsFixed)
                return;

            // TODO : This is currently loss-less
            foreach (var otherBot in allBots.Where(b => b != bot))
            {
                var distanceVector = bot.Position - otherBot.Position;
                var distanceSquared = distanceVector.LengthSquared();

                var speedVector = bot.Speed - otherBot.Speed;
                var relativeSpeedSquared = speedVector.LengthSquared();

                var sizes = bot.Radius + otherBot.Radius;
                if (distanceSquared > 0 && distanceSquared < sizes * sizes)
                {
                    var projection = Vector2.Dot(Vector2.Normalize(distanceVector), speedVector);
                    if (projection < 0)
                    {
                        var m1 = bot.Mass;
                        var m2 = otherBot.IsFixed ? 1e6f : otherBot.Mass;

                        var speedChange = (2 * m2 / (m1 + m2)) * (Vector2.Dot(speedVector, distanceVector) / distanceSquared) * distanceVector;
                        bot.Speed -= speedChange;
                    }
                }
            }
        }

        private void UpdatePosition(Bot bot)
        {
            if (bot.IsFixed)
                return;

            var radiusVector = new Vector2(bot.Radius / 2, bot.Radius / 2);
            bot.Position += bot.Speed;

            WallCollision(bot);

            bot.Position = Vector2.Clamp(bot.Position, radiusVector, field.Size - radiusVector);
        }

        private void UpdateSpeed(Bot bot)
        {
            bot.Speed += bot.Force / bot.Mass;

            bot.Force = Vector2.Zero;
        }

        private void WallCollision(Bot bot)
        {
            var wallForceX = 0.0f;
            var wallForceY = 0.0f;

            if (bot.Position.X <= bot.Radius / 2 || bot.Position.X >= field.Size.X - bot.Radius / 2)
                wallForceX = -(2 - WallCollisionLoss) * bot.Speed.X * bot.Mass;
            if (bot.Position.Y <= bot.Radius / 2 || bot.Position.Y >= field.Size.Y - bot.Radius / 2)
                wallForceY = -(2 - WallCollisionLoss) * bot.Speed.Y * bot.Mass;

            bot.Force += new Vector2(wallForceX, wallForceY);
        }
    }
}