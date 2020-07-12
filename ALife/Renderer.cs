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
        private BotHolder selectedBot;

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

        private void BotClicked(object sender, EventArgs e)
        {
            if (sender is BotHolder bot && bot != selectedBot)
            {
                if (selectedBot != null)
                {
                    selectedBot.IsSelected = false;
                    selectedBot.Update();
                }

                selectedBot = bot;
                selectedBot.IsSelected = true;
                selectedBot.Update();
            }
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
            var bh = new BotHolder(canvas, bot);
            bh.Clicked += BotClicked;
            botHolders.Add(bh);
        }
    }
}