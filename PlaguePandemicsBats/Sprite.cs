using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlaguePandemicsBats
{
    public class Sprite
    {
        private Texture2D texture; // Texture da SpriteSheet
        internal Rectangle bounds; // Posição na SpriteSheet
        private Vector2 origin;

        internal Vector2 size; // Tamanho em unidades "reais"
        internal Vector2 position; // Posição no ambiente "real"
        internal float rotation = 0f;
        internal Game1 _game;
        internal Color _color = Color.White;
        internal OBBCollider obbCollider;
        internal CircleCollider cCollider;
        internal AABBCollider aabbCollider;

        public Sprite(Game1 game, string name, ColliderType colliderType = ColliderType.Circle, float width = 0, float height = 0, float scale = 0, bool collides = true)
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
                if (colliderType == ColliderType.OBB)
                {
                    obbCollider = new OBBCollider(game, name, position, size, rotation);
                    obbCollider.SetDebug(true);
                    game.CollisionManager.Add(obbCollider);
                }
                else if (colliderType == ColliderType.AABB)
                {
                    aabbCollider = new AABBCollider(game, name, position, size);
                    aabbCollider.SetDebug(true);
                    game.CollisionManager.Add(aabbCollider);
                }
                else if (colliderType == ColliderType.Circle)
                {
                    //In a Circle Collider width = height
                    cCollider = new CircleCollider(game, name, position, size.X >= size.Y ? size.X : size.Y);
                    cCollider.SetDebug(true);
                    game.CollisionManager.Add(cCollider);
                }
                
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
            aabbCollider?.SetPosition(position);
            obbCollider?.SetPosition(position);
            cCollider?.SetPosition(position);
            return this;
        }

        public Sprite SetRotation(float rotation)
        {
            obbCollider?.Rotate(rotation);
            this.rotation = rotation;
            return this;
        }

        public void Draw(SpriteBatch spriteBatch)
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

            obbCollider?.Draw(null);
            aabbCollider?.Draw(null);
            cCollider?.Draw(null);
        }
    }
}