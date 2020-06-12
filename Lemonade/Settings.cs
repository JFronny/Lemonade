using System;
using System.Drawing;
using System.Threading;
using CC_Functions.Commandline.TUI;

namespace Lemonade
{
    public class Settings
    {
        public bool Color { get; private set; } = true;
        public int PlayerCount { get; private set; }
        public float DifficultyFactor { get; private set; }
        public Settings()
        {
            Configure();
        }

        public void Configure()
        {
            CenteredScreen settingsScreen = new CenteredScreen(200, 20, ConsoleColor.Black, Color)
            {
                TabPoint = 0,
                Title = "Lemonade - Settings"
#if DEBUG
                                      + "[R to redraw]"
#endif
            };
            Panel scr = settingsScreen.ContentPanel;
            scr.ForeColor = ConsoleColor.DarkGray;
            
            Label playerLabel = new Label("Players");
            playerLabel.Point = new Point(scr.Size.Width / 2 - playerLabel.Content.Length / 2, 3);
            scr.Controls.Add(playerLabel);

            Slider playerSlider = new Slider {MinValue = 1, Value = 2, Size = new Size(100, 1)};
            playerSlider.Point = new Point(scr.Size.Width / 2 - playerSlider.Size.Width / 2, 4);
            scr.Controls.Add(playerSlider);
            
            Label difficultyLabel = new Label("Difficulty");
            difficultyLabel.Point = new Point(scr.Size.Width / 2 - difficultyLabel.Content.Length / 2, 7);
            scr.Controls.Add(difficultyLabel);

            Slider difficulty = new Slider {Value = 5, Size = new Size(100, 1)};
            difficulty.Point = new Point(scr.Size.Width / 2 - difficulty.Size.Width / 2, 8);
            scr.Controls.Add(difficulty);

            CheckBox colorBox = new CheckBox("Color") {Checked = true};
            colorBox.Point = new Point(scr.Size.Width / 2 - (colorBox.Content.Length + 4) / 2, 12);
            colorBox.CheckedChanged += (screen, args) =>
            {
                settingsScreen.Color = colorBox.Checked;
            };
#if DEBUG
            settingsScreen.Input += (screen, args) => { if (args.Info.Key == ConsoleKey.R) DiffDraw.Draw(Color, true); };
#endif
            scr.Controls.Add(colorBox);
            
            Button okButton = new Button("OK");
            okButton.Point = new Point(scr.Size.Width / 2 - okButton.Content.Length / 2, 16);
            scr.Controls.Add(okButton);
            
            bool visible = true;
            okButton.Click += (screen, args) => visible = false;
            settingsScreen.Close += (screen, args) => visible = false;
            
            settingsScreen.Render();
            while (visible)
            {
                settingsScreen.ReadInput();
                Thread.Sleep(100);
            }
            PlayerCount = playerSlider.Value;
            DifficultyFactor = difficulty.Value / 10f;
            Color = colorBox.Checked;
        }
    }
}