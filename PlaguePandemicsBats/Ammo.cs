using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlaguePandemicsBats
{
    public class Ammo : DrawableGameComponent
    {
        private Game1 _game;
        private Texture2D _texture;
        private SpriteManager _sprite;
        private Rectangle _rec;
        private SpriteBatch _spriteBatch;
        private OBBCollider _collider;
        private int _ammoCount = 10;

        public Ammo(Game1 game) : base(game)
        {
            _game = game;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _sprite = new SpriteManager(_game);
            _texture = _sprite.getTexture("cure");
            _rec = _sprite.getRectangle("cure");
        }

        public virtual void Update()
        {
            if (_collider._inCollision)
            {
                foreach (Collider c in _collider.collisions)
                {
                    if (c.Tag == "Player")
                    {
                        AddAmmo();
                        _game.Components.Remove(this);
                    }                   
                }
            }
        }
        public void AddAmmo()
        {
            _game.Player.AmmoQuantity += _ammoCount;
            _game.Ammo.Add(this);
        }
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_texture, _rec, Color.White);            
            _spriteBatch.End();
        }

    }
}
