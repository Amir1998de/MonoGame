using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;

public class ShieldPotion : Potion
{
	public ShieldPotion(int wert, Player player) : base(wert, player)
	{
        Inventory.AddItem(this);
	}

    

    public override void UseEffect()
    {
        player.AddShield(wert);
        Inventory.RemoveItem(this);
    }
}
