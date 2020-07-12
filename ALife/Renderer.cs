using ALife.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ALife
{
    public class Renderer
    {
        private readonly List<BotHolder> botHolders = new List<BotHolder>();
        private readonly Canvas canvas;

        public Renderer(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public async Task AddBot(Bot bot)
        {
            await canvas.Dispatcher.BeginInvoke(new Action<Bot>(RegisterBot), bot);
        }

        public async Task DrawBots(IEnumerable<Bot> bots)
        {
            await canvas.Dispatcher.BeginInvoke(new Action<IEnumerable<Bot>>(DrawBotsToCanvas), bots);
        }

        private void DrawBotsToCanvas(IEnumerable<Bot> bots)
        {
            foreach (var b in botHolders)
            {
                b.Update();
            }
        }

        private void RegisterBot(Bot bot)
        {
            botHolders.Add(new BotHolder(canvas, bot));
        }
    }
}