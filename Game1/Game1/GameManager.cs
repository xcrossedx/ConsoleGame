using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Game1
{
    class GameManager
    {
        public static int[][] grid;

        public static Random rng = new Random();

        public static int gameState = 0;

        public static DateTime gameTick;
        public static int gameTime;
        public static int difficulty;

        public static int score;

        public static float enemyBank;
        public static float powerUpBank;

        public static int enemyTotal;
        public static int powerUpTotal;

        private static void Init()
        {
            ClearGrid();
            ObjectManager.Init();
            enemyBank = 5;
            powerUpBank = 5;
            enemyTotal = 0;
            powerUpTotal = 0;
            gameTick = DateTime.Now;
            gameTime = 0;
            difficulty = 1;
            score = 0;
        }

        private static void ClearGrid()
        {
            ScreenManager.Reset();

            grid = new int[Console.WindowHeight - 2][];

            int columns = (Console.WindowWidth - 2) / 2;

            if (Console.WindowWidth - 2 % 2 != 0)
            {
                columns = (Console.WindowWidth - 3) / 2;
            }

            for (int r = 0; r < grid.Length; r++)
            {
                grid[r] = new int[columns];
            }
        }

        public static void Play()
        {
            while (gameState != -1)
            {
                while (Console.KeyAvailable == false && gameState == 1)
                {
                    if (DateTime.Now >= gameTick.AddSeconds(1))
                    {
                        gameTick = DateTime.Now;
                        gameTime += 1;
                        difficulty = 1 + (gameTime / 60);

                        float income = 5.0f - (5.0f / (1 + (float)Math.Exp((difficulty / 3) - 3)));
                        enemyBank += income;

                        if (enemyTotal < 101 - (100 / (1 + (float)Math.Exp((difficulty / 10) - 2))) && enemyBank >= 1)
                        {
                            int max = (int)enemyBank + 1;
                            if (max > 6) { max = 6; }
                            int budget = rng.Next(1, max);
                            ObjectManager.BuyEnemies(budget);
                            enemyBank -= budget;
                        }

                        if (powerUpTotal < 11 - (6 / 1 + (float)Math.Exp((difficulty / 4) - 3)) && powerUpBank >= 5)
                        {
                            int max = (int)powerUpBank + 1;
                            if (max > 11) { max = 11; }
                            int budget = rng.Next(1, max);
                            ObjectManager.BuyPowerUps(budget);
                            powerUpBank -= budget;
                        }
                    }

                    ObjectManager.UpdateObjects();
                    ScreenManager.ScreenUpdate();
                }

                ConsoleKey input = Console.ReadKey(true).Key;

                if (gameState == 0 && input != ConsoleKey.Escape)
                {
                    ScreenManager.ClearScreen();
                    Init();
                    gameState = 1;
                }

                if (gameState == 1 && input == ConsoleKey.Spacebar && !(ObjectManager.entities[0].direction.r == 0 && ObjectManager.entities[0].direction.c == 0))
                {
                    ObjectManager.entities[0].Fire();
                }
                else if (gameState == 1 && input == ConsoleKey.UpArrow || input == ConsoleKey.W)
                {
                    ObjectManager.entities[0].SetDirection(0);
                }
                else if (gameState == 1 && input == ConsoleKey.RightArrow || input == ConsoleKey.D)
                {
                    ObjectManager.entities[0].SetDirection(1);
                }
                else if (gameState == 1 && input == ConsoleKey.DownArrow || input == ConsoleKey.S)
                {
                    ObjectManager.entities[0].SetDirection(2);
                }
                else if (gameState == 1 && input == ConsoleKey.LeftArrow || input == ConsoleKey.A)
                {
                    ObjectManager.entities[0].SetDirection(3);
                }
                else if (input == ConsoleKey.Escape)
                {
                    switch (gameState)
                    {
                        case 0:
                            gameState = -1;
                            break;
                        case 1:
                            gameState = 2;
                            ScreenManager.GameMessage("Game paused!", 15);
                            break;
                        case 2:
                            gameState = 0;
                            ScreenManager.Clear();
                            break;
                        case 3:
                            gameState = -1;
                            break;
                    }
                }
                else
                {
                    if (gameState == 3 && input == ConsoleKey.Enter)
                    {
                        gameState = 0;
                        ScreenManager.Clear();
                    }
                    else if (gameState != 3)
                    {
                        gameState = 1;
                    }
                }
            }
        }
    }
}
