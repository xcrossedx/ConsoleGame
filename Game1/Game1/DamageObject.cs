using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class DamageObject : ObjectManager
    {
        public int id;
        public DateTime lastUpdate;
        public (int row, int col) position;
        public Vector direction;
        public Sprite sprite;
        public int lifeSpan;
        public int alignment;
        public int damage;

        public DamageObject(int id, Entity src, int spread)
        {
            this.id = id;
            lastUpdate = DateTime.Now;

            if (id == 10)
            {
                if (spread == 1)
                {
                    direction = new Vector(src.direction.r, src.direction.c, 2);
                }
                else if (spread == 2)
                {
                    if (src.direction.r == 0)
                    {
                        direction = new Vector(1, src.direction.c, 2);
                    }
                    else
                    {
                        direction = new Vector(src.direction.r, 1, 2);
                    }
                }
                else if (spread == 3)
                {
                    if (src.direction.r == 0)
                    {
                        direction = new Vector(-1, src.direction.c, 2);
                    }
                    else
                    {
                        direction = new Vector(src.direction.r, -1, 2);
                    }
                }
                else if (spread == 4)
                {
                    if (src.direction.r == 0)
                    {
                        direction = new Vector(1, 0, 2);
                    }
                    else
                    {
                        direction = new Vector(0, 1, 2);
                    }
                }
                else if (spread == 5)
                {
                    if (src.direction.r == 0)
                    {
                        direction = new Vector(-1, 0, 2);
                    }
                    else
                    {
                        direction = new Vector(0, -1, 2);
                    }
                }
                else if (spread == 6)
                {
                    if (src.direction.r == 0)
                    {
                        direction = new Vector(1, -src.direction.c, 2);
                    }
                    else
                    {
                        direction = new Vector(-src.direction.r, 1, 2);
                    }
                }
                else if (spread == 7)
                {
                    if (src.direction.r == 0)
                    {
                        direction = new Vector(-1, -src.direction.c, 2);
                    }
                    else
                    {
                        direction = new Vector(-src.direction.r, -1, 2);
                    }
                }
                else
                {
                    if (src.direction.r == 0)
                    {
                        direction = new Vector(0, -src.direction.c, 2);
                    }
                    else
                    {
                        direction = new Vector(-src.direction.r, 0, 2);
                    }
                }

                if (src.alignment == 0)
                {
                    sprite = new Sprite("* ", 0, 15);
                }
                else
                {
                    sprite = new Sprite("* ", 0, 12);
                }

                position = (src.position.row + direction.r, src.position.col + direction.c);
                lifeSpan = src.range;
            }
            else if (id == 11)
            {
                position = (src.position.row, src.position.col);
                direction = new Vector(0, 0, 0);
                sprite = new Sprite("/\\", 0, 8);
                lifeSpan = src.trail;
            }

            alignment = src.alignment;
            damage = src.damage;
        }
    }
}
