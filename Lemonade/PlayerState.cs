using System;

namespace Lemonade
{
    public class PlayerState
    {
        public int Budget;
        public int Earnings;
        public int Expenses;
        public int Glasses;
        public int GlassPrice;
        public int Number;
        public int Sales;
        public int Signs;

        public PlayerState(int number)
        {
            Budget = 200;
            Number = number;
        }

        public void CalculateIncome(int signCost, int glassCost, Weather weather, Settings settings)
        {
            Expenses = signCost * Signs + glassCost * Glasses;

            double weatherFactor =
                (weather.Factor * 2 + 1d) / (settings.DifficultyFactor * 2d + 1d) /
                2d; //calculate a scalar for sales between 0.2 and 1.5 based on the weather factor
            double signFactor =
                2.5d - (settings.DifficultyFactor + 0.1) * 2d /
                (Signs + 1d); //calculate a scalar between 0.3 and (basically) 2.5 based on the amount of signs
            double
                priceFactor =
                    3 / (GlassPrice / 2 +
                         1); //calculate a scalar between (basically) 0 and 3 based on the price of lemonades
            Sales = (int) Math.Max(Math.Min(weatherFactor * signFactor * priceFactor * Glasses, Glasses),
                0); //Multiply the factors and sanitize results
            Earnings = (int) Math.Floor((double) Sales * GlassPrice); //Convert result to integer

            Budget += Earnings - Expenses;
        }
    }
}