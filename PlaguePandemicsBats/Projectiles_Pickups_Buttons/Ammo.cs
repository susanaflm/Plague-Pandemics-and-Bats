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
        #region Private Variables
        private Game1 _game;
        private Sprite _ammoTex;
        private OBBCollider _collider;
        private int _ammoCount = 10;
        #endregion

        #region Constructor
        public Ammo(Game1 game, Vector2 position)
        {
            _game = game;

            _ammoTex = new Sprite(game, "cure", width: 0.1f);
            _ammoTex.SetPosition(position);

            _collider = new OBBCollider(game, "Ammo", position, _ammoTex.size, 0);
            game.CollisionManager.Add(_collider);
            _game.Ammo.Add(this);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Update Ammo
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //Check if the player collides with the ammo, if he does, add ammo
            if (_collider._inCollision)
            {
                foreach (Collider c in _collider.collisions)
                {
                    if (c.Tag == "Player")
                    {
                        _game.Player.AddAmmo(_ammoCount);
                        _game.Ammo.Remove(this);
                        _game.CollisionManager.Remove(_collider);
                    }                   
                }
            }
        }

        /// <summary>
        /// Draw the ammo sprite
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            _ammoTex.Draw(sb);
        }
        #endregion
    }
}
