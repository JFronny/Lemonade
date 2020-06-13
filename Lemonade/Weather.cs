using System;

namespace Lemonade
{
    public class Weather
    {
        private static readonly Random rnd = new Random();

        public readonly bool Heatwave;
        public readonly bool Thunderstorm;

        public double Factor;

        public Weather(Settings settings)
        {
            Factor = rnd.NextDouble();
            Heatwave = W == W.Hot_and_dry && rnd.NextDouble() > settings.DifficultyFactor;
            Thunderstorm = W == W.Rainy && rnd.NextDouble() < settings.DifficultyFactor;
        }

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