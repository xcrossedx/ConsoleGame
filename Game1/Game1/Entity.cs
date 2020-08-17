using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Entity : ObjectManager
    {
        public int id;
        public DateTime lastUpdate;
        public (int row, int col) position;
        public Vector direction;
        public Sprite sprite;
        public int alignment;
        public int health;
        public int cost;
        public int range;
        public int damage = 1;
        public int spread = 1;
        public int trail = 0;

        public Entity(int id)
        {
            this.id = id;
            lastUpdate = DateTime.Now;

            if (id == 0)
            {
                position = (grid.Length / 2, grid[0].Length / 2);
                direction = new Vector(0, 0, 0);
                sprite = new Sprite("  ", 7, 0);
                alignment = 0;
                health = 10;
                cost = 0;
                range = 10;
            }
            else if (id == 1)
            {
                if (rng.Next() % 2 == 0)
                {
                    if (rng.Next() % 2 == 0)
                    {
                        position = (0, rng.Next(0, grid[0].Length));
                    }
                    else
                    {
                        position = (grid.Length - 1, rng.Next(0, grid[0].Length));
                    }
                }
                else
                {
                    if (rng.Next() % 2 == 0)
                    {
                        position = (rng.Next(0, grid.Length), 0);
                    }
                    else
                    {
                        position = (rng.Next(0, grid.Length), grid[0].Length - 1);
                    }
                }

                while(entities.Exists(x => x.position == position) || damageObjects.Exists(x => x.position == position) || powerUps.Exists(x => x.position == position))
                {
                    if (rng.Next() % 2 == 0)
                    {
                        if (rng.Next() % 2 == 0)
                        {
                            position = (0, rng.Next(0, grid[0].Length));
                        }
                        else
                        {
                            position = (grid.Length - 1, rng.Next(0, grid[0].Length));
                        }
                    }
                    else
                    {
                        if (rng.Next() % 2 == 0)
                        {
                            position = (rng.Next(0, grid.Length), 0);
                        }
                        else
                        {
                            position = (rng.Next(0, grid.Length), grid[0].Length - 1);
                        }
                    }
                }

                direction = new Vector(rng.Next(-1, 2), rng.Next(-1, 2), 1);
                sprite = new Sprite("  ", 4, 0);
                alignment = 1;
                health = 1;
                cost = 1;
                range = 0;
            }
        }

        public void SetDirection(int d)
        {
            switch (d)
            {
                case -1:
                    if (Math.Abs(entities[0].position.row - position.row) > Math.Abs(entities[0].position.col - position.col))
                    {
                        if (entities[0].position.row > position.row)
                        {
                            direction = new Vector(1, 0, 1);
                        }
                        else
                        {
                            direction = new Vector(-1, 0, 1);
                        }
                    }
                    else if (Math.Abs(entities[0].position.row - position.row) < Math.Abs(entities[0].position.col - position.col))
                    {
                        if (entities[0].position.col > position.col)
                        {
                            direction = new Vector(0, 1, 1);
                        }
                        else
                        {
                            direction = new Vector(0, -1, 1);
                        }
                    }
                    else
                    {
                        int r = 0;
                        int c = 0;

                        if (entities[0].position.row > position.row)
                        {
                            r = 1;
                        }
                        else
                        {
                            r = -1;
                        }

                        if (entities[0].position.col > position.col)
                        {
                            c = 1;
                        }
                        else
                        {
                            c = -1;
                        }

                        direction = new Vector(r, c, 1);
                    }
                    break;
                case 0:
                    direction = new Vector(-1, 0, 1);
                    break;
                case 1:
                    direction = new Vector(0, 1, 1);
                    break;
                case 2:
                    direction = new Vector(1, 0, 1);
                    break;
                case 3:
                    direction = new Vector(0, -1, 1);
                    break;
            }
        }
    }
}
