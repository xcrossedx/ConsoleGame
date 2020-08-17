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

        public static void BuyEnemies(int budget)
        {
            while (budget > 0)
            {
                budget -= 1;
                entities.Add(new Entity(1));
            }
        }

        public static void BuyPowerUps(int budget)
        {
            while (budget > 0)
            {
                int tier = rng.Next(0, 3);

                if (tier == 2 && budget >= 5)
                {
                    powerUps.Add(new PowerUp(24));
                    budget -= 5;
                }
                else if (tier >= 1 && budget >= 3)
                {
                    if (rng.Next() % 2 == 0)
                    {
                        powerUps.Add(new PowerUp(23));
                    }
                    else
                    {
                        powerUps.Add(new PowerUp(22));
                    }

                    budget -= 3;
                }
                else
                {
                    if (rng.Next() % 2 == 0)
                    {
                        powerUps.Add(new PowerUp(21));
                    }
                    else
                    {
                        powerUps.Add(new PowerUp(20));
                    }

                    budget -= 1;
                }
            }
        }
    }
}
