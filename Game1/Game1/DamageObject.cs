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
        public bool fresh = true;
        public DateTime lastUpdate;
        public (int row, int col) position;
        public Vector direction;
        public Sprite sprite;
        public int lifeSpan;
        public int alignment;
        public int damage;
        public int durability;

        public DamageObject(int id, Entity src, int spread)
        {
            this.id = id;
            lastUpdate = DateTime.Now;

            if (id == 0)
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
                else if (spread == 8)
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
                else if (spread == 9)
                {
                    if (src.direction.r == 0)
                    {
                        direction = new Vector(src.direction.c, src.direction.c * 2, 1);
                    }
                    else
                    {
                        direction = new Vector(src.direction.r * 2, -src.direction.r, 1);
                    }
                }
                else if (spread == 10)
                {
                    if (src.direction.r == 0)
                    {
                        direction = new Vector(-src.direction.c, src.direction.c * 2, 1);
                    }
                    else
                    {
                        direction = new Vector(src.direction.r * 2, src.direction.r, 1);
                    }
                }
                else if (spread == 11)
                {
                    if (src.direction.r == 0)
                    {
                        direction = new Vector(src.direction.c * 2, src.direction.c, 1);
                    }
                    else
                    {
                        direction = new Vector(src.direction.r, -src.direction.r * 2, 1);
                    }
                }
                else if (spread == 12)
                {
                    if (src.direction.r == 0)
                    {
                        direction = new Vector(-src.direction.c * 2, src.direction.c, 1);
                    }
                    else
                    {
                        direction = new Vector(src.direction.r, src.direction.r * 2, 1);
                    }
                }
                else if (spread == 13)
                {
                    if (src.direction.r == 0)
                    {
                        direction = new Vector(src.direction.c * 2, -src.direction.c, 1);
                    }
                    else
                    {
                        direction = new Vector(-src.direction.r, -src.direction.r * 2, 1);
                    }
                }
                else if (spread == 14)
                {
                    if (src.direction.r == 0)
                    {
                        direction = new Vector(-src.direction.c * 2, -src.direction.c, 1);
                    }
                    else
                    {
                        direction = new Vector(-src.direction.r, src.direction.r * 2, 1);
                    }
                }
                else if (spread == 15)
                {
                    if (src.direction.r == 0)
                    {
                        direction = new Vector(src.direction.c, -src.direction.c * 2, 1);
                    }
                    else
                    {
                        direction = new Vector(-src.direction.r * 2, -src.direction.r, 1);
                    }
                }
                else if (spread == 16)
                {
                    if (src.direction.r == 0)
                    {
                        direction = new Vector(-src.direction.c, -src.direction.c * 2, 1);
                    }
                    else
                    {
                        direction = new Vector(-src.direction.r * 2, src.direction.r, 1);
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
            else if (id == 1)
            {
                position = (src.position.row, src.position.col);
                direction = new Vector(0, 0, 0);
                sprite = new Sprite("/\\", 0, 8);
                lifeSpan = src.trail;
            }

            alignment = src.alignment;
            damage = src.damage;
            durability = src.piercing;
        }
    }
}
