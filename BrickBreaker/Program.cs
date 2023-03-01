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
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Press Enter to start");
            Console.WriteLine("Press ESC to stop");
            Console.WriteLine("Score: "+score);
            while (!Console.KeyAvailable)
            {
                var pressedKey = Console.ReadKey(true);
                switch (pressedKey.Key)
                {
                    case ConsoleKey.Enter:
                        Game game = new Game();
                        score = game.Start();
                        Main(new string[] { });
                        break;
                    default:
                        return;
                        
                }
            }

        }
        
        
    }
}
