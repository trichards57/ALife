using ALife.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ALife
{
    public class BotHolder
    {
        private readonly Ellipse ellipse;
        private readonly Line line;

        public BotHolder(Canvas canvas, Bot bot)
        {
            Bot = bot;
            ellipse = new Ellipse
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
            };
            line = new Line
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            canvas.Children.Add(ellipse);
            canvas.Children.Add(line);
        }

        public Bot Bot { get; }

        public void Update()
        {
            ellipse.Fill = new SolidColorBrush(Color.FromArgb(Bot.Color.A, Bot.Color.R, Bot.Color.G, Bot.Color.B));
            ellipse.Width = Bot.Radius * 2;
            ellipse.Height = Bot.Radius * 2;
            Canvas.SetLeft(ellipse, Bot.Position.X - Bot.Radius);
            Canvas.SetTop(ellipse, Bot.Position.Y - Bot.Radius);

            line.X1 = Bot.Position.X;
            line.Y1 = Bot.Position.Y;
            line.X2 = Bot.Position.X + Bot.Radius * Math.Cos(Bot.Orientation);
            line.Y2 = Bot.Position.Y + Bot.Radius * Math.Sin(Bot.Orientation);
        }
    }

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