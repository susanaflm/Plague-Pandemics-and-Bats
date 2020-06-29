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
        private const float _projWidth = 0.2f;

        private float range = 4f; //Range in Meters
        private Dictionary<int, Sprite> _projFrames;
        private bool isRunningAway = false;

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

            _projFrames = new Dictionary<int, Sprite>
            {
                [0]  = new Sprite(game, "ZProj1",  width: _projWidth),
                [1]  = new Sprite(game, "ZProj2",  width: _projWidth),
                [2]  = new Sprite(game, "ZProj3",  width: _projWidth),
                [3]  = new Sprite(game, "ZProj4",  width: _projWidth),
                [4]  = new Sprite(game, "ZProj5",  width: _projWidth),
                [5]  = new Sprite(game, "ZProj6",  width: _projWidth),
                [6]  = new Sprite(game, "ZProj7",  width: _projWidth),
                [7]  = new Sprite(game, "ZProj8",  width: _projWidth),
                [8]  = new Sprite(game, "ZProj9",  width: _projWidth),
                [9]  = new Sprite(game, "ZProj10", width: _projWidth),
                [10] = new Sprite(game, "ZProj11", width: _projWidth),
                [11] = new Sprite(game, "ZProj12", width: _projWidth),
                [12] = new Sprite(game, "ZProj13", width: _projWidth),
                [13] = new Sprite(game, "ZProj14", width: _projWidth),
                [14] = new Sprite(game, "ZProj15", width: _projWidth),
                [15] = new Sprite(game, "ZProj16", width: _projWidth),
            };

            _health = 20;
            _damage = 0;

            _currentSprite = _spritesDirection[_direction][_frame];

            _enemyCollider = new OBBCollider(game, "Enemy", _position, _currentSprite.size, 0);
            _enemyCollider.SetDebug(true);
            game.CollisionManager.Add(_enemyCollider);
        }

        internal override void Behaviour(GameTime gameTime)
        {
            //TODO: Shooting Range and Run Away movement
            //TODO: New Class for the enemy projectile
        }
    }
}
