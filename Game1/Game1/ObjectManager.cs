using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Vector
    {
        public int r;
        public int c;
        public int v;

        public Vector(int row, int col, int vel)
        {
            r = row;
            c = col;
            v = vel;
        }
    }

    class Sprite
    {
        public string content;
        public int bColor;
        public int fColor;

        public Sprite(string content, int bColor, int fColor)
        {
            this.content = content;
            this.bColor = bColor;
            this.fColor = fColor;
        }
    }

    class ObjectManager : GameManager
    {
        public static List<Entity> entities = new List<Entity>();
        public static List<DamageObject> damageObjects = new List<DamageObject>();
        public static List<PowerUp> powerUps = new List<PowerUp>();

        public static void Init()
        {
            entities.Clear();
            damageObjects.Clear();
            powerUps.Clear();

            entities.Add(new Entity(0));
        }

        public static void UpdateObjects()
        {
            List<PowerUp> tempPowerUpList = new List<PowerUp>();
            List<DamageObject> tempDamageObjectList = new List<DamageObject>();
            List<Entity> tempEntityList = new List<Entity>();

            foreach (PowerUp powerUp in powerUps)
            {
                if (powerUp.lifeSpan > 0)
                {
                    if (DateTime.Now >= powerUp.lastUpdate.AddSeconds(.1))
                    {
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
                    powerUpTotal -= 1;
                }
            }

            powerUps.Clear();
            powerUps.AddRange(tempPowerUpList);

            foreach (DamageObject damageObject in damageObjects)
            {
                if (damageObject.lifeSpan > 0 && damageObject.durability > 0)
                {
                    float updateRate = 0.1f;

                    if (damageObject.direction.v > 0) { updateRate /= (float)damageObject.direction.v; }

                    if (DateTime.Now >= damageObject.lastUpdate.AddSeconds(updateRate))
                    {
                        if (damageObject.direction.v > 0 && damageObject.position.row >= 0 && damageObject.position.row < grid.Length && damageObject.position.col >= 0 && damageObject.position.col < grid[0].Length)
                        {
                            if (!damageObject.fresh)
                            {
                                ScreenManager.changes.Add((damageObject.position.row, damageObject.position.col, new Sprite("  ", 0, 0)));
                            }
                            else
                            {
                                damageObject.fresh = false;
                            }

                            damageObject.position = (damageObject.position.row + damageObject.direction.r, damageObject.position.col + damageObject.direction.c);
                        }

                        if (damageObject.position.row >= 0 && damageObject.position.row < grid.Length && damageObject.position.col >= 0 && damageObject.position.col < grid[0].Length)
                        {
                            if (ScreenManager.changes.Exists(x => x.row == damageObject.position.row && x.col == damageObject.position.col))
                            {
                                ScreenManager.changes.RemoveAt(ScreenManager.changes.IndexOf(ScreenManager.changes.Find(x => x.row == damageObject.position.row && x.col == damageObject.position.col)));
                            }

                            ScreenManager.changes.Add((damageObject.position.row, damageObject.position.col, damageObject.sprite));
                            if (damageObject.direction.v == 0) { damageObject.lifeSpan -= 1; }
                            else { damageObject.lifeSpan -= 2 / damageObject.direction.v; }
                        }
                        else
                        {
                            damageObject.lifeSpan = 0;
                        }

                        damageObject.lastUpdate = DateTime.Now;

                        if (entities.Exists(x => x.alignment != damageObject.alignment && x.position == damageObject.position))
                        {
                            entities[entities.IndexOf(entities.Find(x => x.alignment != damageObject.alignment && x.position == damageObject.position))].health -= 1;
                            enemyTotal -= 1;
                            damageObject.durability -= 1;
                            powerUpBank += 2 - (2 / (1 + (float)Math.Exp((difficulty / 4) - 2)));
                            score += 100;
                        }
                    }

                    tempDamageObjectList.Add((DamageObject)damageObject.MemberwiseClone());
                }
                else if (damageObject.position.row >= 0 && damageObject.position.row < grid.Length && damageObject.position.col >= 0 && damageObject.position.col < grid[0].Length)
                {
                    ScreenManager.changes.Add((damageObject.position.row, damageObject.position.col, new Sprite("  ", 0, 0)));
                }
            }

            damageObjects.Clear();
            damageObjects.AddRange(tempDamageObjectList);

            for (int e = 1; e < entities.Count(); e++)
            {
                Entity entity = entities[e];

                if (entity.health > 0)
                {
                    if (DateTime.Now >= entity.lastUpdate.AddSeconds(0.25))
                    {
                        entity.SetDirection(-1);
                        (int row, int col) tempPosition = (entity.position.row + entity.direction.r, entity.position.col + entity.direction.c);

                        if (damageObjects.Exists(x => x.position == tempPosition))
                        {
                            entity.health = 0;
                            score += 100;
                            damageObjects[damageObjects.IndexOf(damageObjects.Find(x => x.position == tempPosition))].durability -= 1;
                        }
                        else if (entity.position == entities[0].position)
                        {
                            entity.health = 0;
                            entities[0].health -= 1;
                        }
                        else
                        {
                            if (tempPosition.row >= 0 && tempPosition.row < grid.Length && tempPosition.col >= 0 && tempPosition.col < grid[0].Length)
                            {
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
                    }

                    tempEntityList.Add((Entity)entity.MemberwiseClone());
                }
                else
                {
                    ScreenManager.changes.Add((entity.position.row, entity.position.col, new Sprite("  ", 0, 0)));
                }
            }

            Entity player = (Entity)entities[0].MemberwiseClone();

            if (player.health > 0)
            {
                if (DateTime.Now >= player.lastUpdate.AddSeconds(0.1))
                {
                    (int row, int col) tempPosition = (player.position.row + (player.direction.r * player.direction.v), player.position.col + (player.direction.c * player.direction.v));

                    if (tempPosition != player.position)
                    {
                        if (tempPosition.row >= 0 && tempPosition.row < grid.Length && tempPosition.col >= 0 && tempPosition.col < grid[0].Length)
                        {
                            if (player.direction.v > 0)
                            {
                                if (player.trail == 0)
                                {
                                    ScreenManager.changes.Add((player.position.row, player.position.col, new Sprite("  ", 0, 0)));
                                }
                                else
                                {
                                    DamageObject spike = new DamageObject(1, player, 1);
                                    damageObjects.Add(spike);
                                    ScreenManager.changes.Add((spike.position.row, spike.position.col, spike.sprite));
                                }
                            }

                            if (ScreenManager.changes.Exists(x => x.row == tempPosition.row && x.col == tempPosition.col))
                            {
                                ScreenManager.changes.RemoveAt(ScreenManager.changes.IndexOf(ScreenManager.changes.Find(x => x.row == tempPosition.row && x.col == tempPosition.col)));
                            }

                            ScreenManager.changes.Add((tempPosition.row, tempPosition.col, player.sprite));

                            player.position = (tempPosition.row, tempPosition.col);

                            if (powerUps.Exists(x => x.position.row == player.position.row && x.position.col == player.position.col))
                            {
                                PowerUp powerUp = powerUps[powerUps.IndexOf(powerUps.Find(x => x.position.row == player.position.row && x.position.col == player.position.col))];

                                if (powerUp.id == 10)
                                {
                                    string result = "is already full!";

                                    if (player.health < 10)
                                    {
                                        player.health += powerUp.strength;
                                        result = "restored by 1!";
                                    }
                                    
                                    player.powerUpCounter[0] += 1;
                                    ScreenManager.messageQueue.Add(new ItemMessage(powerUp.defaultSprite, $"Health {result}", 5));
                                }
                                else if (powerUp.id == 11)
                                {
                                    string result = "limit reached";

                                    if (player.powerUpCounter[1] < 5)
                                    {
                                        player.range += powerUp.strength;
                                        result = "increased by 2!";
                                    }

                                    player.powerUpCounter[1] += 1;
                                    ScreenManager.messageQueue.Add(new ItemMessage(powerUp.defaultSprite, $"Bullet range {result}", 5));
                                }
                                else if (powerUp.id == 20)
                                {
                                    string result = "limit reached!";

                                    if (player.powerUpCounter[2] < 5)
                                    {
                                        player.fireRate += powerUp.strength;
                                        result = "increased by 1!";
                                    }

                                    player.powerUpCounter[2] += 1;
                                    ScreenManager.messageQueue.Add(new ItemMessage(powerUp.defaultSprite, $"Fire rate {result}", 5));
                                }
                                else if (powerUp.id == 21)
                                {
                                    string result = "limit reached!";

                                    if (player.powerUpCounter[3] < 5)
                                    {
                                        player.piercing += powerUp.strength;
                                        result = "increased by 1!";
                                    }

                                    player.powerUpCounter[3] += 1;
                                    ScreenManager.messageQueue.Add(new ItemMessage(powerUp.defaultSprite, $"Piercing {result}", 5));
                                }
                                else if (powerUp.id == 30)
                                {
                                    string result = "limit reached";

                                    if (player.powerUpCounter[4] < 5)
                                    {
                                        player.trail += powerUp.strength;
                                        result = "increased by 2!";
                                    }

                                    player.powerUpCounter[4] += 1;
                                    ScreenManager.messageQueue.Add(new ItemMessage(powerUp.defaultSprite, $"Spike trail {result}", 5));
                                }
                                else if (powerUp.id == 31)
                                {
                                    string result = "limit reached!";

                                    if (player.powerUpCounter[5] < 3)
                                    {
                                        player.spread += powerUp.strength;
                                        result = "increased by 2!";
                                    }
                                    else if (player.powerUpCounter[5] < 11)
                                    {
                                        player.spread += powerUp.strength / 2;
                                        result = "increased by 1!";
                                    }

                                    player.powerUpCounter[5] += 1;
                                    ScreenManager.messageQueue.Add(new ItemMessage(powerUp.defaultSprite, "Spread " + result, 5));
                                }

                                score += (powerUp.id / 10) * 10;
                                powerUps.RemoveAt(powerUps.IndexOf(powerUp));
                                powerUpTotal -= 1;
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
                gameState = 3;
                ScreenManager.GameMessage("Game over!", 15);
            }

            entities.Clear();
            entities.Add((Entity)player.MemberwiseClone());
            entities.AddRange(tempEntityList);
        }

        public static void BuyEnemies(int budget)
        {
            while (budget > 0)
            {
                budget -= 1;
                entities.Add(new Entity(1));
                enemyTotal += 1;
            }
        }

        public static void BuyPowerUps(int budget)
        {
            while (budget > 0)
            {
                int tier = rng.Next(0, 3);

                PowerUp powerUp;

                if (tier == 2 && budget >= 5)
                {
                    if (rng.Next() % 2 == 0) { powerUp = new PowerUp(30); }
                    else { powerUp = new PowerUp(31); }
                }
                else if (tier >= 1 && budget >= 3)
                {
                    if (rng.Next() % 2 == 0) { powerUp = new PowerUp(20); }
                    else { powerUp = new PowerUp(21); }
                }
                else
                {
                    if (rng.Next() % 2 == 0) { powerUp = new PowerUp(10); }
                    else { powerUp = new PowerUp(11); }
                }

                powerUps.Add(powerUp);
                powerUpTotal += 1;
                budget -= powerUp.cost;
            }
        }
    }
}
