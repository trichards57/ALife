﻿using ALife.Model;
using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
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
            ellipse.MouseLeftButtonUp += BotClicked;

            line = new Line
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            canvas.Children.Add(ellipse);
            canvas.Children.Add(line);
        }

        public event EventHandler Clicked;

        public event PropertyChangedEventHandler PropertyChanged;

        public Bot Bot { get; }

        public SolidColorBrush Color => new SolidColorBrush(System.Windows.Media.Color.FromArgb(Bot.Color.A, Bot.Color.R, Bot.Color.G, Bot.Color.B));

        public bool IsSelected { get; set; }

        public void Update()
        {
            ellipse.Fill = Color;
            ellipse.Width = Bot.Radius * 2;
            ellipse.Height = Bot.Radius * 2;
            Canvas.SetLeft(ellipse, Bot.Position.X - Bot.Radius);
            Canvas.SetTop(ellipse, Bot.Position.Y - Bot.Radius);

            if (IsSelected)
            {
                var col = GetColor(((Bot.Color.GetHue() + 180) % 360) / 360, Bot.Color.GetSaturation(), Bot.Color.GetBrightness());
                ellipse.Stroke = new SolidColorBrush(col);
                ellipse.StrokeThickness = 3;
            }
            else
            {
                ellipse.Stroke = Brushes.Black;
                ellipse.StrokeThickness = 2;
            }

            line.X1 = Bot.Position.X;
            line.Y1 = Bot.Position.Y;
            line.X2 = Bot.Position.X + Bot.Radius * Math.Cos(Bot.Orientation);
            line.Y2 = Bot.Position.Y + Bot.Radius * Math.Sin(Bot.Orientation);
        }

        private void BotClicked(object sender, MouseButtonEventArgs e)
        {
            Clicked?.Invoke(this, EventArgs.Empty);
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

            return System.Windows.Media.Color.FromArgb((byte)(a * 255.0f), (byte)(r * 255.0f), (byte)(g * 255.0f), (byte)(b * 255.0f));
        }
    }
}