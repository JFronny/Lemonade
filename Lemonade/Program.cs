using System;

namespace Lemonade
{
    class Program
    {
        static void Main(string[] args)
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