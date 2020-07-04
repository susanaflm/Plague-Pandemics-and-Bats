using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace PlaguePandemicsBats
{
    public abstract class Collider
    {
        protected Game _game;
        protected bool _debug;
        
        internal Vector2 _position;
        internal bool _inCollision;
        internal List<Collider> collisions;
        public string Tag { get; private set; }
        protected Collider(Game game, string tag)
        {
            _debug = false;
            Tag = tag;
            _game = game;
            collisions = new List<Collider>();
            try { var _ = new Pixel(game); } catch (Exception) { }
        }

        internal void SetCollision(Collider collision)
        {
            _inCollision = true;
            collisions.Add(collision);
        }

        internal void EmptyCollisions()
        {
            _inCollision = false;
            collisions.Clear();
        }
        
        public void SetDebug(bool flag)
        {
            _debug = flag;
        }

        public virtual void SetPosition(Vector2 position)
        {
            _position = position;
        }

        public abstract void Draw(GameTime gameTime);
        public abstract void Rotate(float theta);

    }
}