using Microsoft.Xna.Framework;

namespace PlaguePandemicsBats
{
    public class CircleCollider : Collider
    {

        internal float _radius;

        public CircleCollider(Game game, string tag, Vector2 center, float radius) : base(game, tag)
        {
            _position = center;
            _radius = radius;
        }

        public override void Draw(GameTime gameTime)
        {
            if (_debug)
            {
                Pixel.DrawCircle(_position, _radius, _inCollision ? Color.Red : Color.Blue);
            }
        }

        public override void Rotate(float theta)
        {
            // Rotate circle is the same circle
        }
    }
}