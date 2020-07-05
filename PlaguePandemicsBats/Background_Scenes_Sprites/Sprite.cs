using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlaguePandemicsBats
{
    public class Sprite
    {
        #region Variables
        private Texture2D texture; 
        internal Rectangle bounds; 
        private Vector2 origin;

        internal string _spriteName;
        internal Vector2 size; // Tamanho em unidades "reais"
        internal Vector2 position; // Posição no ambiente "real"
        internal float rotation = 0f;
        internal Game1 _game;
        internal Color _color = Color.White;
        internal OBBCollider obbCollider;
        internal CircleCollider cCollider;
        internal AABBCollider aabbCollider;
        #endregion

        #region Constructor
        public Sprite(Game1 game, string name, ColliderType colliderType = ColliderType.OBB, float width = 0, float height = 0, Vector2? scale = null, bool collides = false)
        {
            _game = game;
            _spriteName = name;

            try
            {
                texture = game.SpriteManager.getTexture(name);
                bounds = game.SpriteManager.getRectangle(name);
            }
            catch (Exception)
            {
                texture = game.Content.Load<Texture2D>(name);
                bounds = texture.Bounds;
            }

            origin = bounds.Size.ToVector2() / 2f;
            
            if (!scale.HasValue && width == 0f && height == 0f)
                throw new Exception("Sprite constructor requires scale, width or height");

            if (scale.HasValue)
            {
                width = scale.Value.X * bounds.Width / 80f;
                height = scale.Value.Y * bounds.Height / 80f;
            }

            if (width == 0f)
            {
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
                    obbCollider = new OBBCollider(game, "Obstacle", position, size, rotation);
                    obbCollider.SetDebug(false);
                    game.CollisionManager.Add(obbCollider);
                }
                else if (colliderType == ColliderType.AABB)
                {
                    aabbCollider = new AABBCollider(game, name, position, size);
                    aabbCollider.SetDebug(false);
                    game.CollisionManager.Add(aabbCollider);
                }
                else if (colliderType == ColliderType.Circle)
                {
                    //In a Circle Collider width = height
                    cCollider = new CircleCollider(game, name, position, size.X >= size.Y ? size.X / 2f : size.Y / 2f);
                    cCollider.SetDebug(false);
                    game.CollisionManager.Add(cCollider);
                }
                
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Forces de Origin of the Sprite
        /// </summary>
        /// <param name="origem"></param>
        /// <returns></returns>
        public Sprite ForceOrigin(Vector2 origem)
        {
            origin = origem;
            return this;
        }

        /// <summary>
        /// Sets the position of the Sprite
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Sprite SetPosition(Vector2 position)
        {
            this.position = position;
            aabbCollider?.SetPosition(position);
            obbCollider?.SetPosition(position);
            cCollider?.SetPosition(position);
            return this;
        }

        /// <summary>
        /// Sets Rotation of the Sprite
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public Sprite SetRotation(float rotation)
        {
            obbCollider?.Rotate(rotation);
            this.rotation = rotation;
            return this;
        }

        /// <summary>
        /// Draw the sprite
        /// </summary>
        /// <param name="spriteBatch"></param>
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
        #endregion
    }
}