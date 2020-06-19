using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaguePandemicsBats
{
    class Bat : Enemy
    {
        public Bat(Game1 game) : base(game)
        {
            //_spritesDirection = new Dictionary<Direction, Sprite[]>
            //{
            //    [Direction.Down] = new [] { new Sprite(game, "BatFront"), new Sprite(game, "BatFrontD"), new Sprite(game, "BatFrontU") },
            //    [Direction.Left] = new [] { new Sprite(game, "BatL"), new Sprite(game, "BatFrontLD"), new Sprite(game, "BatFrontLU")},
            //    [Direction.Right] = new [] { new Sprite(game, "BatR"), new Sprite(game, "BatRD"), new Sprite(game, "BatRU")}

            //};
        }

        public override void Movement()
        {
            throw new NotImplementedException();
        }
    }
}
