using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace PlaguePandemicsBats
{
    public class Button : DrawableGameComponent
    {
        private Texture2D _texture;
        private SpriteBatch _spriteBatch;
        private Game1 _game;
        private Vector2 _position;
        private Rectangle _rec;
        private Color _color = new Color(255,255,255,255);
        private bool down;

        public bool isClicked;

        public Button(Game1 game) : base(game)
        {
            _game = game;
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
        }

        public void Load(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;
        }

        public void Update(MouseState mouseState)
        {
            mouseState = Mouse.GetState();

            _rec = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);

            Rectangle mouseRec = new Rectangle(mouseState.X, mouseState.Y, 1, 1);

            if (mouseRec.Intersects(_rec))
            {
                if (_color.A == 255) down = false;
                if (_color.A == 0) down = true;
                if (down) _color.A += 3; else _color.A -= 3;
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    isClicked = true;
                    _color.A = 255;
                }
            }
            else if (_color.A < 255)
                _color.A += 3;

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rec, _color);
        }
    }
}
