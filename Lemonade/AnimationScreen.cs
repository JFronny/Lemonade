using System;
using System.Drawing;
using CC_Functions.Commandline.TUI;
using CC_Functions.Misc;
using static System.ConsoleColor;
using static Lemonade.W;

namespace Lemonade
{
    public class AnimationScreen : CenteredScreen
    {
        public delegate void OkDelegate();

        private readonly Image imgControl;

        public AnimationScreen(Settings settings) : base(100, 20, Black, settings.Color)
        {
            ContentPanel.ForeColor = DarkGray;
            Title = "Lemonade - Anim";
            imgControl = new Image(new Pixel[0, 0]);
            SetWeather(new Weather(settings));
            ContentPanel.Controls.Add(imgControl);
            Input += (screen, args) => Ok?.Invoke();
            Close += (screen, args) => Ok?.Invoke();
        }

        public event OkDelegate Ok;

        public void SetWeather(Weather w)
        {
            Title = $"Lemonade - Weather: {w}";
            Pixel sky = new Pixel(w.W switch
            {
                Rainy => Black,
                Cloudy => DarkBlue,
                Warm => Blue,
                Sunny => Blue,
                Hot_and_dry => Magenta,
                _ => throw new ArgumentOutOfRangeException()
            }, Black, ' ');
            Pixel grass = new Pixel(w.W switch
            {
                Rainy => Green,
                Cloudy => Green,
                Warm => Green,
                Sunny => Green,
                Hot_and_dry => Green,
                _ => throw new ArgumentOutOfRangeException()
            }, Black, ' ');
            Pixel table = new Pixel(DarkGreen, Black, ' ');
            Pixel glass = new Pixel(Yellow, Black, ' ');
            Pixel sun = new Pixel(w.W switch
            {
                Rainy => Yellow,
                Cloudy => Yellow,
                Warm => Yellow,
                Sunny => Yellow,
                Hot_and_dry => Red,
                _ => throw new ArgumentOutOfRangeException()
            }, Black, ' ');
            Pixel cloud = new Pixel(Gray, Black, ' ');
            Pixel rain = new Pixel(DarkBlue, Black, ' ');
            Pixel[,] image = new Pixel[20, 200];
            //Draw sky
            image.Populate(sky);
            //Draw rain
            Random rainRng = new Random(100);
            if (w.W == Rainy)
                for (int i = 2; i < 12; i++)
                for (int j = 0; j < 200; j++)
                    if (rainRng.NextDouble() < 0.1)
                    {
                        image[i, j] = rain;
                        image[i + 1, j] = rain;
                    }
            //Draw floor
            for (int i = 13; i < 20; i++)
            for (int j = 0; j < 200; j++)
                image[i, j] = grass;
            //Draw table
            for (int i = 12; i < 16; i++)
            for (int j = 40; j < 60; j++)
                image[i, j] = table;
            //Draw glasses
            for (int j = 43; j < 57; j += 4)
            {
                image[11, j] = glass;
                image[11, j + 1] = glass;
            }
            //Draw sun
            if (w.W != Rainy && w.W != Cloudy)
                image.DrawEllipse(new Rectangle(2, 1, 14, 6), sun);
            //Draw clouds
            if (w.W == Rainy || w.W == Cloudy)
            {
                image.DrawEllipse(new Rectangle(8, 1, 25, 3), cloud);
                image.DrawEllipse(new Rectangle(40, 3, 40, 3), cloud);
            }
            //Set image
            imgControl.Img = image;
        }
    }
}