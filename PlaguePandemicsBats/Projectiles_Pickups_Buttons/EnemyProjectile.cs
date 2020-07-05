using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaguePandemicsBats
{
    public class EnemyProjectile
    {
        #region Private Variables
        private const float _projWidth = 0.2f;

        private Game1 _game;
        private float _distance = 6f;
        private float _projSpeed = 3f;
        private float _deltaTime = 0f;
        private int _frame = 0;
        private int _damage = 15;

        private Vector2 _orientation;
        private Vector2 _position;
        private Vector2 _origin;

        private Sprite[] _sprites;
        private Sprite _currentSprite;
        private CircleCollider _projCollider;
        #endregion

        #region Constructor
        public EnemyProjectile(Game1 game, Vector2 orientation, Vector2 position)
        {
            _game = game;
            _orientation = orientation;

            _position = position;
            _origin = _position;

            _sprites = new Sprite[]
            {
                new Sprite(game, "ZProj1",  width: _projWidth),
                new Sprite(game, "ZProj2",  width: _projWidth),
                new Sprite(game, "ZProj3",  width: _projWidth),
                new Sprite(game, "ZProj4",  width: _projWidth),
                new Sprite(game, "ZProj5",  width: _projWidth),
                new Sprite(game, "ZProj6",  width: _projWidth),
                new Sprite(game, "ZProj7",  width: _projWidth),
                new Sprite(game, "ZProj8",  width: _projWidth),
                new Sprite(game, "ZProj9",  width: _projWidth),
                new Sprite(game, "ZProj10", width: _projWidth),
                new Sprite(game, "ZProj11", width: _projWidth),
                new Sprite(game, "ZProj12", width: _projWidth),
                new Sprite(game, "ZProj13", width: _projWidth),
                new Sprite(game, "ZProj14", width: _projWidth),
                new Sprite(game, "ZProj15", width: _projWidth),
                new Sprite(game, "ZProj16", width: _projWidth)
            };

            _currentSprite = _sprites[0];

            _projCollider = new CircleCollider(game, "EnemyProjectile", position, _sprites[0].size.X / 2);
            _projCollider.SetDebug(false);
            game.CollisionManager.Add(_projCollider);

            game.EnemyProjectiles.Add(this);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Updates the projectile
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //Delta time sum
            _deltaTime += gameTime.DeltaTime();

            //Check collisions
            if (_projCollider._inCollision)
            {
                if (_projCollider.collisions[0].Tag == "Player" || _projCollider.collisions[0].Tag == "Obstacle" || _projCollider.collisions[0].Tag == "RedTree" || _projCollider.collisions[0].Tag == "TP")
                {
                    _game.EnemyProjectiles.Remove(this);
                    _game.CollisionManager.Remove(_projCollider);
                }

                if (_projCollider.collisions[0].Tag == "Player")
                {
                    _game.Player.UpdateHealth(_damage);
                }
            }

            //Change the projectile position
            _position += _projSpeed * _orientation * gameTime.DeltaTime();

            //Updates the frame and drawn sprite
            _frame = (int)(_deltaTime * 10) % 16;

            if (_frame > 15)
            {
                _frame = 0;
            }

            _currentSprite = _sprites[_frame];
            _currentSprite.SetPosition(_position);
            _projCollider.SetPosition(_position);

            //Projectile distance dead
            if (Vector2.Distance(_origin, _position) >= _distance)
            {
                _game.EnemyProjectiles.Remove(this);
                _game.CollisionManager.Remove(_projCollider);
            }
        }

        /// <summary>
        /// Draw the projectile
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            _currentSprite.Draw(sb);
            _projCollider?.Draw(null);
        }
        #endregion
    }
}
