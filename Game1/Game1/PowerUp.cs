using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class PowerUp : ObjectManager
    {
        public int id;
        public DateTime lastUpdate;
        public (int row, int col) position;
        public Sprite defaultSprite;
        public Sprite flashSprite;
        public int strength;
        public int cost;
        public int lifeSpan;

        public PowerUp(int id)
        {
            this.id = id;
            lastUpdate = DateTime.Now;
            position = (rng.Next(0, grid.Length), rng.Next(0, grid[0].Length));

            while (entities.Exists(x => x.position == position) || damageObjects.Exists(x => x.position == position) || powerUps.Exists(x => x.position == position))
            {
                position = (rng.Next(0, grid.Length), rng.Next(0, grid[0].Length));
            }

            if (id == 20)
            {
                defaultSprite = new Sprite("()", 0, 7);
                flashSprite = new Sprite("()", 0, 15);
                strength = 1;
                cost = 1;
            }
            else if (id == 21)
            {
                defaultSprite = new Sprite("<>", 0, 7);
                flashSprite = new Sprite("<>", 0, 15);
                strength = 2;
                cost = 1;
            }
            else if (id == 22)
            {
                defaultSprite = new Sprite("^^", 0,2);
                flashSprite = new Sprite("^^", 0, 15);
                strength = 2;
                cost = 3;
            }
            else if (id == 23)
            {
                defaultSprite = new Sprite("||", 0, 2);
                flashSprite = new Sprite("||", 0, 15);
                strength = 1;
                cost = 3;
            }
            else if (id == 24)
            {
                defaultSprite = new Sprite("{}", 0, 5);
                flashSprite = new Sprite("{}", 0, 15);
                strength = 2;
                cost = 5;
            }

            lifeSpan = 1000;
        }
    }
}
