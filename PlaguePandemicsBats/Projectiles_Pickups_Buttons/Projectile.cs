using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace PlaguePandemicsBats
{
    public class Projectile
    {
        #region Private Variables
        private const float _projectileSpeed = 3f;
        private const float _projectileWidth = 0.12f;
        private const float _maxDistance = 4f;

        private Game1 _game;
        private Vector2 _position;
        private Vector2 _origin;
        private float _rotation;
        private Direction _direction;
        private Sprite _sprite;
        private OBBCollider _projectileCollider;

        private Dictionary<Direction, Vector2> _projectileDirection;
        #endregion

        #region Constructor
        public Projectile(Game1 game)
        {
            _game = game;

            _position = game.Player.Position;
            _direction = game.Player.Direction;

            _sprite = new Sprite(game, "cure", width: _projectileWidth);

            if (_direction == Direction.Down)
                _rotation = 0;
            else if (_direction == Direction.Up)
                _rotation = (float)Math.PI;
            else if (_direction == Direction.Right)
                _rotation = (float)(Math.PI * 1.5);
            else
                _rotation = (float)(Math.PI / 2);

            _projectileDirection = new Dictionary<Direction, Vector2>
            {
                [Direction.Up] = Vector2.UnitY,
                [Direction.Down] = -Vector2.UnitY,
                [Direction.Left] = -Vector2.UnitX,
                [Direction.Right] = Vector2.UnitX
            };

            _projectileCollider = new OBBCollider(game, "Projectile", _position, _sprite.size / 2f, _rotation);
            _projectileCollider.SetDebug(false);
            game.CollisionManager.Add(_projectileCollider);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set the origin of the projectile
        /// </summary>
        public void Shoot()
        {
            _origin = _position;
        }

        /// <summary>
        /// Update the projectile
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //Check collisions
            if (_projectileCollider._inCollision)
            {
                foreach (Collider c in _projectileCollider.collisions)
                {
                    if (c.Tag == "Enemy" || c.Tag == "Obstacle" || c.Tag == "RedTree" || c.Tag == "TP" || c.Tag == "Corona")
                    {
                        _game.Projectiles.Remove(this);
                        _game.CollisionManager.Remove(_projectileCollider);
                    }

                    if (c.Tag == "Corona")
                    {
                        _game.corona.DealDamage(100);
                    }
                }
               
            }

            //Distancing dead
            //If the projectile went too far from its origin the projectile dies
            //Otherwise change the projectile position
            float dist = Vector2.Distance(_origin, _position);

            if (dist >= _maxDistance)
            {
                _game.Projectiles.Remove(this);
                _game.CollisionManager.Remove(_projectileCollider);
            }
            else
            {
                _position += _projectileDirection[_direction] * gameTime.DeltaTime() * _projectileSpeed;
                _sprite.SetPosition(_position);
                _sprite.SetRotation(_rotation);
                _projectileCollider.SetPosition(_position);
                _projectileCollider.Rotate(_rotation);
            }
        }

        /// <summary>
        /// Draw the projectile
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            _sprite.Draw(sb);
            _projectileCollider?.Draw(null);
        }
        #endregion
    }
}
