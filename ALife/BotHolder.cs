using ALife.Model;
using System;
using System.ComponentModel;
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

        public event PropertyChangedEventHandler PropertyChanged;

        public Bot Bot { get; }

        public SolidColorBrush Color => new SolidColorBrush(System.Windows.Media.Color.FromArgb(Bot.Color.A, Bot.Color.R, Bot.Color.G, Bot.Color.B));

        public void Update()
        {
            ellipse.Fill = Color;
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
}