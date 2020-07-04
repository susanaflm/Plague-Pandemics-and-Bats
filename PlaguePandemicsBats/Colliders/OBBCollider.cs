using System;
using Microsoft.Xna.Framework;

namespace PlaguePandemicsBats
{
    public class OBBCollider : Collider
    {
        // radians
        private float _rotation; 
        internal Vector2 _extends;
        internal Vector2 _orientation1;
        internal Vector2 _orientation2 => new Vector2(-_orientation1.Y, _orientation1.X);

        #region Constructor
        public OBBCollider(Game game, string tag, Vector2 center, Vector2 size, float rotation) : base(game, tag)
        {
            _rotation = rotation ;
            _position = center;
            _extends = size / 2f;
            UpdateOrientation();
        }
        #endregion

        #region Methods
        void UpdateOrientation()
        {
            _orientation1 = new Vector2((float)Math.Cos(_rotation), (float)-Math.Sin(_rotation)); 
        }

        public override void Draw(GameTime gameTime)
        {
            if (_debug)
            {
                Vector2 pt1 = _position + _orientation1 * _extends.X + _orientation2 * _extends.Y;
                Vector2 pt2 = _position + _orientation1 * _extends.X - _orientation2 * _extends.Y;
                Vector2 pt3 = _position - _orientation1 * _extends.X + _orientation2 * _extends.Y;
                Vector2 pt4 = _position - _orientation1 * _extends.X - _orientation2 * _extends.Y;
                Color color = _inCollision ? Color.Red : Color.Blue;
                
                Pixel.DrawLine(pt1, pt2, color);
                Pixel.DrawLine(pt1, pt3, color);
                Pixel.DrawLine(pt2, pt4, color);
                Pixel.DrawLine(pt3, pt4, color);
            }
        }

        public override void Rotate(float rotation)
        {
            _rotation = rotation;
            UpdateOrientation();
        }
        #endregion
    }
}