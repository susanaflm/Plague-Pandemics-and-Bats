using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PlaguePandemicsBats
{
    public class Button : DrawableGameComponent
    {
        #region Private variables
        private Texture2D _texture;
        private Game1 _game;

        private Vector2 _position;
        private Vector2 _origin;
        private Rectangle _rec;
        private Color _color = new Color(255,255,255,255);
        private bool down;
        #endregion

        #region Public variables
        public bool isClicked;
        #endregion

        #region Constructor
        public Button(Game1 game, Texture2D texture, Vector2 position) : base(game)
        { 
            _game = game;
            _texture = texture;
            _position = position;

            _origin = _texture.Bounds.Size.ToVector2() / 2;
        }
        #endregion

        #region Methods
        /// <summary>
        /// shifts the colors of the buttons depending on the position of the mouse
        /// </summary>
        /// <param name="mouse"></param>
        public void Update(MouseState mouse)
        {
            Rectangle mouseRec = new Rectangle(mouse.X, mouse.Y, 1, 1);

            mouse = Mouse.GetState();
            _rec = new Rectangle((int)_position.X - (_texture.Width / 2), (int)_position.Y - (_texture.Height / 2), _texture.Width, _texture.Height);
        
            if (mouseRec.Intersects(_rec))
            {
                _color = new Color(180, 180, 180, 255);
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    isClicked = true;
                }
            }
            else
                _color = Color.White;
        }

        /// <summary>
        /// Draws the sprites
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture,
                new Rectangle(_position.ToPoint(), _texture.Bounds.Size),
                null,
                _color,
                rotation: 0,
                _origin,
                SpriteEffects.None,
                0);
        }
        #endregion
    }
}
