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
    public class Ammo
    {
        private Game1 _game;
        private Sprite _ammoTex;
        private OBBCollider _collider;
        private int _ammoCount = 10;

        public Ammo(Game1 game, Vector2 position)
        {
            _game = game;

            _ammoTex = new Sprite(game, "cure", width: 0.1f);
            _ammoTex.SetPosition(position);

            _collider = new OBBCollider(game, "Ammo", position, _ammoTex.size, 0);
            game.CollisionManager.Add(_collider);
            _game.Ammo.Add(this);
        }

        public void Update()
        {
            if (_collider._inCollision)
            {
                foreach (Collider c in _collider.collisions)
                {
                    if (c.Tag == "Player")
                    {
                        AddAmmo();
                        _game.Ammo.Remove(this);
                        _game.CollisionManager.Remove(_collider);
                    }                   
                }
            }
        }

        public void AddAmmo()
        {
            _game.Player.AddAmmo(_ammoCount);
        }

        public void Draw(SpriteBatch sb)
        {
            _ammoTex.Draw(sb);
        }
    }
}
