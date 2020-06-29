using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaguePandemicsBats
{
    class Bat : Enemy
    {

        private const float _batWidth = 0.2f;
        private const float _batHeight = 0.3f;

        //private int _state = 0;
        public Bat(Game1 game, Vector2 position) : base(game)
        {
            _position = position;
            _spritesDirection = new Dictionary<Direction, Sprite[]>
            {
                [Direction.Up] = new [] {new Sprite(game, "BatU", width: _batWidth, height: _batHeight), new Sprite(game, "BatUD", width: _batWidth, height: _batHeight), new Sprite(game, "BatUU", width: _batWidth, height: _batHeight) },
                [Direction.Down] = new[] { new Sprite(game, "BatFront", width: _batWidth, height: _batHeight), new Sprite(game, "BatFrontD", width: _batWidth, height: _batHeight), new Sprite(game, "BatFrontU", width: _batWidth, height: _batHeight) },
                [Direction.Left] = new[] { new Sprite(game, "BatL", width: _batWidth, height: _batHeight), new Sprite(game, "BatLD", width: _batWidth, height: _batHeight), new Sprite(game, "BatLU", width: _batWidth, height: _batHeight) },
                [Direction.Right] = new[] { new Sprite(game, "BatR", width: _batWidth, height: _batHeight), new Sprite(game, "BatRD", width: _batWidth, height: _batHeight), new Sprite(game, "BatRU", width: _batWidth, height: _batHeight) }
            };

            _acceleration = 1.2f;
            _health = 10;
            _damage = 5;

            _currentSprite = _spritesDirection[_direction][_frame];

            _enemyCollider = new OBBCollider(game, "Enemy", _position, _currentSprite.size, 0);
            _enemyCollider.SetDebug(true);
            game.CollisionManager.Add(_enemyCollider);
        }

        internal override void Behaviour(GameTime gameTime)
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
            
        }
    }
}
