using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace PlaguePandemicsBats
{
    public class Corona
    {
        #region Private variables
        private const float _coronaWidth = 5f;

        private Game1 _game;
        private CircleCollider _coronaCollider;

        private Vector2 _position;
        private Sprite _coronaSprite;
        private int _health;
        private float _timer;
        private float _shootTimer = 1f;
        private bool isDead = false;
        #endregion

        #region Constructor
        public Corona(Game1 game)
        {
            _game = game;
            _position = new Vector2(305, 300);

            _health = 1000;

            _coronaSprite = new Sprite(game, "borona",width: _coronaWidth);
            _coronaSprite.SetPosition(_position);

            _coronaCollider = new CircleCollider(game, "Corona", _position, _coronaSprite.size.X / 2);
            _coronaCollider.SetDebug(false);
            game.CollisionManager.Add(_coronaCollider);
        }
        #endregion

        #region Methods

        public void Update(GameTime gameTime)
        {
            //Check if the corona has died
            if (_health <= 0)
            {
                isDead = true;
            }

            //Sends projectiles towards the player
            Attack(gameTime);

            //If the corona is dead, then the gamestate is updated to win screen
            if (isDead)
            {
                _game.CoronaDied();
            }
        }

        /// <summary>
        /// Check collisions with the corona
        /// </summary>
        /// <param name="gameTime"></param>
        public void LateUpdate(GameTime gameTime)
        {
            if (_coronaCollider._inCollision)
            {
                if (_coronaCollider.collisions[0].Tag == "Player")
                {
                    _game.Player.Die();
                }
            }
        }

        /// <summary>
        /// This handles the corona projectiles
        /// </summary>
        /// <param name="gameTime"></param>
        public void Attack(GameTime gameTime)
        {
            _timer += gameTime.DeltaTime();

            if (_shootTimer - _timer <= 0 && Vector2.Distance(_position, _game.Player.Position) <= 13f)
            {
                Vector2 projOrientation = _game.Player.Position - _position;

                projOrientation.Normalize();

                new EnemyProjectile(_game, projOrientation, _position);

                _timer = 0;
            }
        }

        /// <summary>
        /// Deal Damage to the corona
        /// </summary>
        /// <param name="damage"></param>
        public void DealDamage(int damage)
        {
            _health -= damage;
        }


        /// <summary>
        /// Sets the Position of Corona
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        /// <summary>
        /// Draws the Corona
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch sb)
        {
            _coronaSprite.Draw(sb);
        }

        #endregion
    }
}

