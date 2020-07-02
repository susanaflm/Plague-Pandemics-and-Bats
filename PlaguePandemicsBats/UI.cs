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
        #region Private variables
        private const float _blinkRate = 0.5f;

        private SpriteFont _spriteFont;
        private Game1 _game;
        private string _input = "";
        private float _blinkTimer = 0f;
        #endregion

        #region Constructor
        public UI(Game1 game) : base(game)
        {
            _game = game;
            _spriteFont = _game.Content.Load<SpriteFont>("minecraft");
        }
        #endregion

        #region Methods

        /// <summary>
        /// Allows the user to use the keyboard to input words
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            Keys k;

            for (k = Keys.A; k <= Keys.Z; k++)
            {
                if (KeyboardManager.IsKeyGoingDown(k))
                {
                    _input += k.ToString();
                }
            }

            if (KeyboardManager.IsKeyGoingDown(Keys.Space)) _input += " ";

            if (_input.Length > 0 && KeyboardManager.IsKeyGoingDown(Keys.Back)) _input = _input.Substring(0, _input.Length - 1);

            _game.Player.Name = _input;
        }

        /// <summary>
        /// Draws back the input of the user
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {           
            Vector2 position = new Vector2(384, 300);

            spriteBatch.DrawString(_spriteFont, _input, position, Color.Black);
            Vector2 inputSize = _spriteFont.MeasureString(_input);
            position.X += inputSize.X;

            if (_blinkTimer < _blinkRate)
                spriteBatch.DrawString(_spriteFont, "_", position, color: Color.Black);

            else if (_blinkTimer > 2 * _blinkRate)
                _blinkTimer = 0f;

            _blinkTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        }
        #endregion
    }
}
