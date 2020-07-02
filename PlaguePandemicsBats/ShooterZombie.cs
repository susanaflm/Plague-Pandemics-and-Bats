using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaguePandemicsBats
{
    //Shooter Enemy Type
    class ShooterZombie : Enemy
    {
        #region Variables
        private const float _zombieWidth = 0.4f;
        private const float _projWidth = 0.2f;

        private float _range = 4f; //Range in Meters
        private float _shootTimer = 2;
        private float _timer;
        private bool isRunningAway = false;
        private bool isShootingAvailable = false;
        #endregion

        #region Constructor
        public ShooterZombie(Game1 game, Vector2 position) : base(game)
        {
            _position = position;

            _spritesDirection = new Dictionary<Direction, Sprite[]>
            {
                [Direction.Up] = new[] { new Sprite(game, "ZGuyU0", width: _zombieWidth), new Sprite(game, "ZGuyU1", width: _zombieWidth), new Sprite(game, "ZGuyU2", width: _zombieWidth) },
                [Direction.Down] = new[] { new Sprite(game, "ZGuyD0", width: _zombieWidth), new Sprite(game, "ZGuyD1", width: _zombieWidth), new Sprite(game, "ZGuyD2", width: _zombieWidth) },
                [Direction.Left] = new[] { new Sprite(game, "ZGuyL0", width: _zombieWidth), new Sprite(game, "ZGuyL1", width: _zombieWidth), new Sprite(game, "ZGuyL2", width: _zombieWidth) },
                [Direction.Right] = new[] { new Sprite(game, "ZGuyR0", width: _zombieWidth), new Sprite(game, "ZGuyR1", width: _zombieWidth), new Sprite(game, "ZGuyR2", width: _zombieWidth) }
            };

            _score = 800;
            _health = 30;
            _damage = 0;
            _acceleration = 1f;

            _currentSprite = _spritesDirection[_direction][_frame];

            _enemyCollider = new OBBCollider(game, "Enemy", _position, _currentSprite.size, 0);
            _enemyCollider.SetDebug(false);
            game.CollisionManager.Add(_enemyCollider);
        }
        #endregion

        #region Methods
        internal override void Behaviour(GameTime gameTime)
        {
            //Checks the Distance to the player to know when to Run
            if (Vector2.DistanceSquared(_position, _game.Player.Position) <= 1.5 * 1.5)
            {
                isRunningAway = true;

                Vector2 runDirection = _position - _game.Player.Position;
                float angle = (float)Math.Atan2(runDirection.Y, runDirection.X);

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
                runDirection.Normalize();

                _position += runDirection * gameTime.DeltaTime() * _acceleration;
            }
            else
            {
                isRunningAway = false;
            }

            //If the Player is in Range to the enemy, it shoots
            if (!isRunningAway && Vector2.DistanceSquared(_position, _game.Player.Position) <= _range * _range)
            {
                _timer += gameTime.DeltaTime();

                //Direction in which the zombie will shoot the projectile
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

                //Timer to see when to shoot
                if (_shootTimer - _timer <= 0)
                {
                    isShootingAvailable = true;
                    _timer = 0;
                }

                //Checks if it can shoot according to the timer
                if (isShootingAvailable)
                {
                    new EnemyProjectile(_game, projOrientation, _position);
                    isShootingAvailable = false;
                }
            }
        }
        #endregion
    }
}
