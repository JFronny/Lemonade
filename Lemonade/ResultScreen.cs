using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using CC_Functions.Commandline;
using CC_Functions.Commandline.TUI;

namespace Lemonade
{
    public class ResultScreen : CenteredScreen
    {
        public event OkDelegate Ok;
        public delegate void OkDelegate();
        private Label lab;
        public ResultScreen(Settings settings) : base(200, 20, ConsoleColor.Black, settings.Color)
        {
            ContentPanel.ForeColor = ConsoleColor.DarkGray;
            Title = "Lemonade - Daily financial report";
            lab = new Label("");
            ContentPanel.Controls.Add(lab);
            Input += (screen, args) => Ok?.Invoke();
            Close += (screen, args) => Ok?.Invoke();
        }

        public void Setup(IEnumerable<PlayerState> players)
        {
            lab.Content = players.ToStringTable(new[] {"Player", "Glasses made", "Earnings per glass", "Glasses sold", "Signs made", "Income", "Expenses", "Profit", "Budget"},
            (s) => s.Number,
            (s) => s.Glasses,
            (s) => s.GlassPrice,
            (s) => s.Sales,
            (s) => s.Signs,
            (s) => s.Earnings,
            (s) => s.Expenses,
            (s) => s.Earnings - s.Expenses,
            (s) => s.Budget);
            lab.Render();
            ActualSize = new Size(lab.Size.Width, lab.Size.Height);
        }
    }
}