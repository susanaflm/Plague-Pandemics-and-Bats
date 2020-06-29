using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        private SoundEffect _dieSound;

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

            _dieSound = _game.Content.Load<SoundEffect>("die");
            _game.Enemies.Add(this);
        }

        /// <summary>
        /// This Function handles the movement of the enemy
        /// </summary>
        /// <param name="gameTime"></param>
        internal abstract void Behaviour(GameTime gameTime);

        /// <summary>
        /// Function to be used when setting the enemy's position
        /// </summary>
        /// <param name="position"></param>
        internal void SetPosition(Vector2 position)
        {
            _position = position;
            _enemyCollider.SetPosition(position);
        }

        /// <summary>
        /// Update that occurs after the collider check
        /// </summary>
        /// <param name="gameTime"></param>
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

        /// <summary>
        /// Update Function
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _deltaTime += gameTime.DeltaTime();

            _oldPosition = _position;

            if (Vector2.DistanceSquared(_position, _game.Player.Position) <= 36f)
                Behaviour(gameTime);

            if (_position != _oldPosition)
            {
                _frame = (int)(_deltaTime * 6) % 3;
                if (_frame > 2)
                    _frame = 0;
            }
            
            _currentSprite = _spritesDirection[_direction][_frame];
            _currentSprite.SetPosition(_position);
            _enemyCollider.SetPosition(_position);

            if (_health <= 0)
            {
                isDead = true;
                Die();
            }
        }

        /// <summary>
        /// Handles the enemy dying sequence
        /// </summary>
        internal void Die()
        {
            _dieSound.Play();
            _game.CollisionManager.Remove(_enemyCollider);
            _game.Enemies.Remove(this);
        }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            _currentSprite.Draw(sb);
            _enemyCollider?.Draw(null);
        }

    }
}
