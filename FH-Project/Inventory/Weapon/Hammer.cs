using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;

public class Hammer : Weapon
{
    public Hammer(int damage, int speed, Texture2D texture) : base(damage, speed, texture)
    {
    }


    public override void UseEffect()
    {
        throw new NotImplementedException();
    }
}
