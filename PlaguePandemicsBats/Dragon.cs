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
        private float _acceleration = 2f;
        private OBBCollider _dragonCollider;
        private int _frame = 0;
        private Dictionary<Direction, Sprite []> _spritesDirection;
        private Dictionary<Direction, Vector2> _dragonDirection;
        private Sprite _currentSprite;
        private float _deltaTime = 0;
        #endregion

        #region Public variables
        public bool isDragonAlive = true;
        public bool dragonAttackActive = false;
        #endregion

        #region Constructor
        public Dragon(Game1 game) 
        {
            _game = game;
            _position = new Vector2(-2, 0);
            _direction = game.Player.Direction;

            #region Dictionary
            _spritesDirection = new Dictionary<Direction, Sprite []>
            {
                [Direction.Up] = new [] { new Sprite(game, "DragonBack", height: _dragonHeight), new Sprite(game, "DragonBackU", height: _dragonHeight), new Sprite(game, "DragonBackD", height: _dragonHeight) },
                [Direction.Down] = new [] { new Sprite(game, "DragonFront", height: _dragonHeight), new Sprite(game, "DragonFrontU", height: _dragonHeight), new Sprite(game, "DragonFrontD", height: _dragonHeight) },
                [Direction.Left] = new [] { new Sprite(game, "DragonL", height: _dragonHeight), new Sprite(game, "DragonLU", height: _dragonHeight), new Sprite(game, "DragonLD", height: _dragonHeight) },
                [Direction.Right] = new [] { new Sprite(game, "DragonR", height: _dragonHeight), new Sprite(game, "DragonRU", height: _dragonHeight), new Sprite(game, "DragonRD", height: _dragonHeight) }
            };

            _dragonDirection = new Dictionary<Direction, Vector2>
            {
                [Direction.Up] = Vector2.UnitY,
                [Direction.Down] = -Vector2.UnitY,
                [Direction.Left] = -Vector2.UnitX,
                [Direction.Right] = Vector2.UnitX
            };
            #endregion

            _currentSprite = _spritesDirection [_direction] [_frame];

            //COLLIDERS
            _dragonCollider = new OBBCollider(game, "Dragon", _position, _currentSprite.size, 0);
            _dragonCollider.SetDebug(false);
            game.CollisionManager.Add(_dragonCollider);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Property to be used when accessing the dragon sprite Size
        /// </summary>
        public Vector2 SpriteSize => _currentSprite.size;
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
        /// updates the dragon's frames and movement
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (isDragonAlive)
            {
                _deltaTime += gameTime.DeltaTime();

                _oldPosition = _position;

                if (dragonAttackActive)
                {
                    DragonAttack(gameTime, _game.Player.Direction);
                }

                _frame = (int)(_deltaTime * 6) % 3;
                if (_frame > 2)
                    _frame = 0;

                _currentSprite = _spritesDirection[_direction][_frame];

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
                        Die();
                    }
                }
            }
        }

        /// <summary>
        /// This Function handles the movement of the Dragon when called
        /// </summary>
        private void DragonAttack(GameTime gameTime, Direction dir)
        {
            _direction = dir;

            _position += _dragonDirection[_direction] * _acceleration * gameTime.DeltaTime();

            if (Vector2.DistanceSquared(_position, _game.Player.Position) >= 36)
            {
                Die();
            }
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
        public void Die()
        {
            isDragonAlive = false;
            _game.CollisionManager.Remove(_dragonCollider);          
        }

        #endregion
    }
}
