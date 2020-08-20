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
        public DateTime lastFire;
        public (int row, int col) position;
        public Vector direction;
        public Sprite sprite;
        public int alignment;
        public int health;
        public int cost;
        public int range;
        public int fireRate = 0;
        public int damage = 1;
        public int spread = 1;
        public int piercing = 1;
        public int trail = 0;
        public int[] powerUpCounter;

        public Entity(int id)
        {
            this.id = id;
            lastUpdate = DateTime.Now;
            lastFire = DateTime.Now;

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
                (int row, int col) tempPosition = (0, 0);
                (int row, int col) playerPosition = entities[0].position;

                tempPosition = (rng.Next(0, grid.Length), rng.Next(0, grid[0].Length));

                while(entities.Exists(x => x.position == tempPosition) || damageObjects.Exists(x => x.position == tempPosition) || powerUps.Exists(x => x.position == tempPosition) || (tempPosition.row < playerPosition.row + 30 && tempPosition.row > playerPosition.row - 30 && tempPosition.col < playerPosition.col + 30 && tempPosition.col > playerPosition.col - 30))
                {
                    tempPosition = (rng.Next(0, grid.Length), rng.Next(0, grid[0].Length));
                }

                position = (tempPosition.row, tempPosition.col);
                direction = new Vector(rng.Next(-1, 2), rng.Next(-1, 2), 1);
                sprite = new Sprite("  ", 4, 0);
                alignment = 1;
                health = 1;
                cost = 1;
                range = 0;
            }

            powerUpCounter = new int[6];
        }

        public void SetDirection(int dir)
        {
            switch (dir)
            {
                case -1:
                    Entity player = entities[0];
                    bool tracking = true;

                    int r = 0;
                    int c = 0;
                    int d = 1;

                    if (position.row < player.position.row - 20 || position.row > player.position.row + 20 || position.col < player.position.col - 20 || position.col > player.position.col + 20)
                    {
                        if (rng.Next() % 100 > 7)
                        {
                            tracking = false;
                        }
                    }

                    if (tracking)
                    {
                        if (position.row < player.position.row) { r = 1; }
                        if (position.row > player.position.row) { r = -1; }

                        if (position.col < player.position.col) { c = 1; }
                        if (position.col > player.position.col) { c = -1; }
                    }
                    else
                    {
                        r = (rng.Next() % 3) - 1;
                        c = (rng.Next() % 3) - 1;
                        d = rng.Next() % 2;
                    }

                    int checks = 0;

                    while (entities.Exists(x => entities.IndexOf(x) != 0 && x.position == (position.row + r, position.col + c)) && checks < 4)
                    {
                        checks += 1;
                        r = (rng.Next() % 3) - 1;
                        c = (rng.Next() % 3) - 1;
                    }

                    if (checks == 5)
                    {
                        d = 0;
                    }

                    direction = new Vector(r, c, d);
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

        public void Fire()
        {
            if (DateTime.Now >= lastFire.AddSeconds(0.5 - (0.05 * fireRate)))
            {
                lastFire = DateTime.Now;

                for (int s = 1; s <= spread; s++)
                {
                    damageObjects.Add(new DamageObject(0, this, s));
                }
            }
        }
    }
}
