using System;
using System.Collections.Generic;
using System.Drawing;
using CC_Functions.Commandline;
using CC_Functions.Commandline.TUI;
using static System.Environment;

namespace Lemonade
{
    public class ResultScreen : CenteredScreen
    {
        public delegate void OkDelegate();

        private readonly Label lab;

        public ResultScreen(Settings settings) : base(200, 20, ConsoleColor.Black, settings.Color)
        {
            ContentPanel.ForeColor = ConsoleColor.DarkGray;
            Title = "Lemonade - Daily financial report";
            lab = new Label("");
            ContentPanel.Controls.Add(lab);
            Input += (screen, args) => Ok?.Invoke();
            Close += (screen, args) => Ok?.Invoke();
        }

        public event OkDelegate Ok;

        public void Setup(IEnumerable<PlayerState> players, Weather weather)
        {
            if (weather.Heatwave)
                lab.Content = "There was a heatwave";
            else if (weather.Thunderstorm)
                lab.Content = "Lightning struck your table while you were setting it up. Nothing was sold";
            else
                lab.Content = $"The weather was {weather}. Nothing out of the ordinary happened";
            lab.Content += NewLine + NewLine + players.ToStringTable(
                new[]
                {
                    "Player", "Glasses made", "Earnings per glass",
                    "Glasses sold", "Signs made", "Income",
                    "Expenses", "Profit", "Budget"
                },
                s => s.Number,
                s => s.Glasses,
                s => s.GlassPrice.ToDollar(),
                s => s.Sales,
                s => s.Signs,
                s => s.Earnings.ToDollar(),
                s => s.Expenses.ToDollar(),
                s => (s.Earnings - s.Expenses).ToDollar(),
                s => s.Budget.ToDollar());
            lab.Render();
            ActualSize = new Size(lab.Size.Width, lab.Size.Height);
        }
    }
}