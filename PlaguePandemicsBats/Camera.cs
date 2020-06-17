using System;
using Microsoft.Xna.Framework;

namespace PlaguePandemicsBats
{
    public class Camera {

        public static Camera instance; 

        // Monogame window size
        private Vector2 _pxSize; 

        // Monogame window in worldsize
        private Vector2 _wdSize; 
       
        private Vector2 _ratio;
        
        private Vector2 _target;       
           
        public Camera(Game game, Vector2 worldSize)
        {
            _pxSize = new Vector2(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);

            AuxConstructor(game, worldSize);
        }

        public Camera(Game game, float worldWidth = 0f, float worldHeight = 0f) 
        {
            _pxSize = new Vector2(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
          
            if (worldWidth == 0 && worldHeight == 0)
                throw new Exception("Camera called with zero dimensions");

            if (worldWidth == 0)
                worldWidth = _pxSize.X * worldHeight / _pxSize.Y;

            if (worldHeight == 0)
                worldHeight = _pxSize.Y * worldWidth / _pxSize.X;

            AuxConstructor(game, new Vector2(worldWidth, worldHeight));
        }

        private void AuxConstructor(Game game, Vector2 worldSize) 
        {
            if (instance != null)
                throw new Exception("Singleton called twice");

            instance = this;

            _wdSize = worldSize;
            
            _ratio = _pxSize / _wdSize;

            _target = Vector2.Zero;
        }

        public static Vector2 Size() 
        {
            return instance._wdSize;
        }

        private void UpdateRatio() 
        {
            _ratio = _pxSize / _wdSize; 
        }

        public static void Zoom(float zoom) 
        {
            instance._wdSize *= zoom;

            instance.UpdateRatio();
        }

        // Recebe coordenadas do mundo "real" (p.ex., em metros, km, whatever) e converte para pixeis
        public static Vector2 ToPixel(Vector2 pos)
        {
            return new Vector2(
                (pos.X - instance._target.X + instance._wdSize.X / 2f) * instance._ratio.X,
                instance._pxSize.Y - (pos.Y - instance._target.Y + instance._wdSize.Y / 2f) * instance._ratio.Y);
        }

        public static Vector2 ToLength(Vector2 len)
        {
            return len * instance._ratio;
        }

        public static float PixelSize(float x)
        {
            return x * instance._ratio.X;
        }

        public static void LookAt(Vector2 tgt) {
            instance._target = tgt;
        }
        public static Vector2 Target()
        {
            return instance._target;
        }

    }
}