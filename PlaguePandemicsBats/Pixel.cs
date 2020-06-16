using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlaguePandemicsBats
{
    public class Pixel {
        // SINGLETON!!
        static Pixel instance;
        Texture2D pixel;
        SpriteBatch spriteBatch;

        public Pixel(Game game) {
            // Garantir que se obtem erro ao criar mais que uma instancia de pixel
            if (instance != null) throw new Exception("Pixel constructor called twice");

            instance = this;

            pixel = new Texture2D(game.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[]{ Color.White });

            spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        public static void Draw(Vector2 position, Color color) 
        {  
            instance._Draw(Camera.ToPixel(position), color);
        }
         public static void DrawLine(Vector2 a, Vector2 b, Color color)
         {
             Vector2 pixelA = Camera.ToPixel(a);
             Vector2 pixelB = Camera.ToPixel(b);
             instance._DrawLine(pixelA.ToPoint(), pixelB.ToPoint(), color);
         }

        public static void DrawRectangle(Rectangle rectangle, Color color)
        {   // FIXME !!! Camera
            //Vector2 recPosition = Camera.ToPixel(rectangle.Location.ToVector2());
            //Vector2 recSize = Camera.ToLength(rectangle.Size.ToVector2());

            //Rectangle pixelRectangle = new Rectangle(recPosition.ToPoint(), recSize.ToPoint());

            instance._Rectangle(rectangle, color);
        }

        public static void DrawCircle(Vector2 center, float radius, Color color) { // FIXME!! Camera
            center = Camera.ToPixel(center);
            radius = Camera.PixelSize(radius);
            instance._DrawCircle(center, radius, color);
        }

        void _DrawCircle(Vector2 center, float radius, Color color) {
            spriteBatch.Begin();
            float delta = 1f / radius;
            for (float theta = 0; theta <= 2 * Math.PI; theta += delta) {
                float x = (float)(center.X + radius * Math.Cos(theta));
                float y = (float)(center.Y + radius * Math.Sin(theta));
                spriteBatch.Draw(pixel, new Vector2(x, y), color);
            }
            spriteBatch.End();
        }

        void _Rectangle(Rectangle rectangle, Color color) {
            spriteBatch.Begin();
            spriteBatch.Draw(pixel, rectangle, color);
            spriteBatch.End();
        }

        void _Draw(Vector2 position, Color color) {
            spriteBatch.Begin();
            spriteBatch.Draw(pixel, position, color);
            spriteBatch.End();
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

                spriteBatch.Begin();
                if (Math.Abs(o.X - t.X) > Math.Abs(o.Y - t.Y)) {
                    // Reta essencialmente horizontal
                    int xMin = Math.Min(o.X, t.X);
                    int xMax = Math.Max(o.X, t.X);
                
                    for (int x = xMin; x <= xMax; x++) {
                        int y = (int)(m * x + b);
                        spriteBatch.Draw(pixel, new Vector2(x, y), c);
                    }                    
                } else {
                    // Reta essencialmente vertical
                    int yMin = Math.Min(o.Y, t.Y);
                    int yMax = Math.Max(o.Y, t.Y);

                    for (int y = yMin; y <= yMax; y++) {
                        int x = (int)( (y - b) / m );
                        spriteBatch.Draw(pixel, new Vector2(x, y), c);
                    }
                }
                spriteBatch.End();
            }
        }
    }
}