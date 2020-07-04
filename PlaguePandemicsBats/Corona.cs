using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace PlaguePandemicsBats
{
    public class Corona
    {
        private Game1 _game;
        private Texture2D _texture;
        private SpriteBatch _spriteBatch;
        private Rectangle _rec;
        private Vector2 _position;
        private int _score;
        private int _health;
        private int _damage;
       
        public Corona(Game1 game, Vector2 position) 
        {
            _game = game;            
            _texture = _game.Content.Load<Texture2D>("borona");
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            _position = position;

            _score = 200;
            _health = 500;
            _damage = 30;
        }

        #region Methods

        public void Update(GameTime gameTime)
        {
            if(_health == 0)
            {
                _game.CoronaDied();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {
            if(_game.hasPlayerTouchedBlueHouse)
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                _spriteBatch.Draw(_texture, _rec, Color.White);
                _spriteBatch.End();
            }
            
        }

        #endregion
    }
}

