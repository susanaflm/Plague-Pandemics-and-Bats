using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaguePandemicsBats
{
    public abstract class Enemy 
    {
        protected Game1 _game;

        internal Vector2 _position;
        internal Vector2 _oldPosition;
        internal Direction _direction = Direction.Down;
        internal float _acceleration;
        internal CircleCollider _enemyCollider;
        internal int _health;
        internal int _damage;
        internal Dictionary<Direction, Sprite[]> _spritesDirection;

        private int _frame = 0;
        private Dictionary<Direction, Vector2> _enemyDirection;
        private Sprite _currentSprite;

        public Enemy(Game1 game)
        {
            _game = game;

            _enemyDirection = new Dictionary<Direction, Vector2>
            {
                [Direction.Up] = Vector2.UnitY,
                [Direction.Down] = -Vector2.UnitY,
                [Direction.Left] = -Vector2.UnitX,
                [Direction.Right] = Vector2.UnitX
            };

            _currentSprite = _spritesDirection[_direction][_frame];

            _enemyCollider = new CircleCollider(game, "Enemy", _position, _currentSprite.size.X >= _currentSprite.size.Y ? _currentSprite.size.X / 2f : _currentSprite.size.Y / 2f);
            _enemyCollider.SetDebug(true);
            game.CollisionManager.Add(_enemyCollider);

        }

        public abstract void Movement();

        internal void SetPosition(Vector2 position)
        {
            _position = position;
        }

        public void LateUpdate(GameTime gameTime)
        {
            if (_enemyCollider._inCollision)
            {
                foreach (Collider c in _enemyCollider.collisions)
                {
                    if (c.Tag == "Player")
                    {
                        _game.Player.UpdateHealth(_damage);
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = gameTime.DeltaTime();
            float totalTime = gameTime.TotalTime();

            _oldPosition = _position;

            Movement();

            _position += _acceleration * _enemyDirection[_direction] * deltaTime;

            if (_oldPosition != _position)
            {
                _frame = (int)(totalTime * 6) % 3;
                if (_frame > 2)
                    _frame = 1;
            }
            else
                _frame = 0;
        }

        public void Draw(SpriteBatch sb)
        {
            _currentSprite.Draw(sb);
            _enemyCollider?.Draw(null);
        }

    }
}
