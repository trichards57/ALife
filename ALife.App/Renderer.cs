using ALife.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace ALife.App
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

        public event EventHandler SelectedBotChanged;

        public BotHolder SelectedBot
        {
            get => selectedBot; set
            {
                selectedBot = value;
                var _ = canvas.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => SelectedBotChanged?.Invoke(this, EventArgs.Empty));
            }
        }

        public async Task AddBot(Bot bot)
        {
            await canvas.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => RegisterBot(bot));
        }

        public async Task DrawBots(IEnumerable<Bot> bots)
        {
            await canvas.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => DrawBotsToCanvas(bots));
        }

        private void BotClicked(object sender, EventArgs e)
        {
            if (sender is BotHolder bot && bot != SelectedBot)
            {
                if (SelectedBot != null)
                {
                    SelectedBot.IsSelected = false;
                    SelectedBot.Update();
                }

                SelectedBot = bot;
                SelectedBot.IsSelected = true;
                SelectedBot.Update();
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