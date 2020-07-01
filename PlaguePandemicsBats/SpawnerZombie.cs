using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaguePandemicsBats
{
    //Spawner Enemy Type
    class SpawnerZombie : Enemy
    {
        #region Variables
        private const float _zombieheight = 0.55f;

        private int _spawnQuantity = 3;
        private float _spawnRange = 3;        
        private float _spawnTimer = 1;
        private float _timer;
        private List<Bat> _spawnedBats;
        private bool _isSpawnAvailable = false;
        #endregion

        #region Contructor
        public SpawnerZombie(Game1 game, Vector2 position) : base(game)
        {
            _position = position;
            _spritesDirection = new Dictionary<Direction, Sprite[]>
            {
                [Direction.Up] = new[] { new Sprite(game, "ZGlassBoyU0", height: _zombieheight), new Sprite(game, "ZGlassBoyU1", height: _zombieheight), new Sprite(game, "ZGlassBoyU2", height: _zombieheight) },
                [Direction.Down] = new[] { new Sprite(game, "ZGlassBoyD0", height: _zombieheight), new Sprite(game, "ZGlassBoyD1", height: _zombieheight), new Sprite(game, "ZGlassBoyD2", height: _zombieheight) },
                [Direction.Left] = new[] { new Sprite(game, "ZGlassBoyL0", height: _zombieheight), new Sprite(game, "ZGlassBoyL1", height: _zombieheight), new Sprite(game, "ZGlassBoyL2", height: _zombieheight) },
                [Direction.Right] = new[] { new Sprite(game, "ZGlassBoyR0", height: _zombieheight), new Sprite(game, "ZGlassBoyR1", height: _zombieheight), new Sprite(game, "ZGlassBoyR2", height: _zombieheight) }
            };

            _spawnedBats = new List<Bat>();

            _score = 400;
            _acceleration = 0.8f;
            _health = 20;
            _damage = 0;

            _currentSprite = _spritesDirection[_direction][_frame];

            _enemyCollider = new OBBCollider(game, "Enemy", _position, _currentSprite.size, 0);
            _enemyCollider.SetDebug(true);
            game.CollisionManager.Add(_enemyCollider);
        }
        #endregion

        #region Methods
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

                //Timer to Make the enemy spawn a bat
                if (_spawnTimer - _timer <= 0 )
                {
                    _isSpawnAvailable = true;
                    _timer = 0;
                }

                //Spawns a Bat and adds to the list of spawned Bats if the spawn is available
                if (_spawnedBats.Count < _spawnQuantity && _isSpawnAvailable)
                {
                    Bat bat = new Bat(_game, Vector2.Add(_position, new Vector2(0, _currentSprite.size.Y) * _enemyDirection[_direction]));
                    _spawnedBats.Add(bat);
                    _isSpawnAvailable = false;
                }

                foreach (Bat b in _spawnedBats.ToArray())
                {
                    //Check if the bat died and removes it from the list
                    if (b.isDead)
                        _spawnedBats.Remove(b);
                }
            }
        }
        #endregion
    }
}
