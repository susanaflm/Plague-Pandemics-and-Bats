using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaguePandemicsBats
{
    class SpawnerZombie : Enemy
    {
        private const float _zombieWidth = 0.5f;

        private int _spawnQuantity = 3;
        private float _spawnRange = 3;
        private List<Bat> _spawnedBats;
        private bool _isSpawnAvailable = false;
        private float _spawnTimer = 1;
        private float _timer;

        public SpawnerZombie(Game1 game, Vector2 position) : base(game)
        {
            _position = position;
            _spritesDirection = new Dictionary<Direction, Sprite[]>
            {
                [Direction.Up] = new[] { new Sprite(game, "ZGuyU0", width: _zombieWidth), new Sprite(game, "ZGuyU1", width: _zombieWidth), new Sprite(game, "ZGuyU2", width: _zombieWidth) },
                [Direction.Down] = new[] { new Sprite(game, "ZGuyD0", width: _zombieWidth), new Sprite(game, "ZGuyD1", width: _zombieWidth), new Sprite(game, "ZGuyD2", width: _zombieWidth) },
                [Direction.Left] = new[] { new Sprite(game, "ZGuyL0", width: _zombieWidth), new Sprite(game, "ZGuyL1", width: _zombieWidth), new Sprite(game, "ZGuyL2", width: _zombieWidth) },
                [Direction.Right] = new[] { new Sprite(game, "ZGuyR0", width: _zombieWidth), new Sprite(game, "ZGuyR1", width: _zombieWidth), new Sprite(game, "ZGuyR2", width: _zombieWidth) }
            };

            _spawnedBats = new List<Bat>();

            _acceleration = 0.8f;
            _health = 20;
            _damage = 0;

            _currentSprite = _spritesDirection[_direction][_frame];

            _enemyCollider = new OBBCollider(game, "Enemy", _position, _currentSprite.size, 0);
            _enemyCollider.SetDebug(true);
            game.CollisionManager.Add(_enemyCollider);
        }

        internal override void Behaviour(GameTime gameTime)
        {
            //Spawning Method, within a Range to the Player
            if (Vector2.Distance(_position, _game.Player.Position) <= _spawnRange)
            {
                _timer += gameTime.DeltaTime();

                Vector2 orientation = _game.Player.Position - _position ;

                float angle = (float)Math.Atan2(orientation.Y, orientation.X);

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

                if (_spawnTimer - _timer <= 0 )
                {
                    _isSpawnAvailable = true;
                    _timer = 0;
                }

                if (_spawnedBats.Count < _spawnQuantity && _isSpawnAvailable)
                {
                    Bat bat = new Bat(_game, Vector2.Add(_position, new Vector2(0, _currentSprite.size.Y) * _enemyDirection[_direction]));
                    _spawnedBats.Add(bat);
                    _isSpawnAvailable = false;
                }

                foreach (Bat b in _spawnedBats.ToArray())
                {
                    if (b.isDead)
                    {
                        _spawnedBats.Remove(b);
                    }
                }

            }

            

            
        }
    }
}
