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
    public class Corona : DrawableGameComponent
    {
        #region Private variables
        private readonly Game1 _game;
        private readonly Texture2D _texture;
        private readonly SpriteBatch _spriteBatch;
        private readonly CircleCollider _coronaCollider;

        private Direction _direction = Direction.Down;
        private Rectangle _rec;
        private Vector2 _position;
        private int _health;
        private float _timer;
        #endregion

        #region Constructor
        public Corona(Game1 game) : base(game)
        {
            _game = game;
            _texture = _game.Content.Load<Texture2D>("borona");
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            _position = game.Player.Position;

            _health = 500;

            _coronaCollider = new CircleCollider(_game, "Corona", new Vector2(_texture.Width - _texture.Width / 2), _texture.Width / 2f);
            _coronaCollider.SetDebug(true);
            game.CollisionManager.Add(_coronaCollider);
        }
        #endregion

        #region Methods

        public override void Update(GameTime gameTime)
        {
            if (_health == 0)
            {
                _game.CoronaDied();
            }

            Attack(gameTime);
        }

        public void LateUpdate(GameTime gameTime)
        {
            if (_coronaCollider._inCollision)
            {
                if (_coronaCollider.collisions[0].Tag == "Player")
                {
                    _game.Player.Die();
                }

                if (_coronaCollider.collisions[0].Tag == "Projectile")
                {
                    _health -= 100;
                }
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

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_texture, _rec, Color.White);
            _spriteBatch.End();
        }

        #endregion
    }
}

