using System;
using Microsoft.Xna.Framework;

namespace PlaguePandemicsBats
{
    public class Camera {

        public static Camera instance; 
        Vector2 pxSize; // Monogame window size
        Vector2 wdSize; // Monogame window in worldsize
        Vector2 ratio;
        Vector2 target;
     
        public static Vector2 Size() {
            return instance.wdSize;
        }
       
        public Camera(Game game, Vector2 worldSize)
        {
            pxSize = new Vector2(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            AuxConstructor(game, worldSize);
        }

        public Camera(Game game, float worldWidth = 0f, float worldHeight = 0f) {
            pxSize = new Vector2(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
          
            if (worldWidth == 0 && worldHeight == 0)
                throw new Exception("Camera called with zero dimensions");

            if (worldWidth == 0)
                worldWidth = pxSize.X * worldHeight / pxSize.Y;
            if (worldHeight == 0)
                worldHeight = pxSize.Y * worldWidth / pxSize.X;
            AuxConstructor(game, new Vector2(worldWidth, worldHeight));
        }

        private void AuxConstructor(Game game, Vector2 worldSize) {
            if (instance != null)
                throw new Exception("Singleton called twice");
            instance = this;

            wdSize = worldSize;
            
            ratio = pxSize / wdSize;
            target = Vector2.Zero;
        }

        private void UpdateRatio() { ratio = pxSize / wdSize; }

        public static void Zoom(float zoom) {
            instance.wdSize *= zoom;
            instance.UpdateRatio();
        }

        // Recebe coordenadas do mundo "real" (p.ex., em metros, km, whatever) e converte para pixeis
        public static Vector2 ToPixel(Vector2 pos)
        {
            return new Vector2(
                (pos.X - instance.target.X + instance.wdSize.X / 2f) * instance.ratio.X,
                instance.pxSize.Y - (pos.Y - instance.target.Y + instance.wdSize.Y / 2f) * instance.ratio.Y);
        }

        public static Vector2 ToLength(Vector2 len)
        {
            return len * instance.ratio;
        }

        public static float PixelSize(float x)
        {
            return x * instance.ratio.X;
        }

        public static void LookAt(Vector2 tgt) {
            instance.target = tgt;
        }
        public static Vector2 Target()
        {
            return instance.target;
        }

    }
}