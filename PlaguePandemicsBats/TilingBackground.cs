using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlaguePandemicsBats
{
    public class TilingBackground
    {
        private  Texture2D _background;

        private  Vector2 _realSize;

        private  Game _game;

        private SpriteBatch _spriteBatch;

        public TilingBackground(Game game, string texture, Vector2 realSize)
        {
            _game = game;

            _realSize = realSize;

            _background = game.Content.Load<Texture2D>(texture);

            //spritebatch tem acesso à placa gráfica através do graphics device
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);

        }

        public void Draw(GameTime gameTime)
        {
            Vector2 center = _background.Bounds.Size.ToVector2() / 2f;

            //convert coordenates from the worlsize to pixels
            Rectangle outRec = new Rectangle(Camera.ToPixel(Vector2.Zero).ToPoint(), Camera.ToLength(_realSize).ToPoint());
            
            _spriteBatch.Begin();

            _spriteBatch.Draw(texture: _background, 
                destinationRectangle: outRec, 
                sourceRectangle: null,
                color: Color.White, 
                rotation: 0f, 
                origin: center, 
                effects: SpriteEffects.None, 
                layerDepth: 0);

            _spriteBatch.End();
        }
    }
}
