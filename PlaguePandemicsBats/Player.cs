using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlaguePandemicsBats
{
    class Player : DrawableGameComponent
    {
        private int _playerType;

        public Player(Game1 game, int playerType) : base(game)
        {
            _playerType = playerType;
        }
    }
}
