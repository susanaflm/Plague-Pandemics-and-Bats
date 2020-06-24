using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlaguePandemicsBats
{
    public class Player
    {
        #region Private variables
        private const float playerWidth = 0.3f;

        private Game1 _game;
        private int _playerGender;
        private Dictionary<Direction, Vector2> _playerDirection;
        private Dictionary<Direction, Sprite[]> _spriteDirectionMale;
        private Dictionary<Direction, Sprite[]> _spriteDirectionFemale;
        private OBBCollider _playerCollider;

        private Direction _direction = Direction.Down;
        private Vector2 _oldPosition;
        private Vector2 _position;
        private float _acceleration;
        private int _frame = 0; 
        private int _health = 100;
        private int lives = 3;
        private Sprite _currentSprite;
        #endregion

        #region Constructor
        /// <summary>
        /// Player Constructor
        /// </summary>
        /// <param name="game">Game1 Instance</param>
        /// <param name="playerGender">The gender of the character</param>
        public Player(Game1 game, int playerGender)
        {
            _playerGender = playerGender;
            _game = game;
            _position = new Vector2(0, 0);

            #region Dictionaries
            _playerDirection = new Dictionary<Direction, Vector2>
            {
                [Direction.Up] = Vector2.UnitY,
                [Direction.Down] = -Vector2.UnitY,
                [Direction.Left] = -Vector2.UnitX,
                [Direction.Right] = Vector2.UnitX
            };

            _spriteDirectionMale = new Dictionary<Direction, Sprite[]>
            {
                [Direction.Up] = new [] { new Sprite(game, "GuyU0", width: playerWidth), new Sprite(game, "GuyU1",width: playerWidth), new Sprite(game, "GuyU2", width: playerWidth) },
                [Direction.Down] = new [] { new Sprite(game, "GuyD0", width: playerWidth), new Sprite(game, "GuyD1", width: playerWidth), new Sprite(game, "GuyD2", width: playerWidth) },
                [Direction.Left] = new [] { new Sprite(game, "GuyL0", width: playerWidth), new Sprite(game, "GuyL1", width: playerWidth), new Sprite(game, "GuyL2", width: playerWidth) },
                [Direction.Right] = new [] { new Sprite(game, "GuyR0", width: playerWidth), new Sprite(game, "GuyR1", width: playerWidth), new Sprite(game, "GuyR2", width: playerWidth) }
            };

            _spriteDirectionFemale = new Dictionary<Direction, Sprite[]>
            {
                [Direction.Up] = new[] { new Sprite(game, "GirlU0", width: playerWidth), new Sprite(game, "GirlU1", width: playerWidth), new Sprite(game, "GirlU2", width: playerWidth) },
                [Direction.Down] = new[] { new Sprite(game, "GirlD0", width: playerWidth), new Sprite(game, "GirlD1", width: playerWidth), new Sprite(game, "GirlD2", width: playerWidth) },
                [Direction.Left] = new[] { new Sprite(game, "GirlL0", width: playerWidth), new Sprite(game, "GirlL1", width: playerWidth), new Sprite(game, "GirlL2", width: playerWidth) },
                [Direction.Right] = new[] { new Sprite(game, "GirlR0", width: playerWidth), new Sprite(game, "GirlR1", width: playerWidth), new Sprite(game, "GirlR2", width: playerWidth) }
            };
            #endregion

            if (_playerGender == 0)
            {
                _currentSprite = _spriteDirectionFemale[_direction][_frame];
            }
            else
            {
                _currentSprite = _spriteDirectionMale[_direction][_frame];
            }

            _playerCollider = new OBBCollider(game, "Player", _position, _currentSprite.size, rotation: 0);
            _playerCollider.SetDebug(true);
            game.CollisionManager.Add(_playerCollider);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Get the Player's Position
        /// </summary>
        public Vector2 Position => _position;

        /// <summary>
        /// Get the Player's Direction;
        /// </summary>
        public Direction Direction => _direction;

        /// <summary>
        /// Get the Player's Current Sprite;
        /// </summary>
        public Sprite CurrentSprite => _currentSprite;

        /// <summary>
        /// Get the Player's Collider
        /// </summary>
        public Collider Collider => _playerCollider;
        #endregion

        #region Methods
        public void LateUpdate(GameTime gameTime)
        {
            if (_playerCollider._inCollision)
            {
                int extraCollision = 0;

                foreach (Collider c in _playerCollider.collisions)
                {
                    if (c.Tag != "Projectile")
                    {
                        extraCollision += 1;
                    }
                }

                if (extraCollision == 2)
                {
                    _position = _oldPosition;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = gameTime.DeltaTime();
            float totalTime = gameTime.TotalTime();

            if (_playerGender == 0)
            {
                _currentSprite = _spriteDirectionFemale[_direction][_frame];
            }
            else
            {
                _currentSprite = _spriteDirectionMale[_direction][_frame];
            }

            _oldPosition = _position;

            HandleInput();

            _position += _acceleration * deltaTime * _playerDirection[_direction];
            _currentSprite.SetPosition(_position);
            _playerCollider.SetPosition(_position);

            _acceleration = 0;

            if (_oldPosition != _position)
            {
                _frame = (int) (totalTime * 6 ) % 3;
                if (_frame > 2)
                    _frame = 1;
            }
            else
                _frame = 0;

            if (_health <= 0)
                Die();

            Camera.LookAt(_position);
        }

        public void HandleInput()
        {
            if (KeyboardManager.IsKeyDown(Keys.W) || KeyboardManager.IsKeyDown(Keys.Up))
            {
                _direction = Direction.Up;
                _acceleration = 1.5f;
            }
            if (KeyboardManager.IsKeyDown(Keys.S) || KeyboardManager.IsKeyDown(Keys.Down))
            {
                _direction = Direction.Down;
                _acceleration = 1.5f;
            }
            if (KeyboardManager.IsKeyDown(Keys.A) || KeyboardManager.IsKeyDown(Keys.Left))
            {
                _direction = Direction.Left;
                _acceleration = 1.5f;
            }
            if (KeyboardManager.IsKeyDown(Keys.D) || KeyboardManager.IsKeyDown(Keys.Right))
            {
                _direction = Direction.Right;
                _acceleration = 1.5f;
            }
            if (KeyboardManager.IsKeyDown(Keys.LeftShift))
            {
                _position = Vector2.Zero;
            }
            if (KeyboardManager.IsKeyGoingDown(Keys.Space))
            {
                Projectile proj = new Projectile(_game);
                _game.Projectiles.Add(proj);
                proj.Shoot();
            }
            
        }

        /// <summary>
        /// This method allows the manipulation of the player's health
        /// </summary>
        /// <param name="damage">damage done to the player</param>
        public void UpdateHealth(int damage)
        {
            _health -= damage;
        }

        /// <summary>
        /// Initialize Dying sequence for the player
        /// </summary>
        public void Die()
        {
            lives--;

            //TODO: Go Back To checkpoint
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
            _playerCollider.SetPosition(position);
        }

        public void Draw(SpriteBatch sb)
        {
            _currentSprite.Draw(sb);
            _playerCollider?.Draw(null);
        }
        #endregion
    }
}
