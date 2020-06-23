using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlaguePandemicsBats
{
    public class UI : DrawableGameComponent
    {
        private SpriteFont _spriteFont;
        private Game _game;
        private string _input = "";

        public UI(Game1 game) : base(game)
        {
            _game = game;
            _spriteFont = _game.Content.Load<SpriteFont>("font");
        }

        public override void Update(GameTime gameTime)
        {
            Keys k;

            for (k = Keys.A; k <= Keys.Z; k++)
            {
                if (KeyboardManager.IsKeyGoingDown((k)))
                {
                    _input = k.ToString();
                }
            }

            if (KeyboardManager.IsKeyGoingDown(k = Keys.Space)) _input += " ";

            if (_input.Length > 0 && KeyboardManager.IsKeyGoingDown(k = Keys.Back)) _input = _input.Substring(0, _input.Length - 1);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_spriteFont, _input, new Vector2(20, 50), color: Color.Black);
        }
    }
}
