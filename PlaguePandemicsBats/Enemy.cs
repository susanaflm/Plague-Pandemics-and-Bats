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
        internal OBBCollider _enemyCollider;
        internal int _health;
        internal int _damage;   
        internal int _frame = 0;
        internal Dictionary<Direction, Sprite[]> _spritesDirection;
        internal Sprite _currentSprite;
        internal Dictionary<Direction, Vector2> _enemyDirection;
        internal bool isDead = false;

        private float _deltaTime = 0;

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

            _game.Enemies.Add(this);
        }

        /// <summary>
        /// This Function handles the movement of the enemy
        /// </summary>
        /// <param name="gameTime"></param>
        internal abstract void Behaviour(GameTime gameTime);

        internal void SetPosition(Vector2 position)
        {
            _position = position;
            _enemyCollider.SetPosition(position);
        }

        public virtual void LateUpdate(GameTime gameTime)
        {
            if (_enemyCollider._inCollision)
            {
                foreach (Collider c in _enemyCollider.collisions)
                {
                    if (c.Tag == "Player")
                    {
                        _game.Player.UpdateHealth(_damage);
                        _position = _oldPosition;
                    }
                    if (c.Tag == "Projectile")
                    {
                        _health -= 10;
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            _deltaTime += gameTime.DeltaTime();

            _oldPosition = _position;

            Behaviour(gameTime);
           
            _frame = (int)(_deltaTime * 6) % 3;
            if (_frame > 2)
                _frame = 0;

            _currentSprite = _spritesDirection[_direction][_frame];
            _currentSprite.SetPosition(_position);
            _enemyCollider.SetPosition(_position);

            if (_health <= 0)
            {
                isDead = true;
                Die();
            }
        }

        internal void Die()
        {
            _game.CollisionManager.Remove(_enemyCollider);
            _game.Enemies.Remove(this);
        }

        public void Draw(SpriteBatch sb)
        {
            _currentSprite.Draw(sb);
            _enemyCollider?.Draw(null);
        }

    }
}
