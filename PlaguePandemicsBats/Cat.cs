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
        private const float _catHeight = 0.3f;
       
        private Game1 _game;
        private Vector2 _position;
        private Vector2 _oldPosition;
        private Direction _direction = Direction.Down;
        private OBBCollider _catCollider;
        private int _frame = 0;
        private int _damage = 10;
        private List<Enemy> _inRangeEnemies;
        private Dictionary<Direction, Sprite []> _spritesDirection;
        private Sprite _currentSprite;
        private float _acceleration;
        private float _deltaTime = 0;
        private float _timer = 0;
        private float _damageTimer = 2f;
        private bool _isFollowingPlayer = false;
        private bool _isCatAbleToAttack = false;
        #endregion

        #region Constructor
        public Cat(Game1 game)
        {
            _game = game;
            _position = new Vector2(-3, 0);

            _inRangeEnemies = new List<Enemy>();

            _spritesDirection = new Dictionary<Direction, Sprite []>
            {
                [Direction.Up] = new [] { new Sprite(game, "catU0", width: 0.2f, height: _catHeight), new Sprite(game, "catU1", width: 0.2f, height: _catHeight), new Sprite(game, "catU2", width: 0.2f, height: _catHeight) },
                [Direction.Down] = new [] { new Sprite(game, "catD0", width: 0.2f, height: _catHeight), new Sprite(game, "catD1", width: 0.2f, height: _catHeight), new Sprite(game, "catD2", width: 0.2f, height: _catHeight) },
                [Direction.Left] = new [] { new Sprite(game, "catL0", width: _catWidth, height: _catHeight), new Sprite(game, "catL1", width: _catWidth, height: _catHeight), new Sprite(game, "catL2", width: _catWidth, height: _catHeight) },
                [Direction.Right] = new [] { new Sprite(game, "catR0", width: _catWidth, height: _catHeight), new Sprite(game, "catR1", width: _catWidth, height: _catHeight), new Sprite(game, "catR2", width: _catWidth, height: _catHeight) }
            };

            _currentSprite = _spritesDirection [_direction] [_frame];

            _catCollider = new OBBCollider(game, "Cat", _position, _currentSprite.size, 0);
            _catCollider.SetDebug(false);
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
            _currentSprite.SetPosition(position);
            _catCollider.SetPosition(position);
        }

        /// <summary>
        /// updates the cats frames and movement
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _deltaTime += gameTime.DeltaTime();
            _timer += gameTime.DeltaTime();

            _oldPosition = _position;

            if (_isFollowingPlayer)
            {
                //Filter the enemies list from game according to a range
                _inRangeEnemies = _game.Enemies.Where(e => Vector2.DistanceSquared(e._position, _position) <= 4).ToList();

                if (_inRangeEnemies.Count == 0)
                {
                    Movement(gameTime);

                    //If the cat gets Stuck in a corner or is too far, teleport to the player position
                    if (Vector2.DistanceSquared(_position, _game.Player.Position) >= 16f)
                        SetPosition(_game.Player.Position);
                }
                else
                {
                    Attack(gameTime);

                    //Repeated because bugs
                    if (Vector2.DistanceSquared(_position, _game.Player.Position) >= 16f)
                        SetPosition(_game.Player.Position);
                }

                
            }

            _frame = (int)(_deltaTime * 6) % 3;
            if (_frame > 2)
                _frame = 1;

            if (_oldPosition == _position)
                _currentSprite = _spritesDirection[_direction][0];
            else
                _currentSprite = _spritesDirection[_direction][_frame];

            if (_damageTimer - _timer <= 0 && !_isCatAbleToAttack)
            {
                _isCatAbleToAttack = true;
                _timer = 0;
            }


            //Set the Sprite and Collider position
            _currentSprite.SetPosition(_position);
            _catCollider.SetPosition(_position);
        }

        /// <summary>
        /// Updates the collision effects
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
                        _position = _oldPosition;

                        if (_isCatAbleToAttack)
                        {
                            _inRangeEnemies[0].DamageEnemy(_damage);
                            _isCatAbleToAttack = false;
                        }
                    }

                    //This is turned off, the cat will move through obstacles, because its a cat
                    //if (c.Tag == "Obstacle")
                    //{
                    //   // _position = _oldPosition;
                    //}

                    if (c.Tag == "Player")
                    {
                        _isFollowingPlayer = true;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Cat Movement. It will follow the player wherever he goes
        /// </summary>
        /// <param name="gameTime"></param>
        private void Movement(GameTime gameTime)
        {
            //Calculate the direction to face
            Vector2 faceDir = _game.Player.Position - _position;
            float angle = (float)Math.Atan2(faceDir.Y, faceDir.X);

            //Aply that direction to one of the general ones
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

            //Move the cat towards the player
            _position += faceDir * _acceleration * gameTime.DeltaTime();
        }

        private void Attack(GameTime gameTime)
        {
            //The list will be ordered according to the closest enemy
            _inRangeEnemies.OrderBy(e => Vector2.DistanceSquared(e._position, _position));

            //Compute the Direction to the closest enemy
            Vector2 faceDir = _inRangeEnemies[0]._position - _position;
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
            _acceleration = 1.2f;

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

