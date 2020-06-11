using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaguePandemicsBats
{
    public static class Extensions
    {
        public static float Pos(this Vector2 vector, int i)
        {
            if (i == 0)
                return vector.X;
            else
                return vector.Y;
        }

        public static float DeltaTime(this GameTime gameTime)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        
    }
}
