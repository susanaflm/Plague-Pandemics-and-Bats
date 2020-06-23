using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaguePandemicsBats
{
    public class Cat
    {                
        private const float _catWidth = 0.4f;
        private const float _batHeight = 0.3f;
       
        private Game1 _game;
        private Vector2 _position;
        private Vector2 _oldPosition;
        private Direction _direction = Direction.Down;
        private float _acceleration;
        private OBBCollider _catCollider;
        private int _health;
        private int _damage;
        private int _frame = 0;
        private Dictionary<Direction, Sprite []> _spritesDirection;
        private Sprite _currentSprite;
        private float _deltaTime = 0;

        public Cat(Game1 game)
        {
            _game = game;
            _position = new Vector2(-3, 0);
            _spritesDirection = new Dictionary<Direction, Sprite []>
            {
                [Direction.Up] = new [] { new Sprite(game, "catU0", width: 0.2f, height: _batHeight), new Sprite(game, "catU1", width: 0.2f, height: _batHeight), new Sprite(game, "catU2", width: 0.2f, height: _batHeight) },
                [Direction.Down] = new [] { new Sprite(game, "catD0", width: 0.2f, height: _batHeight), new Sprite(game, "catD1", width: 0.2f, height: _batHeight), new Sprite(game, "catD2", width: 0.2f, height: _batHeight) },
                [Direction.Left] = new [] { new Sprite(game, "catL0", width: _catWidth, height: _batHeight), new Sprite(game, "catL1", width: _catWidth, height: _batHeight), new Sprite(game, "catL2", width: _catWidth, height: _batHeight) },
                [Direction.Right] = new [] { new Sprite(game, "catR0", width: _catWidth, height: _batHeight), new Sprite(game, "catR1", width: _catWidth, height: _batHeight), new Sprite(game, "catR2", width: _catWidth, height: _batHeight) }
            };

            _acceleration = 1.1f;
            _health = 100;
            _damage = 10;

            _currentSprite = _spritesDirection [_direction] [_frame];

            _catCollider = new OBBCollider(game, "Cat", _position, _currentSprite.size, 0);
            _catCollider.SetDebug(true);
            game.CollisionManager.Add(_catCollider);
        }
        public void SetPosition(Vector2 position)
        {
            _position = position;
            _catCollider.SetPosition(position);
        }

        public void Update(GameTime gameTime)
        {
            _deltaTime += gameTime.DeltaTime();

            _oldPosition = _position;

            Movement(gameTime);

            _frame = (int)(_deltaTime * 6) % 3;
            if (_frame > 2)
                _frame = 0;

            _currentSprite = _spritesDirection [_direction] [_frame];
            _currentSprite.SetPosition(_position);
            _catCollider.SetPosition(_position);
        }

        public void Movement(GameTime gameTime)
        {

            if (Camera.PixelSize(Vector2.Distance(_game.Player.Position, _position)) >= Camera.PixelSize(0.5f))
            {
                Vector2 faceDir = _game.Player.Position - _position;
                float angle = (float)Math.Atan2(faceDir.Y, faceDir.X);

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

                faceDir.Normalize();
                _position += faceDir * _acceleration * gameTime.DeltaTime();

            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentSprite.Draw(spriteBatch);
            _catCollider?.Draw(null);
        }

    }
}

