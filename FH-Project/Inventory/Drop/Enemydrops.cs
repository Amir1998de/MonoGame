using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;

public class Enemydrops : Item
{
    public static Texture2D EnemyDropTexture;
    private Vector2 position;
    public Rectangle DropBounds { get; private set;}
    public Enemydrops(Vector2 position)
    {
        this.position = position;
        DropBounds = new Rectangle((int)position.X,(int) position.Y, EnemyDropTexture.Width, EnemyDropTexture.Height);
    }

    public void UseEffect()
    {
        Inventory.AddItem(this);
    }

   

    public void Draw()
    {
        Globals.SpriteBatch.Draw(EnemyDropTexture, position, null, Color.White, 0, new(0, 0), 1, SpriteEffects.None, 0);
    }
}
