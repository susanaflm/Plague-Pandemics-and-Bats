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
        #region Private variables
        private const float _coronaWidth = 5f;

        private Game1 _game;
        private CircleCollider _coronaCollider;

        private Vector2 _position;
        private Sprite _coronaSprite;
        private int _health;
        private float _timer;
        private float _shootTimer = 1f;
        #endregion

        #region Constructor
        public Corona(Game1 game)
        {
            _game = game;
            _position = new Vector2(305, 300);

            _health = 500;

            _coronaSprite = new Sprite(game, "borona",width: _coronaWidth);

            _coronaCollider = new CircleCollider(game, "Corona", _position, _coronaSprite.size.X / 2);
            _coronaCollider.SetDebug(true);
            game.CollisionManager.Add(_coronaCollider);
        }
        #endregion

        #region Methods

        public void Update(GameTime gameTime)
        {
            if (_health <= 0)
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

        /// <summary>
        /// This handles the corona projectiles
        /// </summary>
        /// <param name="gameTime"></param>
        public void Attack(GameTime gameTime)
        {
            _timer += gameTime.DeltaTime();

            if (_shootTimer - _timer <= 0)
            {
                Vector2 projOrientation = _game.Player.Position - _position;

                projOrientation.Normalize();

                new EnemyProjectile(_game, projOrientation, _position);

                _timer = 0;
            }
        }

        /// <summary>
        /// Sets the Position of Corona
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        /// <summary>
        /// Draws the Corona
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch sb)
        {
            _coronaSprite.Draw(sb);
        }

        #endregion
    }
}

