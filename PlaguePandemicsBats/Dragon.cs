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
    public class Dragon 
    {
        #region Private variables
        private const float _dragonHeight = 1.5f;

        private Game1 _game;
        private Vector2 _position;
        private Vector2 _oldPosition;
        private Direction _direction = Direction.Down;
        private float _acceleration;
        private OBBCollider _dragonCollider;
        private int _health;
        private int _frame = 0;
        private Dictionary<Direction, Sprite []> _spritesDirection;
        private Sprite _currentSprite;
        private float _deltaTime = 0;
        #endregion

        #region Public variables
        public bool isDragonAlive = true;
        #endregion

        #region Constructor
        public Dragon(Game1 game) 
        {
            _game = game;
            _position = new Vector2(-2, 0);
            _health = 100;

            #region Dictionary
            _spritesDirection = new Dictionary<Direction, Sprite []>
            {
                [Direction.Up] = new [] { new Sprite(game, "DragonBack", height: _dragonHeight), new Sprite(game, "DragonBackU", height: _dragonHeight), new Sprite(game, "DragonBackD", height: _dragonHeight) },
                [Direction.Down] = new [] { new Sprite(game, "DragonFront", height: _dragonHeight), new Sprite(game, "DragonFrontU", height: _dragonHeight), new Sprite(game, "DragonFrontD", height: _dragonHeight) },
                [Direction.Left] = new [] { new Sprite(game, "DragonL", height: _dragonHeight), new Sprite(game, "DragonLU", height: _dragonHeight), new Sprite(game, "DragonLD", height: _dragonHeight) },
                [Direction.Right] = new [] { new Sprite(game, "DragonR", height: _dragonHeight), new Sprite(game, "DragonRU", height: _dragonHeight), new Sprite(game, "DragonRD", height: _dragonHeight) }
            };
            #endregion
           
            _currentSprite = _spritesDirection [_direction] [_frame];

            //COLLIDERS
            _dragonCollider = new OBBCollider(game, "_dragon", _position, _currentSprite.size, 0);
            _dragonCollider.SetDebug(false);
            game.CollisionManager.Add(_dragonCollider);
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
            _dragonCollider.SetPosition(position);
        }

        /// <summary>
        /// updates the _dragons frames and movement
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if(isDragonAlive)
            {
                _deltaTime += gameTime.DeltaTime();

                _oldPosition = _position;

                if(_game.Player.isDragonHiden = false)
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
                _dragonCollider.SetPosition(_position);
            }
            
        }

        /// <summary>
        /// Updates the colision effects
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void LateUpdate(GameTime gameTime)
        {
            if (_dragonCollider._inCollision && isDragonAlive)
            {
                foreach (Collider c in _dragonCollider.collisions)
                {
                    if (c.Tag == "Enemy")
                    {
                        _health = 0;
                        _position = _oldPosition;
                        this.Die();
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
        /// Draws the dragon
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isDragonAlive)
            {
                _currentSprite.Draw(spriteBatch);
                _dragonCollider?.Draw(null);
            }            
        }

        /// <summary>
        /// Kills the dragon
        /// </summary>
        public  void Die()
        {
            isDragonAlive = false;
            _game.CollisionManager.Remove(_dragonCollider);          
        }

        #endregion
    }
}
