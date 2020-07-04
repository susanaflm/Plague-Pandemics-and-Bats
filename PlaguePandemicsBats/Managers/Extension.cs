using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaguePandemicsBats
{
    public static class Extension
    {
        public static float Pos(this Vector2 vector, int i)
        {
            return i == 0 ? vector.X : vector.Y;
        }

        public static float DeltaTime(this GameTime gameTime)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public static float TotalTime(this GameTime gameTime)
        {
            return (float)gameTime.TotalGameTime.TotalSeconds;
        }
    }
}
