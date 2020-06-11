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
    class TilingBackground
    {
        private  Texture2D _background;

        private  Vector2 _realSize;

        private  Game _game;

        public TilingBackground(Game game, string texture, Vector2 realSize)
        {
            _game = game;

            _realSize = realSize;

            _background = game.Content.Load<Texture2D>(texture);

        }
    }
}
