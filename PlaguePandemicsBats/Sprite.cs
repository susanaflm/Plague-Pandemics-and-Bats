using System;
using IPCA.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IPCA.Collision;

namespace CarsGame
{
    public class Sprite
    {
        private Texture2D texture; // Texture da SpriteSheet
        internal Rectangle bounds; // Posição na SpriteSheet
        private Vector2 origin;

        internal Vector2 size; // Tamanho em unidades "reais"
        internal Vector2 position; // Posição no ambiente "real"
        internal float rotation = 0f;
        internal OBBCollider collider;
        internal Game1 _game;
        internal Color _color = Color.White;

        public Sprite(Game1 game, string name, float width = 0, float height = 0, float scale = 0, bool collides = false)
        {
            _game = game;

            try
            {
                texture = game.SpriteManager.getTexture(name);
                bounds = game.SpriteManager.getRectangle(name);
            }
            catch (Exception e)
            {
                texture = game.Content.Load<Texture2D>(name);
                bounds = texture.Bounds;
            }

            origin = bounds.Size.ToVector2() / 2f;

            
            // Se nao indicaram largura nem altura, dar erro
            if (scale == 0 && width == 0f && height == 0f)
                throw new Exception("Sprite constructor requires scale, width or height");

            if (scale > 0)
            {
                // FIXME: hardcoded que 80 pixeis sao uma unidade no overlap2d
                // pixeis / 80 => unidade
                // scale * pixeis / 80
                width = scale * bounds.Width / 80f;
                height = scale * bounds.Height / 80f;
            }
            
            
            if (width == 0f)
            {
                /* height -> bounds.height
                 * width -> bounds.width         */
                width = bounds.Width * height / bounds.Height;
            }

            if (height == 0f)
            {
                height = bounds.Height * width / bounds.Width;
            }

            size = new Vector2(width, height);
            position = new Vector2(0, 0);
            if (collides)
            {
                collider = new OBBCollider(game, name, position, size, rotation);
                collider.SetDebug(false);
                game.cManager.Add(collider);
            }
        }

        public Sprite ForceOrigin(Vector2 origem)
        {
            origin = origem;
            return this;
        }

        public Sprite SetPosition(Vector2 position)
        {
            this.position = position;
            collider?.SetPosition(position);
            return this;
        }

        public Sprite SetRotation(float rotation)
        {
            collider?.Rotate(rotation);
            this.rotation = rotation;
            return this;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture,
                new Rectangle(Camera.ToPixel(position).ToPoint(), Camera.ToLength(size).ToPoint()),
                bounds,
                _color,
                rotation,
                origin,
                SpriteEffects.None,
                0);

            collider?.Draw(null);
        }

        public virtual void Update(GameTime gameTime)
        {
        }
        public virtual void LateUpdate(GameTime gameTime)
        {
        }
    }
}