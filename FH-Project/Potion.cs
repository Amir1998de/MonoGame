using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;

public abstract class Potion : Subject ,Item
{
	protected int wert;
	Random random = new Random();
	protected Player player;
	public Potion(int wert, Player player)
	{
		this.wert = wert;
		this.player = player;
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
