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
    public class MainMenu
    {
        private Texture2D _texture;
        private Vector2 _position;
        private Rectangle _rec;
        private bool down;

        private Color _color = new Color(255, 255, 255, 255);

        public Vector2 size;
        public bool isClicked;

        public MainMenu(Texture2D texture, GraphicsDevice graphicsDevice)
        {
            _texture = texture;
            size = new Vector2(graphicsDevice.Viewport.Width / 8, graphicsDevice.Viewport.Height / 30);
        }

        public void Update(MouseState mouse)
        {
            Rectangle mouseRec = new Rectangle(mouse.X, mouse.Y, 1, 1);
            
            _rec = new Rectangle((int)_position.X, (int)_position.Y, (int)size.X, (int)size.Y);

            if (mouseRec.Intersects(_rec))
            {
                if (_color.A == 255) down = false;
                if (_color.A == 0) down = true;
                if (down) _color.A += 3; else _color.A -= 3;
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    isClicked = true;
                    _color.A = 255;
                }
            }
            else if (_color.A < 255)
            {
                _color.A += 3;
                isClicked = false;
            }
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rec, _color);
        }
    }
}
