using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FH_Project;

public static class CollisionHandler
{


    public static bool CollisionEntitys(Rectangle entity1, Rectangle entity2)
    {

        return entity1.Intersects(entity2);
    }

    public static bool CollisionWithEnviorment(Rectangle textureRectangle, Rectangle enityRectangle)
    {
        return enityRectangle.Intersects(textureRectangle);
    }
}
