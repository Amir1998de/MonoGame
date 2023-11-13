using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;

public class RandomPotion : Potion
{
    Random random = new Random();
	public RandomPotion(int wert, Player player) : base(wert, player)
	{
        Inventory.AddItem(this);
	}

   

    public override void UseEffect()
    {

        int i = random.Next(0, 3);

        switch (i)
        {
            case 0:
                player.AddHealth(wert);
                break;
            case 1:
                player.ReduceHealth(wert);
                break;
            case 2:
                player.AddShield(wert);
                break;
            case 3:
                player.ReduceShield(wert);
                break;
            default:
                break;
        }
    }
}
