using System;

namespace Lemonade
{
    public class Weather
    {
        private static readonly Random rnd = new Random();

        public double Factor = rnd.NextDouble();

        public W W
        {
            get
            {
                if (Factor < 0.2) return W.Rainy;
                if (Factor < 0.4) return W.Cloudy;
                if (Factor < 0.6) return W.Warm;
                if (Factor < 0.8) return W.Sunny;
                return W.Hot_and_dry;
            }
        }

        public override string ToString() => W.ToString().Replace('_', ' ');
    }

    public enum W
    {
        Rainy,
        Cloudy,
        Warm,
        Sunny,
        Hot_and_dry
    }
}