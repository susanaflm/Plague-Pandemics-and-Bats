using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaguePandemicsBats
{
    class ShooterZombie : Enemy
    {
        private const float _zombieWidth = 0.4f;

        private float range = 2.5f; //Range in Meters

        public ShooterZombie(Game1 game) : base(game)
        {
            _position = new Vector2(3, 0);
            _spritesDirection = new Dictionary<Direction, Sprite[]>
            {
                [Direction.Up] = new[] { new Sprite(game, "ZGlassBoyU0", width: _zombieWidth), new Sprite(game, "ZGlassBoyU1", width: _zombieWidth), new Sprite(game, "ZGlassBoyU2", width: _zombieWidth) },
                [Direction.Down] = new[] { new Sprite(game, "ZGlassBoyD0", width: _zombieWidth), new Sprite(game, "ZGlassBoyD1", width: _zombieWidth), new Sprite(game, "ZGlassBoyD2", width: _zombieWidth) },
                [Direction.Left] = new[] { new Sprite(game, "ZGlassBoyL0", width: _zombieWidth), new Sprite(game, "ZGlassBoyL1", width: _zombieWidth), new Sprite(game, "ZGlassBoyL2", width: _zombieWidth) },
                [Direction.Right] = new[] { new Sprite(game, "ZGlassBoyR0", width: _zombieWidth), new Sprite(game, "ZGlassBoyR1", width: _zombieWidth), new Sprite(game, "ZGlassBoyR2", width: _zombieWidth) }
            };

            _health = 20;
            _damage = 0;

            _currentSprite = _spritesDirection[_direction][_frame];

            _enemyCollider = new OBBCollider(game, "Enemy", _position, _currentSprite.size, 0);
            _enemyCollider.SetDebug(true);
            game.CollisionManager.Add(_enemyCollider);
        }

        public override void Movement(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
