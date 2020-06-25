using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaguePandemicsBats
{
    public class PinkZombie : Enemy
    {
        private const float _zombieWidth = 0.4f;

        private float _patrolDistance = 1f;
        private Vector2 _originPosition;

        public PinkZombie(Game1 game, Vector2 position) : base(game)
        {
            _position = position;
            _originPosition = _position;

            _spritesDirection = new Dictionary<Direction, Sprite[]>
            {
                [Direction.Up] = new[] { new Sprite(game, "ZGirlU0", width: _zombieWidth), new Sprite(game, "ZGirlU1", width: _zombieWidth), new Sprite(game, "ZGirlU2", width: _zombieWidth) },
                [Direction.Down] = new[] { new Sprite(game, "ZGirlD0", width: _zombieWidth), new Sprite(game, "ZGirlD1", width: _zombieWidth), new Sprite(game, "ZGirlD2", width: _zombieWidth) },
                [Direction.Left] = new[] { new Sprite(game, "ZGirlL0", width: _zombieWidth), new Sprite(game, "ZGirlL1", width: _zombieWidth), new Sprite(game, "ZGirlL2", width: _zombieWidth) },
                [Direction.Right] = new[] { new Sprite(game, "ZGirlR0", width: _zombieWidth), new Sprite(game, "ZGirlR1", width: _zombieWidth), new Sprite(game, "ZGirlR2", width: _zombieWidth) }
            };

            _acceleration = 0.5f;
            _health = 50;
            _damage = 20;
            _direction = Direction.Up;

            _currentSprite = _spritesDirection[_direction][_frame];

            _enemyCollider = new OBBCollider(game, "Enemy", _position, _currentSprite.size, 0);
            _enemyCollider.SetDebug(true);
            game.CollisionManager.Add(_enemyCollider);
        }

        public override void Movement(GameTime gameTime)
        {
            //TODO: Patrol Movement

            _position += _acceleration * gameTime.DeltaTime() * _enemyDirection[_direction];

            float dist = Camera.PixelSize(Vector2.Distance(_originPosition, _position));

            if (dist >= Camera.PixelSize(_patrolDistance))
                _direction = Direction.Down;
            else if (dist <= 0)
                _direction = Direction.Up;
        }
    }
}
