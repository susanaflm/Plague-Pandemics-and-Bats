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
        private Texture2D _background;
        private Vector2 _realSize;
        private Game _game;
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
            int x, y;

            Rectangle cameraBounds = new Rectangle((Camera.Target() - Camera.Size() / 2f).ToPoint(), Camera.Size().ToPoint());
            
            Vector2 center = _background.Bounds.Size.ToVector2() / 2f;

            Point bottomL = new Point(0);
            Point topR = new Point(0);

            bottomL.X = (int)_realSize.X * (((int)(cameraBounds.Width / 2f) / 2) - 1);

            while (bottomL.X > cameraBounds.Left) bottomL.X -= (int)_realSize.X;

            while (bottomL.Y > cameraBounds.Top) bottomL.Y -= (int)_realSize.Y;

            while (topR.X < cameraBounds.Right) topR.X += (int)_realSize.X;

            while (topR.Y < cameraBounds.Bottom) topR.Y += (int)_realSize.Y;
                              
            _spriteBatch.Begin();

            for( x = bottomL.X; x <= topR.X; x +=(int)_realSize.X )
            {
                for( y = bottomL.Y; y <= topR.Y; y+= (int)_realSize.Y )
                {
                    //convert coordenates from the worlsize to pixels
                    Rectangle outRec = new Rectangle(Camera.ToPixel(new Vector2(x,y)).ToPoint(), Camera.ToLength(_realSize).ToPoint());

                    _spriteBatch.Draw(texture: _background,
                     destinationRectangle: outRec,
                     sourceRectangle: null,
                     color: Color.White,
                     rotation: 0f,
                     origin: center,
                     effects: SpriteEffects.None,
                     layerDepth: 0);

                }
            }
            
            _spriteBatch.End();
        }
    }
}
