using ALife.Model;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace ALife.App
{
    public class BotHolder
    {
        private readonly Ellipse ellipse;
        private readonly Line line;
        private readonly Path[] visionPath;

        public BotHolder(Canvas canvas, Bot bot)
        {
            Bot = bot;
            ellipse = new Ellipse
            {
                StrokeThickness = 1
            };
            ellipse.Tapped += BotClicked;

            visionPath = new Path[Bot.EyeCount];
            for (var i = 0; i < Bot.EyeCount; i++)
            {
                visionPath[i] = new Path
                {
                    StrokeThickness = 1,
                    Visibility = Visibility.Collapsed
                };
                canvas.Children.Add(visionPath[i]);
            }

            line = new Line
            {
                StrokeThickness = 2
            };

            canvas.Children.Add(ellipse);
            canvas.Children.Add(line);
        }

        public event EventHandler Clicked;

        public Bot Bot { get; }

        public SolidColorBrush Color => new SolidColorBrush(Windows.UI.Color.FromArgb(Bot.Color.A, Bot.Color.R, Bot.Color.G, Bot.Color.B));

        public bool IsSelected { get; set; }

        public void Update()
        {
            ellipse.Fill = Color;
            ellipse.Width = Bot.Radius * 2;
            ellipse.Height = Bot.Radius * 2;
            Canvas.SetLeft(ellipse, Bot.Position.X - Bot.Radius);
            Canvas.SetTop(ellipse, Bot.Position.Y - Bot.Radius);

            var outlineColor = Colors.Black;

            if (App.Current.RequestedTheme == ApplicationTheme.Dark)
                outlineColor = Colors.White;

            line.Stroke = new SolidColorBrush(outlineColor);

            if (IsSelected)
            {
                var col = GetColor(((Bot.Color.GetHue() + 180) % 360) / 360, Bot.Color.GetSaturation(), Bot.Color.GetBrightness());
                ellipse.Stroke = new SolidColorBrush(col);
                ellipse.StrokeThickness = 3;

                for (var i = 0; i < Bot.EyeCount; i++)
                {
                    visionPath[i].Visibility = Visibility.Visible;
                    visionPath[i].Stroke = new SolidColorBrush(col);
                    var visionRadius = Bot.VisionLimit - Bot.EyeDistances[i];

                    var eyePosition = i - Bot.EyeCount / 2;
                    var eyeStart = (eyePosition * Bot.EyeAngle * 2) - Bot.EyeAngle;
                    var eyeStop = (eyePosition * Bot.EyeAngle * 2) + Bot.EyeAngle;

                    var arcStartX = visionRadius * Math.Cos(Bot.Orientation + eyeStart);
                    var arcStartY = visionRadius * Math.Sin(Bot.Orientation + eyeStart);
                    var arcEndX = visionRadius * Math.Cos(Bot.Orientation + eyeStop);
                    var arcEndY = visionRadius * Math.Sin(Bot.Orientation + eyeStop);

                    visionPath[i].Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), $"M0,0 L{arcStartX},{arcStartY} A{visionRadius},{visionRadius} 0 0 1 {arcEndX},{arcEndY} z");
                    Canvas.SetLeft(visionPath[i], Bot.Position.X);
                    Canvas.SetTop(visionPath[i], Bot.Position.Y);
                }
            }
            else
            {
                ellipse.Stroke = new SolidColorBrush(outlineColor);
                ellipse.StrokeThickness = 2;
                foreach (var p in visionPath)
                    p.Visibility = Visibility.Collapsed;
            }

            line.X1 = Bot.Position.X;
            line.Y1 = Bot.Position.Y;
            line.X2 = Bot.Position.X + Bot.Radius * Math.Cos(Bot.Orientation);
            line.Y2 = Bot.Position.Y + Bot.Radius * Math.Sin(Bot.Orientation);
            line.IsTapEnabled = true;
        }

        private void BotClicked(object sender, TappedRoutedEventArgs e)
        {
            Clicked?.Invoke(this, EventArgs.Empty);
            e.Handled = true;
        }

        private Color GetColor(float h, float s, float l, float a = 1.0f)
        {
            double v;
            double r, g, b;

            if (a > 1.0)
                a = 1.0f;

            r = l;   // default to gray
            g = l;
            b = l;
            v = (l <= 0.5) ? (l * (1.0 + s)) : (l + s - l * s);

            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0f;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;

                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;

                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;

                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;

                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;

                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;

                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }

            return Windows.UI.Color.FromArgb((byte)(a * 255.0f), (byte)(r * 255.0f), (byte)(g * 255.0f), (byte)(b * 255.0f));
        }
    }
}
