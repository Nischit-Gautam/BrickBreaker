using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BrickBreaker
{
    internal class Game
    {

        public int ballXPosition = 20;
        public int ballYPosition = 23;
        public int width = 40;
        public int height = 25;
        public double ballMovement = 0;
        public double ballSpeed = 0.5;
        public int padelXPosition = 20;
        public int ballXDirection = 1;
        public int ballYDirection = 1;        
        public int padelWidth = 5;
        public bool gameOver = false;
        public int brickColumns = 20;
        public int brickRaws = 4;
        public static int refreshRate = 0;
        public List<int> paddlePositions = new List<int>();
        public List<Brick> bricks = new List<Brick>();
        public Random random = new Random();
        public static System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
        public int score = 0;
        bool isPowerActive = false;
        public static int powerUpDuration = 0;
        public int Start()
        {
            ReadGameFile readGameFile = new ReadGameFile();
            var settings = readGameFile.ReadSettingFile();
            refreshRate = settings.RefreshRate;
            //InitializeBricks();
            CreateRandomBricks();
            while (!gameOver)
            {
                CheckWallCollision();
                DrawBricks();
                DrawRightBorder();
                CheckBrickCollision();
                MoveBall();
                DrawBall();
                DrawPadel();
                CheckPaddleCollision();
                DrawScore();
                if (Console.KeyAvailable == true)
                {
                    MovePadel(Console.ReadKey(true));
                }
                if(isPowerActive == true) {
                    padelWidth = 10;                
                }
                var powerActiveTime = stopWatch.Elapsed;
                if(powerActiveTime>TimeSpan.FromSeconds(powerUpDuration)) {
                    padelWidth /= 2;
                    stopWatch.Stop();
                    stopWatch.Reset();
                    isPowerActive = false;
                    powerUpDuration = 0;
                }
                Thread.Sleep(refreshRate);

            }
            return score;
        }
        private void DrawRightBorder()
        {
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(width + 1, i);
                Console.WriteLine("|");
            }
        }
        private void InitializeBricks()
        {
            int offSet = (int)Math.Ceiling((double)((width - brickColumns) / 2));
            for (int i = 0; i <= brickColumns; i++)
            {
                for (int j = 0; j <= brickRaws; j++)
                {
                    //initialize
                    bricks.Add(new Brick(offSet + i, j));
                }
            }
        }
        private void CreateRandomBricks()
        {
            int totalBrickCount = brickColumns * brickRaws;
           
            HashSet<int> addedBricks = new HashSet<int> ();
            for (int i = 0; i <= totalBrickCount; i++)
            {                
                    bool gotValue = false;
                    while (!gotValue)
                    {
                        var coordinate = GetCoOrdinates();
                        string tmp = coordinate.Item1.ToString() + coordinate.Item2.ToString();
                        if (!addedBricks.TryGetValue(Convert.ToInt32(tmp), out int aa))
                        {
                            gotValue = true;
                            if(i%10==0)
                            {
                                bricks.Add(new Brick(coordinate.Item2, coordinate.Item1,true));
                            }
                            else
                            {
                                bricks.Add(new Brick(coordinate.Item2, coordinate.Item1));
                            }
                            
                            addedBricks.Add(Convert.ToInt32(tmp));
                        }
                    }
                   
                
            }
            
        }
        private (int,int) GetCoOrdinates()
        {
            var tempHeight = random.Next(height - 20);
            var tempWeidth = random.Next(width);
            return (tempHeight,tempWeidth);
        }
        public  void MovePadel(ConsoleKeyInfo key)
        {

            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    {
                        if (padelXPosition != 0)
                        {
                            padelXPosition--;
                        }
                    }
                    break;

                case ConsoleKey.RightArrow:
                    {
                        if ((padelXPosition+padelWidth) <= width)
                        {
                            padelXPosition++;
                        }
                    }
                    break;
            }
        }
        public  void CheckPaddleCollision()
        {
            paddlePositions.Clear();
            int tempPosition = padelXPosition;
            for (int i = 0; i <= padelWidth; i++)
            {
                paddlePositions.Add(tempPosition);
                tempPosition++;
            }
            if (paddlePositions.Contains(ballXPosition) && ballYPosition == height - 2 && ballXDirection == 1)
            {
                //reverse the ball 
                ballXDirection = 1;
                ballYDirection = -1;
            }
            if (paddlePositions.Contains(ballXPosition) && ballYPosition == height - 2 && ballXDirection == -1)
            {
                //reverse the ball 
                ballXDirection = -1;
                ballYDirection = -1;
            }
        }

        public  void CheckWallCollision()
        {
            if (ballXPosition == 0 && ballXDirection == -1)
            {
                ballXDirection = 1;
            }
            if (ballXPosition == width && ballXDirection == 1)
            {
                ballXDirection = -1;
            }
            if (ballYPosition == 0)
            {
                ballYDirection = 1;
            }
            if (ballYPosition == height)
            {
                gameOver = true;
            }
        }
        public  void CheckBrickCollision()
        {
            foreach (Brick brick in bricks)
            {
                if (ballXPosition == brick.x && ballYPosition == brick.y && !brick.isDestroyed)
                {
                    brick.isDestroyed = true;
                    score += 10;
                    if (brick.hasSpecialPower)
                    {
                        powerUpDuration += 10;
                        isPowerActive = true;
                        stopWatch.Start();
                    }
                    if (ballYDirection == 1)
                    {
                        ballYDirection = -1;

                        int randomInt = random.Next(2);

                        if (randomInt == 0)
                            ballXDirection = -1;
                        else
                            ballXDirection = 1;
                    }
                    else
                    {
                        ballYDirection = 1;
                        int randomInt = random.Next(2);

                        if (randomInt == 0)
                            ballXDirection = -1;
                        else
                            ballXDirection = 1;
                    }
                }

            }
        }
        private void DrawBall()
        {

            Console.SetCursorPosition(ballXPosition, ballYPosition);
            Console.WriteLine("O");
        }
        public  void DrawPadel()
        {
            //Console.Clear();
            Console.SetCursorPosition(padelXPosition, height - 2);
            string paddle = string.Empty;
            for (int i = 0; i <= padelWidth; i++)
            {
                paddle += "=";
            }

            Console.WriteLine(paddle);
        }
        public  void DrawBricks()
        {
            Console.Clear();
            foreach (Brick brick in bricks)
            {
                if (!brick.isDestroyed)
                {
                    Console.SetCursorPosition(brick.x, brick.y);
                    if (brick.hasSpecialPower)
                    {
                        Console.WriteLine("@");
                    }
                    else
                    {                        
                        Console.WriteLine("#");
                    }
                }
            }
        }
        public  void DrawScore()
        {
            Console.SetCursorPosition(0, height - 1);
            Console.WriteLine("Score: " + score);
        }
        public  void MoveBall()
        {
            if (ballMovement > 1)
            {
                ballYPosition = ballYPosition + ballYDirection;
                ballXPosition = ballXPosition + ballXDirection;
                ballMovement = 0;
            }
            ballMovement = ballMovement + ballSpeed;
        }
    }
}
