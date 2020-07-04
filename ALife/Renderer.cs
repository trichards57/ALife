using ALife.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ALife
{
    internal class Renderer
    {
        private readonly Canvas canvas;

        public Renderer(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public async Task DrawBots(IEnumerable<Bot> bots)
        {
            await canvas.Dispatcher.BeginInvoke(new Action<IEnumerable<Bot>>(DrawBotsToCanvas), bots);
        }

        private void DrawBotsToCanvas(IEnumerable<Bot> bots)
        {
            foreach (var b in bots)
            {
                if (b.Ellipse == null)
                {
                    b.Ellipse = new Ellipse
                    {
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                    };

                    canvas.Children.Add(b.Ellipse);
                }

                b.Ellipse.Fill = new SolidColorBrush(b.Color);
                b.Ellipse.Width = b.Radius * 2;
                b.Ellipse.Height = b.Radius * 2;
                Canvas.SetLeft(b.Ellipse, b.Position.X - b.Radius);
                Canvas.SetTop(b.Ellipse, b.Position.Y - b.Radius);
            }
        }
    }
}