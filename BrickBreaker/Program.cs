using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;

namespace BrickBreaker
{
    internal class Program
    {

        static int score = 0;
        //public static Brick[,] bricks;
        static void Main(string[] args)
        {
            Console.Clear();
            ReadGameFile readGameFile = new ReadGameFile();
            var setting = readGameFile.ReadSettingFile();
            Console.SetCursorPosition(0, 0);
            if (string.IsNullOrEmpty(setting.PreferredName))
            {
                Console.WriteLine("Please enter your Name");
                var name = Console.ReadLine();
                if(name.Length<=0)
                {
                    Main(args);
                }
                setting.PreferredName = name;
                readGameFile.WriteSettingFile(setting);
            }
            Console.Clear();
            
            Console.WriteLine("Press Enter to start");
            Console.WriteLine("Press ESC to stop");
            Console.WriteLine("Score: "+score);
            Console.WriteLine(setting.PreferredName+ " your HighScore is:" + setting.HighScore);
            while (!Console.KeyAvailable)
            {
                var pressedKey = Console.ReadKey(true);
                switch (pressedKey.Key)
                {
                    case ConsoleKey.Enter:
                        Game game = new Game();
                        score = game.Start();
                        if(score > setting.HighScore)
                        {
                            setting.HighScore = score;
                            readGameFile.WriteSettingFile(setting);
                        }
                        Main(new string[] { });
                        break;
                    case ConsoleKey.Escape:
                        return;
                        
                }
            }

        }
        
        
    }
}
