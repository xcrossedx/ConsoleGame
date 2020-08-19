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

        public static int score;

        public static int enemyBank;
        public static int powerUpBank;

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

                        int difficulty = 1 + (gameTime / 60);

                        enemyBank += difficulty;

                        //WHERE I LEFT OFF!
                        //I was trying to change the enemy spawn rate and difficulty curve,
                        //possibly working in a limit on enemies and maybe power ups a well.

                        if (rng.Next(0, 10 + (5 * difficulty)) <= enemyBank)
                        {
                            int budget = rng.Next(1, enemyBank + 1);
                            ObjectManager.BuyEnemies(budget);
                            enemyBank -= budget;
                        }

                        if (rng.Next(0, 10 + (5 * difficulty)) <= powerUpBank)
                        {
                            int budget = rng.Next(1, powerUpBank + 1);
                            ObjectManager.BuyPowerUps(budget);
                            powerUpBank -= budget;
                        }
                    }

                    List<PowerUp> tempPowerUpList = new List<PowerUp>();
                    List<DamageObject> tempDamageObjectList = new List<DamageObject>();
                    List<Entity> tempEntityList = new List<Entity>();

                    foreach (PowerUp powerUp in ObjectManager.powerUps)
                    {
                        if (powerUp.lifeSpan > 0)
                        {
                            if (DateTime.Now >= powerUp.lastUpdate.AddSeconds(.1))
                            {
                                grid[powerUp.position.row][powerUp.position.col] = powerUp.id;

                                if (powerUp.lifeSpan > 5 || powerUp.lifeSpan % 2 == 0)
                                {
                                    ScreenManager.changes.Add((powerUp.position.row, powerUp.position.col, powerUp.defaultSprite));
                                }
                                else
                                {
                                    ScreenManager.changes.Add((powerUp.position.row, powerUp.position.col, powerUp.flashSprite));
                                }

                                powerUp.lifeSpan -= 1;
                                powerUp.lastUpdate = DateTime.Now;
                            }

                            tempPowerUpList.Add((PowerUp)powerUp.MemberwiseClone());
                        }
                        else
                        {
                            ScreenManager.changes.Add((powerUp.position.row, powerUp.position.col, new Sprite("  ", 0, 0)));
                        }
                    }

                    ObjectManager.powerUps.Clear();
                    ObjectManager.powerUps.AddRange(tempPowerUpList);

                    foreach (DamageObject damageObject in ObjectManager.damageObjects)
                    {
                        if (damageObject.lifeSpan > 0)
                        {
                            if (damageObject.direction.v > 0)
                            {
                                if (DateTime.Now >= damageObject.lastUpdate.AddSeconds(0.1 / damageObject.direction.v))
                                {
                                    ScreenManager.changes.Add((damageObject.position.row, damageObject.position.col, new Sprite("  ", 0, 0)));
                                    damageObject.position = (damageObject.position.row + damageObject.direction.r, damageObject.position.col + damageObject.direction.c);

                                    if (!ObjectManager.entities.Exists(x => x.alignment != damageObject.alignment && x.position == damageObject.position))
                                    {
                                        if (damageObject.position.row >= 0 && damageObject.position.row < grid.Length && damageObject.position.col >= 0 && damageObject.position.col < grid[0].Length)
                                        {
                                            grid[damageObject.position.row][damageObject.position.col] = damageObject.id;

                                            if (ScreenManager.changes.Exists(x => x.row == damageObject.position.row && x.col == damageObject.position.col))
                                            {
                                                ScreenManager.changes.RemoveAt(ScreenManager.changes.IndexOf(ScreenManager.changes.Find(x => x.row == damageObject.position.row && x.col == damageObject.position.col)));
                                            }

                                            ScreenManager.changes.Add((damageObject.position.row, damageObject.position.col, damageObject.sprite));
                                            damageObject.lifeSpan -= 1;
                                        }
                                        else
                                        {
                                            damageObject.lifeSpan = 0;
                                        }
                                        
                                        damageObject.lastUpdate = DateTime.Now;
                                    }
                                    else
                                    {
                                        ObjectManager.entities[ObjectManager.entities.IndexOf(ObjectManager.entities.Find(x => x.alignment != damageObject.alignment && x.position == damageObject.position))].health -= 1;
                                        damageObject.lifeSpan = 0;
                                    }
                                }
                            }
                            else
                            {
                                if (DateTime.Now >= damageObject.lastUpdate.AddSeconds(0.1))
                                {
                                    damageObject.lastUpdate = DateTime.Now;

                                    if (!ObjectManager.entities.Exists(x => x.alignment != damageObject.alignment && x.position == damageObject.position))
                                    {
                                        ScreenManager.changes.Add((damageObject.position.row, damageObject.position.col, damageObject.sprite));
                                        damageObject.lifeSpan -= 1;
                                    }
                                    else
                                    {
                                        ObjectManager.entities[ObjectManager.entities.IndexOf(ObjectManager.entities.Find(x => x.alignment != damageObject.alignment && x.position == damageObject.position))].health -= 1;
                                        damageObject.lifeSpan = 0;
                                    }
                                }
                            }

                            tempDamageObjectList.Add((DamageObject)damageObject.MemberwiseClone());
                        }
                        else if (damageObject.position.row >= 0 && damageObject.position.row < grid.Length && damageObject.position.col >= 0 && damageObject.position.col < grid[0].Length)
                        {
                            ScreenManager.changes.Add((damageObject.position.row, damageObject.position.col, new Sprite("  ", 0, 0)));
                        }
                    }

                    ObjectManager.damageObjects.Clear();
                    ObjectManager.damageObjects.AddRange(tempDamageObjectList);

                    for (int e = 1; e < ObjectManager.entities.Count(); e++)
                    {
                        Entity entity = ObjectManager.entities[e];

                        if (entity.health > 0)
                        {
                            entity.SetDirection(-1);

                            if (DateTime.Now >= entity.lastUpdate.AddSeconds(0.25))
                            {
                                (int row, int col) tempPosition = (entity.position.row + entity.direction.r, entity.position.col + entity.direction.c);

                                if (entity.position != ObjectManager.entities[0].position)
                                {
                                    if (tempPosition.row >= 0 && tempPosition.row < grid.Length && tempPosition.col >= 0 && tempPosition.col < grid[0].Length)
                                    {
                                        grid[tempPosition.row][tempPosition.col] = entity.id;

                                        ScreenManager.changes.Add((entity.position.row, entity.position.col, new Sprite("  ", 0, 0)));

                                        if (ScreenManager.changes.Exists(x => x.row == tempPosition.row && x.col == tempPosition.col))
                                        {
                                            ScreenManager.changes.RemoveAt(ScreenManager.changes.IndexOf(ScreenManager.changes.Find(x => x.row == tempPosition.row && x.col == tempPosition.col)));
                                        }

                                        ScreenManager.changes.Add((tempPosition.row, tempPosition.col, entity.sprite));

                                        entity.position = (tempPosition.row, tempPosition.col);
                                    }

                                    entity.lastUpdate = DateTime.Now;
                                }
                                else
                                {
                                    entity.health = 0;
                                    ObjectManager.entities[0].health -= 1;
                                }
                            }

                            tempEntityList.Add((Entity)entity.MemberwiseClone());
                        }
                        else
                        {
                            ScreenManager.changes.Add((entity.position.row, entity.position.col, new Sprite("  ", 0, 0)));
                            powerUpBank += 1;
                            score += 100;
                        }
                    }

                    Entity player = (Entity)ObjectManager.entities[0].MemberwiseClone();

                    if (player.health > 0)
                    {
                        if (DateTime.Now >= player.lastUpdate.AddSeconds(0.1))
                        {
                            (int row, int col) tempPosition = (player.position.row + (player.direction.r * player.direction.v), player.position.col + (player.direction.c * player.direction.v));

                            if (tempPosition != player.position)
                            {
                                if (tempPosition.row >= 0 && tempPosition.row < grid.Length && tempPosition.col >= 0 && tempPosition.col < grid[0].Length)
                                {
                                    grid[tempPosition.row][tempPosition.col] = player.id;

                                    if (player.direction.v > 0)
                                    {
                                        if (player.trail == 0)
                                        {
                                            ScreenManager.changes.Add((player.position.row, player.position.col, new Sprite("  ", 0, 0)));
                                        }
                                        else
                                        {
                                            DamageObject spike = new DamageObject(11, player, 1);
                                            ObjectManager.damageObjects.Add(spike);
                                            ScreenManager.changes.Add((spike.position.row, spike.position.col, spike.sprite));
                                        }
                                    }

                                    if (ScreenManager.changes.Exists(x => x.row == tempPosition.row && x.col == tempPosition.col))
                                    {
                                        ScreenManager.changes.RemoveAt(ScreenManager.changes.IndexOf(ScreenManager.changes.Find(x => x.row == tempPosition.row && x.col == tempPosition.col)));
                                    }

                                    ScreenManager.changes.Add((tempPosition.row, tempPosition.col, player.sprite));

                                    player.position = (tempPosition.row, tempPosition.col);

                                    if (ObjectManager.powerUps.Exists(x => x.position.row == player.position.row && x.position.col == player.position.col))
                                    {
                                        PowerUp powerUp = ObjectManager.powerUps[ObjectManager.powerUps.IndexOf(ObjectManager.powerUps.Find(x => x.position.row == player.position.row && x.position.col == player.position.col))];

                                        if (powerUp.id == 20)
                                        {
                                            player.health += powerUp.strength;
                                            player.powerUpCounter[0] += 1;
                                            score += 10;
                                        }
                                        else if (powerUp.id == 21)
                                        {
                                            player.range += powerUp.strength;
                                            player.powerUpCounter[1] += 1;
                                            score += 10;
                                        }
                                        else if (powerUp.id == 22)
                                        {
                                            player.trail += powerUp.strength;
                                            player.powerUpCounter[2] += 1;
                                            score += 20;
                                        }
                                        else if (powerUp.id == 23)
                                        {
                                            player.fireRate += powerUp.strength;
                                            player.powerUpCounter[3] += 1;
                                            score += 20;
                                        }
                                        else if (powerUp.id == 24)
                                        {
                                            player.spread += powerUp.strength;
                                            player.powerUpCounter[4] += 1;
                                            score += 30;
                                        }

                                        ObjectManager.powerUps.RemoveAt(ObjectManager.powerUps.IndexOf(powerUp));
                                    }
                                }

                                player.direction.v = 0;
                            }
                            else
                            {
                                ScreenManager.changes.Add((player.position.row, player.position.col, player.sprite));
                            }

                            player.lastUpdate = DateTime.Now;
                        }
                    }
                    else
                    {
                        gameState = 0;
                    }

                    ObjectManager.entities.Clear();
                    ObjectManager.entities.Add((Entity)player.MemberwiseClone());
                    ObjectManager.entities.AddRange(tempEntityList);

                    ScreenManager.ScreenUpdate();
                }

                ConsoleKey input = Console.ReadKey(true).Key;

                if (gameState == 0)
                {
                    ScreenManager.ClearScreen();
                    Init();
                    gameState = 1;
                }

                if (input == ConsoleKey.Spacebar && !(ObjectManager.entities[0].direction.r == 0 && ObjectManager.entities[0].direction.c == 0))
                {
                    ObjectManager.entities[0].Fire();
                }
                else if (input == ConsoleKey.UpArrow || input == ConsoleKey.W)
                {
                    ObjectManager.entities[0].SetDirection(0);
                }
                else if (input == ConsoleKey.RightArrow || input == ConsoleKey.D)
                {
                    ObjectManager.entities[0].SetDirection(1);
                }
                else if (input == ConsoleKey.DownArrow || input == ConsoleKey.S)
                {
                    ObjectManager.entities[0].SetDirection(2);
                }
                else if (input == ConsoleKey.LeftArrow || input == ConsoleKey.A)
                {
                    ObjectManager.entities[0].SetDirection(3);
                }
                else if (input == ConsoleKey.Escape)
                {
                    gameState = -1;
                }
            }
        }
    }
}
