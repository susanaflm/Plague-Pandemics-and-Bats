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

        private float _rotation;

        public Bat(Game1 game) : base(game)
        {
            _position = new Vector2(3, 0);
            _spritesDirection = new Dictionary<Direction, Sprite[]>
            {
                [Direction.Up] = new [] {new Sprite(game, "BatU", width: _batWidth), new Sprite(game, "BatUD", width: _batWidth), new Sprite(game, "BatUU", width: _batWidth) },
                [Direction.Down] = new[] { new Sprite(game, "BatFront", width: _batWidth), new Sprite(game, "BatFrontD", width: _batWidth), new Sprite(game, "BatFrontU", width: _batWidth) },
                [Direction.Left] = new[] { new Sprite(game, "BatL", width: _batWidth), new Sprite(game, "BatLD", width: _batWidth), new Sprite(game, "BatLU", width: _batWidth) },
                [Direction.Right] = new[] { new Sprite(game, "BatR", width: _batWidth), new Sprite(game, "BatRD", width: _batWidth), new Sprite(game, "BatRU", width: _batWidth) }
            };

            _acceleration = 1.2f;
            _health = 10;
            _damage = 5;

            _currentSprite = _spritesDirection[_direction][_frame];

            _enemyCollider = new CircleCollider(game, "Enemy", _position, _currentSprite.size.X >= _currentSprite.size.Y ? _currentSprite.size.X / 2f : _currentSprite.size.Y / 2f);
            _enemyCollider.SetDebug(true);
            game.CollisionManager.Add(_enemyCollider);
        }

        public override void Movement(GameTime gameTime)
        {
            Vector2 faceDir = _game.Player.Position - _position;
            float angle = (float) Math.Atan2(faceDir.Y, faceDir.X);

            _rotation = angle;

            faceDir.Normalize();

            _position += faceDir * _acceleration * gameTime.DeltaTime();
            _currentSprite.SetPosition(_position);
            _enemyCollider.SetPosition(_position);
        }
    }
}
