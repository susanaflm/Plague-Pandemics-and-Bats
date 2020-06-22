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
    public class Cat : Enemy
    {
        private const float _catWidth = 0.4f;
        private const float _batHeight = 0.3f;
        private Player _player;

        public Player player => _player;
        public Cat(Game1 game) : base(game)
        {
            _position = new Vector2(-3, 0);
            _spritesDirection = new Dictionary<Direction, Sprite []>
            {
                [Direction.Up] = new [] { new Sprite(game, "catU0", width: 0.2f, height: _batHeight), new Sprite(game, "catU1", width: 0.2f, height: _batHeight), new Sprite(game, "catU2", width: 0.2f, height: _batHeight) },
                [Direction.Down] = new [] { new Sprite(game, "catD0", width: 0.2f, height: _batHeight), new Sprite(game, "catD1", width: 0.2f, height: _batHeight), new Sprite(game, "catD2", width: 0.2f, height: _batHeight) },
                [Direction.Left] = new [] { new Sprite(game, "catL0", width: _catWidth, height: _batHeight), new Sprite(game, "catL1", width: _catWidth, height: _batHeight), new Sprite(game, "catL2", width: _catWidth, height: _batHeight) },
                [Direction.Right] = new [] { new Sprite(game, "catR0", width: _catWidth, height: _batHeight), new Sprite(game, "catR1", width: _catWidth, height: _batHeight), new Sprite(game, "catR2", width: _catWidth, height: _batHeight) }
            };

            _acceleration = 1.1f;
            _health = 10;

            _currentSprite = _spritesDirection [_direction] [_frame];

            _enemyCollider = new OBBCollider(game, "Enemy", _position, _currentSprite.size, 0);
            _enemyCollider.SetDebug(true);
            game.CollisionManager.Add(_enemyCollider);
        }

        public override void Movement(GameTime gameTime)
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
            _position += faceDir * _acceleration * gameTime.DeltaTime();

            if (KeyboardManager.KeyState.GoingUp.Equals(Keys.A)
                || KeyboardManager.KeyState.GoingUp.Equals(Keys.W)
                || KeyboardManager.KeyState.GoingUp.Equals(Keys.S)
                || KeyboardManager.KeyState.GoingUp.Equals(Keys.D)
                || KeyboardManager.KeyState.GoingUp.Equals(Keys.Up)
                || KeyboardManager.KeyState.GoingUp.Equals(Keys.Down)
                || KeyboardManager.KeyState.GoingUp.Equals(Keys.Left)
                || KeyboardManager.KeyState.GoingUp.Equals(Keys.Right))

            {
                _position = _position;
            }

        }

        public override void LateUpdate(GameTime gameTime)
        {
            if (_enemyCollider._inCollision)
            {
                foreach (Collider c in _enemyCollider.collisions)
                {
                    if (c.Tag == "Player")
                    { 
                        if(_acceleration <= 0)
                        {
                            _acceleration = _acceleration;
                            _position = -_position;
                        }
                        else
                             _acceleration -= 0.1f; 
                       
                    }
                }

            }
        }

    }
}

