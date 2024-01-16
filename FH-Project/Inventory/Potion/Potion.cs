using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;

public abstract class Potion : Subject, Item
{
    protected int wert;
    protected Texture2D potionTexture;
    Random random = new Random();
    protected Player player;
    protected Vector2 pos;

    //Gibt an was für ein Potion (1= Health, 2=Shield, 3=Random)
    public int id;
    public Potion(Player player, Texture2D texture)
    {
        int id = 0;
        wert = Globals.Player.Health;
        potionTexture = texture;
        if (player != null)
        {
            this.player = player;
        }
        else
        {
            pos = new Vector2(100, 100);
        }
    }

    public void Draw()
    {
        if (player != null)
        {
            Globals.SpriteBatch.Draw(potionTexture, new Vector2(player.Position.X + player.EntityTexture.Width, player.Position.Y + player.EntityTexture.Height), null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
        }
        else
        {
            Globals.SpriteBatch.Draw(potionTexture, pos, null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
        }
    }


    public abstract void UseEffect();

    public override bool Equals(object obj)
    {
        return obj is Potion potion &&
               wert == potion.wert &&
               EqualityComparer<Random>.Default.Equals(random, potion.random) &&
               EqualityComparer<Player>.Default.Equals(player, potion.player);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(wert, random, player);
    }
}
