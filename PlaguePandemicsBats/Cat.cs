using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaguePandemicsBats
{
    public class Cat
    {
        #region Private variables
        private const float _catWidth = 0.4f;
        private const float _batHeight = 0.3f;
       
        private Game1 _game;
        private Vector2 _position;
        private Vector2 _oldPosition;
        private Direction _direction = Direction.Down;
        private float _acceleration;
        private OBBCollider _catCollider;
        private int _health;
        private int _damage;
        private int _frame = 0;
        private Dictionary<Direction, Sprite []> _spritesDirection;
        private Sprite _currentSprite;
        private float _deltaTime = 0;
        #endregion

        #region Constructor
        public Cat(Game1 game)
        {
            _game = game;
            _position = new Vector2(-3, 0);
            _spritesDirection = new Dictionary<Direction, Sprite []>
            {
                [Direction.Up] = new [] { new Sprite(game, "catU0", width: 0.2f, height: _batHeight), new Sprite(game, "catU1", width: 0.2f, height: _batHeight), new Sprite(game, "catU2", width: 0.2f, height: _batHeight) },
                [Direction.Down] = new [] { new Sprite(game, "catD0", width: 0.2f, height: _batHeight), new Sprite(game, "catD1", width: 0.2f, height: _batHeight), new Sprite(game, "catD2", width: 0.2f, height: _batHeight) },
                [Direction.Left] = new [] { new Sprite(game, "catL0", width: _catWidth, height: _batHeight), new Sprite(game, "catL1", width: _catWidth, height: _batHeight), new Sprite(game, "catL2", width: _catWidth, height: _batHeight) },
                [Direction.Right] = new [] { new Sprite(game, "catR0", width: _catWidth, height: _batHeight), new Sprite(game, "catR1", width: _catWidth, height: _batHeight), new Sprite(game, "catR2", width: _catWidth, height: _batHeight) }
            };

            _health = 100;
            _damage = 10;

            _currentSprite = _spritesDirection [_direction] [_frame];

            _catCollider = new OBBCollider(game, "Cat", _position, _currentSprite.size, 0);
            _catCollider.SetDebug(true);
            game.CollisionManager.Add(_catCollider);
        }
        #endregion

        #region Methods

        /// <summary>
        /// sets the cats position
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector2 position)
        {
            _position = position;
            _catCollider.SetPosition(position);
        }

        /// <summary>
        /// updates the cats frames and movement
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _deltaTime += gameTime.DeltaTime();

            _oldPosition = _position;

            Movement(gameTime);

            _frame = (int)(_deltaTime * 6) % 3;
            if (_frame > 2)
                _frame = 1;

            if (_oldPosition == _position)
            {
                _currentSprite = _spritesDirection[_direction][0];
            }
            else
            {
                _currentSprite = _spritesDirection[_direction][_frame];
            }

            _currentSprite.SetPosition(_position);
            _catCollider.SetPosition(_position);
        }

        /// <summary>
        /// Updates the colision effects
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void LateUpdate(GameTime gameTime)
        {
            if (_catCollider._inCollision)
            {
                foreach (Collider c in _catCollider.collisions)
                {
                    if (c.Tag == "Enemy")
                    {
                        _health -= 10;
                        _position = _oldPosition;
                    }
                   
                }
            }
        }

        /// <summary>
        /// moves the cat according to the frames and angle
        /// </summary>
        /// <param name="gameTime"></param>
        public void Movement(GameTime gameTime)
        {
            Vector2 faceDir = _game.Player.Position - _position;
            float angle = (float)Math.Atan2(faceDir.Y, faceDir.X);

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

            faceDir.Normalize();

            float dist = Vector2.Distance(_game.Player.Position, _position);

            if (Camera.PixelSize(dist) >= Camera.PixelSize(1f))
                _acceleration = 1.1f;
            else if (Camera.PixelSize(dist) >= Camera.PixelSize(0.5f))
                _acceleration *= 0.97f;
            else
                _acceleration = 0f;

            _position += faceDir * _acceleration * gameTime.DeltaTime();
        }

        /// <summary>
        /// Draws the cat
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            _currentSprite.Draw(spriteBatch);
            _catCollider?.Draw(null);
        }

        #endregion
    }
}

