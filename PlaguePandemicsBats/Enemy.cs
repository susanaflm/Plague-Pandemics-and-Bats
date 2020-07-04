using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlaguePandemicsBats
{
    //Enemy base Class
    public abstract class Enemy 
    {
        #region Variables
        protected Game1 _game;

        internal Vector2 _position;
        internal Vector2 _oldPosition;
        internal Direction _direction = Direction.Down;
        internal float _acceleration;
        internal OBBCollider _enemyCollider;
        internal int _health;
        internal int _score;
        internal int _damage;   
        internal int _frame = 0;
        
        internal Dictionary<Direction, Sprite[]> _spritesDirection;
        internal Sprite _currentSprite;
        internal Dictionary<Direction, Vector2> _enemyDirection;
        internal bool isDead = false;

        private float _deltaTime = 0;
        private SoundEffect _dieSound;
        private float _timeToDamage = 1;
        private float _timer = 0;
        private bool _isEnemyAbleToDamage = false;
        #endregion

        #region Constructor
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
        #endregion

        #region Methods
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
                    //Check Colliders, and decide for each type
                    if (c.Tag == "Player")
                    {
                        if (_isEnemyAbleToDamage)
                        {
                            _game.Player.UpdateHealth(_damage);
                            _isEnemyAbleToDamage = false;
                        }
                        _position = _oldPosition;
                    }

                    if (c.Tag == "Projectile")
                    {
                        _health -= 10;
                    }

                    if (c.Tag == "Obstacle" || c.Tag == "RedTree" || c.Tag == "TP")
                    {
                        _position = _oldPosition;
                    }

                    if (c.Tag == "Dragon")
                    {
                        Die();
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
            _timer += gameTime.DeltaTime();

            _oldPosition = _position;

            //Optimization of the game, the enemies will only move if the player is within 6 meters 
            //Also it will not cause problems with the colliders
            if (Vector2.DistanceSquared(_position, _game.Player.Position) <= 36f)
                Behaviour(gameTime);

            //Validates if the enemy is still or moving
            if (_position != _oldPosition)
            {
                _frame = (int)(_deltaTime * 6) % 3;
                if (_frame > 2)
                    _frame = 0;
            }
            
            _currentSprite = _spritesDirection[_direction][_frame];
            _currentSprite.SetPosition(_position);
            _enemyCollider.SetPosition(_position);

            //Time to damage the player
            if (_timeToDamage - _timer <= 0)
            {
                _timer = 0;
                _isEnemyAbleToDamage = true;
            }

            //If the enemy's health goes below zero it dies
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
            _game.Player.UpdateScore(_score);
            _dieSound.Play();
            _game.CollisionManager.Remove(_enemyCollider);
            _game.Enemies.Remove(this);
        }

        /// <summary>
        /// Allows some classes to damage the player
        /// </summary>
        /// <param name="damageAmount"></param>
        public void DamageEnemy(int damageAmount)
        {
            _health -= damageAmount;
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
        #endregion
    }
}
