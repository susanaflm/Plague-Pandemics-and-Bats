using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlaguePandemicsBats
{
    public class Pixel 
    {
        #region Private variables
        private static Pixel _instance;
        private Texture2D _pixel;
        private SpriteBatch _spriteBatch;
        #endregion

        #region Constructor
        public Pixel(Game game) {
            //only creates 1 instance of pixel
            if (_instance != null) throw new Exception("Pixel constructor called twice");

            _instance = this;

            _pixel = new Texture2D(game.GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new Color[]{ Color.White });

            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }
        #endregion

        #region Methods
        public static void Draw(Vector2 position, Color color) 
        {  
            _instance._Draw(Camera.ToPixel(position), color);
        }
         public static void DrawLine(Vector2 a, Vector2 b, Color color)
         {
             Vector2 pixelA = Camera.ToPixel(a);
             Vector2 pixelB = Camera.ToPixel(b);
             _instance._DrawLine(pixelA.ToPoint(), pixelB.ToPoint(), color);
         }

        public static void DrawRectangle(Rectangle rectangle, Color color)
        {   
            _instance._Rectangle(rectangle, color);
        }

        public static void DrawCircle(Vector2 center, float radius, Color color) { 
            center = Camera.ToPixel(center);
            radius = Camera.PixelSize(radius);
            _instance._DrawCircle(center, radius, color);
        }

        void _DrawCircle(Vector2 center, float radius, Color color) {
            _spriteBatch.Begin();
            float delta = 1f / radius;
            for (float theta = 0; theta <= 2 * Math.PI; theta += delta) {
                float x = (float)(center.X + radius * Math.Cos(theta));
                float y = (float)(center.Y + radius * Math.Sin(theta));
                _spriteBatch.Draw(_pixel, new Vector2(x, y), color);
            }
            _spriteBatch.End();
        }

        void _Rectangle(Rectangle rectangle, Color color) {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_pixel, rectangle, color);
            _spriteBatch.End();
        }

        void _Draw(Vector2 position, Color color) {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_pixel, position, color);
            _spriteBatch.End();
        }

        void _DrawLine(Point o, Point t, Color c) {
            if (o.Y == t.Y) {
                // Horizontal Line
                _Rectangle(new Rectangle(Math.Min(o.X, t.X), o.Y, Math.Abs(o.X - t.X), 1), c);
            }
            else if (o.X == t.X) {
                // Vertical Line
                _Rectangle(new Rectangle(o.X, Math.Min(o.Y, t.Y), 1, Math.Abs(o.Y - t.Y)), c);
            }
            else {
                float m = ((float)(t.Y - o.Y)) / (t.X - o.X);
                float b = o.Y - m * o.X;

                _spriteBatch.Begin();
                if (Math.Abs(o.X - t.X) > Math.Abs(o.Y - t.Y)) {
                    // Reta essencialmente horizontal
                    int xMin = Math.Min(o.X, t.X);
                    int xMax = Math.Max(o.X, t.X);
                
                    for (int x = xMin; x <= xMax; x++) {
                        int y = (int)(m * x + b);
                        _spriteBatch.Draw(_pixel, new Vector2(x, y), c);
                    }                    
                } else {
                    // Reta essencialmente vertical
                    int yMin = Math.Min(o.Y, t.Y);
                    int yMax = Math.Max(o.Y, t.Y);

                    for (int y = yMin; y <= yMax; y++) {
                        int x = (int)( (y - b) / m );
                        _spriteBatch.Draw(_pixel, new Vector2(x, y), c);
                    }
                }
                _spriteBatch.End();
            }
        }
        #endregion
    }
}