using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;

public class Sword : Weapon
{
	public Sword(int damage, int speed, Texture2D texture) : base(damage, speed, texture)
	{

	}

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, null, Color.White, 0, new Vector2(0, 0), 0.25f, SpriteEffects.None, 0);
    }
}
