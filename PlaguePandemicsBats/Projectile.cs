using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace PlaguePandemicsBats
{
    public class Projectile : DrawableGameComponent
    {
        private const float _projectileSpeed = 3f;
        private const float _projectileWidth = 0.2f;
        private const float _maxDistance = 5f;

        private Game1 _game;
        private Vector2 _position;
        private Vector2 _origin;
        private float _rotation;
        private Direction _direction;
        private Sprite _sprite;

        private Dictionary<Direction, Vector2> _projectileDirection;

        public Projectile(Game1 game) : base(game)
        {
            _game = game;

            _position = game.Player.Position;
            _direction = game.Player.Direction;

            _sprite = new Sprite(game, "cure", colliderType: ColliderType.OBB, width: _projectileWidth);

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
        }

        public void Shoot()
        {
            _origin = _position;
        }

        public override void Update(GameTime gameTime)
        {
            if (_sprite.obbCollider._inCollision)
            {
                if (_sprite.obbCollider.collisions[0].Tag != _game.Player.CurrentSprite._spriteName || _sprite.obbCollider.collisions.Count != 1)
                {
                    _game.Projectiles.Remove(this);
                }
            }

            float dist = Vector2.Distance(_origin, _position);

            if (dist >= _maxDistance)
            {
                _game.Projectiles.Remove(this);
            }
            else
            {
                _position += _projectileDirection[_direction] * gameTime.DeltaTime() * _projectileSpeed;
                _sprite.SetPosition(_position);
                _sprite.SetRotation(_rotation);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            _sprite.Draw(sb);
        }

    }
}
