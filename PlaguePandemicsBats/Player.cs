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
    class Player : Sprite
    {
        private int _playerType;

        /// <summary>
        /// Player Constructor
        /// </summary>
        /// <param name="game"></param>
        /// <param name="playerType"></param>
        public Player(Game1 game, string name, float scale = 1f) : base(game, name, scale: scale, collides: true )
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
