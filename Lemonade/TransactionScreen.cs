using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using CC_Functions.Commandline.TUI;

namespace Lemonade
{
    public class TransactionScreen : CenteredScreen
    {
        public delegate void OkDelegate(int glasses, int price, int signs);

        private readonly Slider _glasses;
        private readonly Label _glassesLabel;

        private readonly Label _infoLabel;
        private readonly Label _infoLabelBottom;
        private readonly Slider _price;
        private readonly Label _priceLabel;
        private readonly Slider _signs;
        private readonly Label _signsLabel;
        private int _lemonadeCost;
        private PlayerState _player;
        private int _signCost;

        //TODO restore last transaction if budget allows
        public TransactionScreen(Settings set) : base(200, 20, ConsoleColor.Black, set.Color)
        {
            ContentPanel.ForeColor = ConsoleColor.DarkGray;

            _infoLabel = new Label("[DAY 1]");
            _infoLabel.Point = new Point(ContentPanel.Size.Width / 2 - _infoLabel.Content.Length / 2, 0);
            ContentPanel.Controls.Add(_infoLabel);

            _glassesLabel = new Label("How many glasses of lemonade do you wish to make?");
            _glassesLabel.Point = new Point(ContentPanel.Size.Width / 2 - _glassesLabel.Content.Length / 2, 2);
            ContentPanel.Controls.Add(_glassesLabel);

            _glasses = new Slider {Size = new Size(100, 1)};
            _glasses.Point = new Point(ContentPanel.Size.Width / 2 - _glasses.Size.Width / 2, 3);
            _glasses.ValueChanged += (screen, args) => CalculateMax();
            ContentPanel.Controls.Add(_glasses);

            _priceLabel = new Label("What price (in cents) do you wish to charge for lemonade?");
            _priceLabel.Point = new Point(ContentPanel.Size.Width / 2 - _priceLabel.Content.Length / 2, 6);
            ContentPanel.Controls.Add(_priceLabel);

            _price = new Slider {Size = new Size(100, 1)};
            _price.Point = new Point(ContentPanel.Size.Width / 2 - _price.Size.Width / 2, 7);
            _price.ValueChanged += (screen, args) => CalculateMax();
            ContentPanel.Controls.Add(_price);

            _signsLabel = new Label("How many advertising signs do you want to make?");
            _signsLabel.Point = new Point(ContentPanel.Size.Width / 2 - _signsLabel.Content.Length / 2, 10);
            ContentPanel.Controls.Add(_signsLabel);

            _signs = new Slider {Size = new Size(100, 1)};
            _signs.Point = new Point(ContentPanel.Size.Width / 2 - _signs.Size.Width / 2, 11);
            _signs.ValueChanged += (screen, args) => CalculateMax();
            ContentPanel.Controls.Add(_signs);

            _infoLabelBottom = new Label("Total Expenses: 0/0");
            _infoLabelBottom.Point = new Point(ContentPanel.Size.Width / 2 - _infoLabelBottom.Content.Length / 2, 14);
            ContentPanel.Controls.Add(_infoLabelBottom);

            Button okButton = new Button("OK");
            okButton.Point = new Point(ContentPanel.Size.Width / 2 - okButton.Size.Width / 2, 16);
            okButton.Click += (sender, e) => Ok?.Invoke(_glasses.Value, _price.Value, _signs.Value);
            ContentPanel.Controls.Add(okButton);

            Close += (screen, args) => Ok?.Invoke(_glasses.Value, _price.Value, _signs.Value);
        }

        public event OkDelegate Ok;

        public void SetUp(PlayerState player, Settings settings, int playerIndex, int day, Weather weather,
            int signCost, int lemonadeCost)
        {
            TabPoint = 0;
            _signCost = signCost;
            _lemonadeCost = lemonadeCost;
            Title = $"Lemonade - Player {playerIndex + 1}/{settings.PlayerCount}";
            _player = player;
            _glasses.Value = 0;
            _price.Value = 0;
            _signs.Value = 0;
            CalculateMax();
            _infoLabel.Content = $"It is day {day}, the weather is {weather}";
            if (weather.W == W.Hot_and_dry)
                _infoLabel.Content += ". There is a chance of heatwaves";
            else if (weather.W == W.Rainy)
                _infoLabel.Content += ". There is a chance of thunderstorms";
            _infoLabel.Point = new Point(ContentPanel.Size.Width / 2 - _infoLabel.Content.Length / 2, 0);
            _glassesLabel.Content = $"How many glasses of lemonade do you wish to make? {lemonadeCost}ct each";
            _glassesLabel.Point = new Point(ContentPanel.Size.Width / 2 - _glassesLabel.Content.Length / 2, 2);
            _signsLabel.Content = $"How many advertising signs ({signCost}ct each) do you want to make?";
            _signsLabel.Point = new Point(ContentPanel.Size.Width / 2 - _signsLabel.Content.Length / 2, 10);
        }

        private void CalculateMax()
        {
            int leftover = _player.Budget - CalculateExpenses();
            _glasses.MaxValue = (int) Math.Floor(leftover / (double) _lemonadeCost) + _glasses.Value;
            _price.MaxValue = 200;
            _signs.MaxValue = (int) Math.Floor(leftover / (double) _signCost) + _signs.Value;
            _infoLabelBottom.Content = $"Leftover: {leftover.ToDollar()}/{_player.Budget.ToDollar()}";
            _infoLabelBottom.Point = new Point(ContentPanel.Size.Width / 2 - _infoLabelBottom.Content.Length / 2, 14);
        }

        [Pure]
        private int CalculateExpenses() => _lemonadeCost * _glasses.Value + _signCost * _signs.Value;
    }
}