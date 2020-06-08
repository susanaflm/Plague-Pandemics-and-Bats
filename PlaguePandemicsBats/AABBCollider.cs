using Microsoft.Xna.Framework;

namespace PlaguePandemicsBats
{
    // AABB - Axis Aligned Bounding Box
    public class AABBCollider : Collider
    {
        internal Rectangle _rectangle;
        
        public AABBCollider(Game game, string tag, Vector2 center, Vector2 size) : base(game, tag)
        {
            _position = center;
            _rectangle = new Rectangle((center - size / 2f).ToPoint(), size.ToPoint());
        }

        public OBBCollider ToObb()
        {
            return new OBBCollider(_game, Tag, _position, _rectangle.Size.ToVector2(), 0f);
        }
        
        public override void Draw(GameTime gameTime)
        {
            if (_debug)
            {
                Pixel.DrawRectangle(_rectangle, _inCollision ? Color.Red : Color.Blue);
            }
        }

        public override void Rotate(float theta)
        {
            throw new System.Exception("It's not possible to rotate an AABB");
        }

        public override void SetPosition(Vector2 position)
        {
            _position = position;
            _rectangle.Location = (position - _rectangle.Size.ToVector2() / 2f).ToPoint();
        }
    }
}