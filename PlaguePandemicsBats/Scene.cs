using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Runtime.CompilerServices;

namespace PlaguePandemicsBats
{
    public class Scene
    {
        #region Private variables
        private const float deg2Reg = (float)Math.PI / 180f;
        private string _sceneName;
        private int gender;
        private Game1 _game;
        private SpriteBatch _spriteBatch;        
        private List<Sprite> _sprites;
        #endregion
        public Player Player { get; }
        #region Constructor
        public Scene(Game1 game, string sceneFile)
        {
            _game = game;
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            _sprites = new List<Sprite>();

            JObject json = JObject.Parse(File.ReadAllText($"Content/pandemics/scenes/{sceneFile}.dt"));
            //gives us jtoken bc they are different types of data, but i convert it to string
            _sceneName = json ["sceneName"].Value<string>();

            //starts reading on composite
            foreach (JToken image in json ["composite"] ["sImages"])
            {
                string imgName = image ["imageName"].Value<string>();
                //if there is no x then the x is taken as value 0
                float x = image ["x"]?.Value<float>() ?? 0f;
                float y = image ["y"]?.Value<float>() ?? 0f;
                float rotation = deg2Reg * (image ["rotation"]?.Value<float>() ?? 0f);
                float scaleX = image ["scaleX"]?.Value<float>() ?? 1;
                float scaleY = image ["scaleY"]?.Value<float>() ?? 1;
                float originX = image["originX"].Value<float>();
                float originY = image["originY"].Value<float>();

                if (image ["itemIdentifier"]?.Value<string>() == "Player")
                {
                    Player = new Player(_game, 1);
                    Player.SetPosition(new Vector2(x, y));
               
                }
                else if (image["imageName"].Value<string>() == "ZGirlD0")
                {
                    PinkZombie pinkZombie = new PinkZombie(_game, new Vector2(x, y));
                    _game.Enemies.Add(pinkZombie);
                }
                else
                {
                    Sprite sprite = new Sprite(_game, imgName, scale: new Vector2(scaleX, scaleY));
                    sprite.SetPosition(new Vector2(x, y));
                    sprite.SetRotation(rotation);
                    sprite.ForceOrigin(new Vector2(originX, originY));

                    _sprites.Add(sprite);

                }

            }
        }
        #endregion

        #region Properties
        public List<Sprite> Sprites => _sprites;
        #endregion

        #region Methods
        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (Sprite s in Sprites.ToArray())
            {
                s.Draw(_spriteBatch);
            }
            _spriteBatch.End();
        }
        #endregion
    }
}
