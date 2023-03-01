using System;
using System.Reflection.Metadata;
using System.Threading;

namespace BrickBreaker
{
    internal class Program
    {
        // Game variables
        static int paddleX; // Paddle X position
        static int ballX, ballY; // Ball position
        static int ballDirX, ballDirY; // Ball direction (1 or -1)
        static int score; // Player's score
        static int bricksRemaining; // Number of bricks remaining
        static bool gameOver; // Is the game over?
        static float ballVelocityX=1, ballVelocityY=-1;
        // Game constants
        const int ConsoleWidth = 80;
        const int ConsoleHeight = 25;
        const int PaddleWidth = 10;
        const int PaddleHeight = 1;
        const int BallSize = 1;
        const int BrickWidth = 2;
        const int BrickHeight = 2;
        const int brickRows = 5;
        const int brickCols = 10;    
        const int maxBallSpeed = 3;


        // Game objects
        private static Brick[,] bricks;
        static void Main(string[] args)
        {
            // Set up the console
            Console.Title = "Brick Breaker";
            Console.WindowWidth = ConsoleWidth;
            Console.BufferWidth = ConsoleWidth;
            Console.WindowHeight = ConsoleHeight;
            Console.BufferHeight = ConsoleHeight;
            Console.CursorVisible = false;

            // Initialize the game
            InitGame();

            // Game loop
            while (!gameOver)
            {
                // Handle input
               // HandleInput();

                // Update game objects
                Update();

                // Draw game objects
                DrawObj();

                // Wait for next frame
                System.Threading.Thread.Sleep(50);
            }

            // Game over
            Console.Clear();
            Console.WriteLine("Game Over!");
            Console.WriteLine("Score: " + score);
            Console.ReadKey();
        }
        static void Update()
        {
            // Move ball
            MoveBall();

            // Check for wall collision
            CheckWallCollision();

            // Check for paddle collision
            CheckPaddleCollision();

            // Check for brick collision
            CheckCollision();

            // Check for game over
            if (ballY >= Console.WindowHeight)
            {
                gameOver = true;
            }
        }
        static void MoveBall()
        {
            ballX += (int)ballVelocityX;
            ballY += (int)ballVelocityY;
            CheckWallCollision();
            CheckPaddleCollision();
            CheckCollision();
        }
        static void CheckWallCollision()
        {
            if (ballX <= 0 || ballX >= Console.WindowWidth - 1)
            {
                ballVelocityX = -ballVelocityX;
                // adjust ball position to avoid getting stuck in wall
                ballX = ballX < 0 ? 0 : Console.WindowWidth - 1;
            }

            if (ballY <= 0)
            {
                ballVelocityY = -ballVelocityY;
                // adjust ball position to avoid getting stuck in wall
                ballY = 0;
            }
            else if (ballY >= Console.WindowHeight)
            {
                gameOver = true;
            }
                        
           
        }
        static void CheckPaddleCollision()
        {
            if (ballY == ConsoleHeight - 2 && ballX >= paddleX && ballX < paddleX + PaddleWidth)
            {
                ballVelocityY = -ballVelocityY;
                float ballPositionOnPaddle = ballX - paddleX - PaddleWidth / 2f;
                ballVelocityX = ballPositionOnPaddle / (PaddleWidth / 2f) * maxBallSpeed;
            }
                        
           
        }
        static void CheckCollision()
        {
            for (int i = 0; i < brickRows; i++)
            {
                for (int j = 0; j < brickCols; j++)
                {
                    Brick brick = bricks[i, j];
                    if (brick != null && brick.isDestroyed == false && CheckBrickCollision(brick))
                    {
                        // Ball collided with brick
                        brick.isDestroyed = true;
                        //score += 10;
                        ballDirY *= -1;
                    }
                }
            }


        }
        public static bool CheckBrickCollision(Brick brick)
        {
            for (int i = 0; i < bricks.GetLength(0); i++)
            {
                for (int j = 0; j < bricks.GetLength(1); j++)
                {
                    if (!bricks[i, j].isDestroyed && bricks[i, j].x <= ballX && bricks[i, j].x + BrickWidth >= ballX &&
                        bricks[i, j].y <= ballY && bricks[i, j].y + BrickHeight >= ballY)
                    {
                        bricks[i, j].isDestroyed = true;
                        ballVelocityY *= -1;
                        score++;
                        return true;
                    }
                }
            }
            return false;
        }

        static void DrawObj()
        {
            Console.Clear();

            // Draw the ball
            DrawBall();

            // Draw the paddle
            DrawPaddle();

            // Draw the bricks
            for (int i = 0; i < brickRows; i++)
            {
                for (int j = 0; j < brickCols; j++)
                {
                    //Brick brick = bricks[i, j];
                    var tempBrick = bricks[i, j];
                    if (tempBrick != null && tempBrick.isDestroyed == false)
                    {
                        Console.SetCursorPosition(tempBrick.x, tempBrick.y);
                        Console.Write("###");
                    }
                }
            }

            // Draw the score
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("Score: " + score);
        }
        static void Start()
        {
            // Initialize bricks array
            bricks = new Brick[brickRows, brickCols];

            for (int row = 0; row < brickRows; row++)
            {
                for (int col = 0; col < brickCols; col++)
                {
                    // Initialize brick at (x, y)
                    int x = col * BrickWidth + 1; // Add 1 for padding
                    int y = row * BrickHeight + 1; // Add 1 for padding
                    bricks[row, col] = new Brick(x, y);
                }
            }
        }
        static void InitGame()
        {
            // Initialize paddle position
            paddleX = ConsoleWidth / 2 - PaddleWidth / 2;

            // Initialize ball position and direction
            ballX = ConsoleWidth / 2;
            ballY = ConsoleHeight - 3;
            ballDirX = 1;
            ballDirY = -1;

            // Initialize score and bricks remaining
            score = 0;
            bricksRemaining = brickRows * brickCols;

            // Initialize bricks
            Start();

            // Initialize game over state
            gameOver = false;
        }
        public static void DrawBall()
        {
            Console.SetCursorPosition(ballX, ballY);
            Console.Write("O");
        }
        public static void DrawPaddle()
        {
            Console.SetCursorPosition(paddleX, 24);
            Console.Write("==="); // paddle length is 3
        }
        static void HandleInput()
        {
            // Move the paddle left or right with the arrow keys
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.LeftArrow && paddleX > 0)
                {
                    paddleX--;
                }
                else if (key.Key == ConsoleKey.RightArrow && paddleX < ConsoleWidth - PaddleWidth)
                {
                    paddleX++;
                }
            }
        }
    }
}
