using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PlaguePandemicsBats
{
    public static class Vector2Extension
    {
        public static float Pos(this Vector2 vector, int i)
        {
            if (i == 0)
                return vector.X;
            else
                return vector.Y;
        }
    }
}
