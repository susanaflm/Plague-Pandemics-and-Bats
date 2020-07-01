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
    public class Dragon : Cat
    {
        #region Private variables
        private const float __dragonWidth = 0.4f;
        private const float __dragonHeight = 0.3f;

        private Game1 _game;
        private Vector2 _position;
        private Vector2 _oldPosition;
        private Direction _direction = Direction.Down;
        private float _acceleration;
        private OBBCollider __dragonCollider;
        private int _health;
        private int _frame = 0;
        private Dictionary<Direction, Sprite []> _spritesDirection;
        private Sprite _currentSprite;
        private float _deltaTime = 0;
        #endregion

        #region Constructor
        public Dragon(Game1 game) : base(game)
        {
            _game = game;
            _position = new Vector2(-3, 0);
            _spritesDirection = new Dictionary<Direction, Sprite []>
            {
                [Direction.Up] = new [] { new Sprite(game, "DragonBack", width: 0.2f, height: __dragonHeight), new Sprite(game, "DragonBackU", width: 0.2f, height: __dragonHeight), new Sprite(game, "DragonBackD", width: 0.2f, height: __dragonHeight) },
                [Direction.Down] = new [] { new Sprite(game, "DragonFront", width: 0.2f, height: __dragonHeight), new Sprite(game, "DragonFrontU", width: 0.2f, height: __dragonHeight), new Sprite(game, "DragonFrontD", width: 0.2f, height: __dragonHeight) },
                [Direction.Left] = new [] { new Sprite(game, "DragonL", width: __dragonWidth, height: __dragonHeight), new Sprite(game, "DragonLU", width: __dragonWidth, height: __dragonHeight), new Sprite(game, "DragonLD", width: __dragonWidth, height: __dragonHeight) },
                [Direction.Right] = new [] { new Sprite(game, "DragonR", width: __dragonWidth, height: __dragonHeight), new Sprite(game, "DragonRU", width: __dragonWidth, height: __dragonHeight), new Sprite(game, "DragonRD", width: __dragonWidth, height: __dragonHeight) }
            };

            _health = 100;

            _currentSprite = _spritesDirection [_direction] [_frame];

            __dragonCollider = new OBBCollider(game, "_dragon", _position, _currentSprite.size, 0);
            __dragonCollider.SetDebug(true);
            game.CollisionManager.Add(__dragonCollider);
        }
        #endregion

        #region Methods

        /// <summary>
        /// sets the _dragons position
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector2 position)
        {
            _position = position;
            __dragonCollider.SetPosition(position);
        }

        /// <summary>
        /// updates the _dragons frames and movement
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
                _currentSprite = _spritesDirection [_direction] [0];
            }
            else
            {
                _currentSprite = _spritesDirection [_direction] [_frame];
            }

            _currentSprite.SetPosition(_position);
            __dragonCollider.SetPosition(_position);
        }

        /// <summary>
        /// Updates the colision effects
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void LateUpdate(GameTime gameTime)
        {
            if (__dragonCollider._inCollision)
            {
                foreach (Collider c in __dragonCollider.collisions)
                {
                    if (c.Tag == "Enemy")
                    {
                        _health = 0;
                        _position = _oldPosition;
                    }

                }
            }
        }

        /// <summary>
        /// moves the _dragon according to the frames and angle
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
        /// Draws the _dragon
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            _currentSprite.Draw(spriteBatch);
            __dragonCollider?.Draw(null);
        }

        #endregion
    }
}
