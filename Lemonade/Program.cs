using System;

namespace Lemonade
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Settings settings = new Settings();
            ScreenManager screenManager = new ScreenManager(settings);
            screenManager.Run();
            Console.ResetColor();
        }
    }
}