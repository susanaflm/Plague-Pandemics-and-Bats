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
        private Direction _direction = Direction.Down;
        private OBBCollider _coronaCollider;
        private Rectangle _rec;
        private Vector2 _position;
        private int _score;
        private int _health;
        private int _damage;
        private float _timer;

        public Corona(Game1 game, Vector2 position)
        {
            _game = game;
            _texture = _game.Content.Load<Texture2D>("borona");
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            _position = position;

            _score = 200;
            _health = 500;
            _damage = 30;

            _coronaCollider = new OBBCollider(game, "Corona", _position, _texture.Bounds.Size.ToVector2(), 0);
            _coronaCollider.SetDebug(false);
            game.CollisionManager.Add(_coronaCollider);
        }

        #region Methods

        public void Update(GameTime gameTime)
        {
            if (_health == 0)
            {
                _game.CoronaDied();
            }
        }

        public void Attack(GameTime gameTime)
        {
            _timer += gameTime.DeltaTime();

            Vector2 projOrientation = _game.Player.Position - _position;

            float angle = (float)Math.Atan2(projOrientation.Y, projOrientation.X);

            if (angle <= -3 * Math.PI / 4)
                _direction = Direction.Left;
            else if (angle <= -Math.PI / 4)
                _direction = Direction.Down;
            else if (angle <= Math.PI / 4)
                _direction = Direction.Right;
            else if (angle <= 3 * Math.PI / 4)
                _direction = Direction.Up;
            else
                _direction = Direction.Left;

            projOrientation.Normalize();

            new EnemyProjectile(_game, projOrientation, _position);
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

