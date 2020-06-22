using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;

namespace PlaguePandemicsBats
{
    class SpriteInfo
    {
        public Rectangle bounds { get; private set; }
        public string fileName { get; private set; }

        public SpriteInfo(string name, Point position, Point size)
        {
            fileName = name;
            bounds = new Rectangle(position, size);
        }
    }

    public class SpriteManager
    {
        private Game _game;
        private Dictionary<string, Texture2D> _textures;
        private Dictionary<string, SpriteInfo> _infos;

        public SpriteManager(Game game)
        {
            _game = game;
            _textures = new Dictionary<string, Texture2D>();
            _infos = new Dictionary<string, SpriteInfo>();
        }

        public void AddSpriteSheet(string name)
        {
            if (_textures.ContainsKey(name)) { throw new Exception($"Loading duplicated sprite sheet '{name}'"); }
            _textures [name] = _game.Content.Load<Texture2D>(name);

            JObject jsonFile = JObject.Parse(File.ReadAllText($"{_game.Content.RootDirectory}/{name}.json"));
            JObject frames = jsonFile ["frames"] as JObject;

            foreach (JProperty property in frames.Properties())
            {
                JObject frame = property.Value ["frame"] as JObject;
                int x = frame.GetValue("x").ToObject<int>();
                int y = frame.GetValue("y").ToObject<int>();
                int w = frame.GetValue("w").ToObject<int>();
                int h = frame.GetValue("h").ToObject<int>();
                string frameName = property.Name;

                SpriteInfo info = new SpriteInfo(name, new Point(x, y), new Point(w, h));

                if (_infos.ContainsKey(frameName)) { throw new Exception($"Duplicated sprite name '{frameName}'"); }
                _infos [frameName] = info;
            }
        }

        public Texture2D getTexture(string objectName)
        {
            if (!_infos.ContainsKey(objectName)) throw new Exception($"No object named {objectName}");

            SpriteInfo info = _infos [objectName];

            string textureName = info.fileName;

            if (!_textures.ContainsKey(textureName)) throw new Exception($"No texture for {textureName}");

            return _textures [textureName];
        }

        public Rectangle getRectangle(string objectName)
        {
            if (!_infos.ContainsKey(objectName)) throw new Exception($"No object named {objectName}");

            SpriteInfo info = _infos [objectName];

            return info.bounds;
        }
    }
}